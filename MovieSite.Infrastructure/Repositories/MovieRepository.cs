﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieSite.Application.DTO.Responses;
using MovieSite.Application.Interfaces.Repositories;
using MovieSite.Domain.Models;

namespace MovieSite.Infrastructure.Repositories
{
    public class MovieRepository : RepositoryAsync<Movie>, IMovieRepository
    {
        private readonly DbContext _dbContext;

        public MovieRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsContains(int movieId)
        {
            return await _dbContext.Set<Movie>().AnyAsync(movie => movie.Id == movieId);
        }
        
        public new async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Set<Movie>().AsNoTracking().ToListAsync();
        }
        
        public async Task<Movie> FindByTitleAsync(string title)
        {
            return await _dbContext.Set<Movie>().FirstOrDefaultAsync(movie => movie.Title == title);
        }
        
        public async Task<Movie> FindByTitleForUpdateAsync(string title)
        {
            return await _dbContext.Set<Movie>()
                .Include(movie => movie.GenreMovies)
                .FirstOrDefaultAsync(movie => movie.Title == title);
        }

        public async Task<Movie> GetMovieWithRatings(int movieId)
        {
            return await _dbContext.Set<Movie>()
                .Include(movie => movie.MovieRatings)
                .FirstOrDefaultAsync(movie => movie.Id == movieId);
        }
        
        public async Task<IList<MovieRating>> GetMovieRating(int movieId)
        {
            return await _dbContext.Set<Movie>()
                .Where(movie => movie.Id == movieId)
                .Select(movie => movie.MovieRatings)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public void SetMovieRatingIsModified(Movie movie)
        {
            _dbContext.Entry(movie).Property(movie1 => movie1.Rating).IsModified = true;
        }
        
        public async Task<IReadOnlyList<MovieCommentsResponse>> GetMovieWithComments(int movieId)
        {
            var movieCommentsResponse = from movie in _dbContext.Set<Movie>()
                where movie.Id == movieId
                join comments in _dbContext.Set<Comment>() on movie.Id equals comments.MovieId
                join user in _dbContext.Set<User>() on comments.UserId equals user.Id
                select new MovieCommentsResponse
                {
                    CommentId = comments.Id, 
                    CommentText =comments.Text,
                    UserName = user.UserName, 
                    UserAvatar = Encoding.UTF8.GetString(user.Avatar)
                };

            return (await movieCommentsResponse.ToListAsync()).AsReadOnly();
        }
    }
}
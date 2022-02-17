﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MovieSite.Application.ViewModels;
using MovieSite.Domain.Models;

namespace MovieSite.Application.Interfaces.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie> FindByTitleAsync(string title);
        Task<Movie> FindByTitleForUpdateAsync(string title);
        Task<IList<MovieRating>> GetMovieRating(int movieId);
        void SetMovieRatingIsModified(Movie movie);
        Task<Movie> GetMovieWithRatings(int movieId);
        Task<IReadOnlyList<MovieCommentsResponse>> GetMovieWithCommentsAsync(int movieId);
        Task<MovieWithGenresResponse> GetMovieWithGenresByIdAsync(int movieId);
    }
}
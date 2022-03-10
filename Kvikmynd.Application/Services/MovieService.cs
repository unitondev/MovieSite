﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Kvikmynd.Application.Common.Enums;
using Kvikmynd.Application.Common.Helper;
using Kvikmynd.Application.Common.Services;
using Kvikmynd.Application.Interfaces.Repositories;
using Kvikmynd.Application.Interfaces.Services;
using Kvikmynd.Application.Models;
using Kvikmynd.Application.ViewModels;
using Kvikmynd.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Kvikmynd.Application.Services
{
    public class MovieService : GenericService<Movie>, IMovieService, IDisposable, IAsyncDisposable
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork work, IMapper mapper) : base(work.MovieRepository, work)
        {
            _work = work;
            _mapper = mapper;
        }
        
        public async Task<MoviesWithGenresAndRatingsViewModel> GetAllMoviesAsync(PaginationParametersModel paginationParameters)
        {
            var movies = await _work.MovieRepository.GetMovieWithGenresAndRatingsAsync(paginationParameters);

            var moviesViewModels = _mapper.Map<List<MovieWithGenresAndRatingsModel>, 
                List<MovieWithGenresAndRatingsViewModel>>(movies.Items);

            return new MoviesWithGenresAndRatingsViewModel
            {
                Items = moviesViewModels,
                TotalCount = movies.TotalCount
            };
        }
        
        public async Task<MovieWithGenresViewModel> GetMovieWithGenresByIdAsync(int id)
        {
            var entityModel = await _work.MovieRepository.GetMovieWithGenresByIdAsync(id);
            
            return _mapper.Map<MovieWithGenresModel, MovieWithGenresViewModel>(entityModel);
        }
        
        public async Task<ServiceResult<Movie>> CreateMovieAsync(MovieModel model)
        {
            var createdMovie = await _work.MovieRepository.FindAsync(m => m.Title == model.Title);
            if (createdMovie != null)
            {
                return new ServiceResult<Movie>(ErrorCode.MovieAlreadyExists);
            }

            List<Genre> genres = new List<Genre>();
            
            foreach (var requestedGenre in model.Genres)
            {
                var genre = await _work.GenreRepository.FindAsync(g => g.Name == requestedGenre);
                if (genre == null)
                {
                    return new ServiceResult<Movie>(ErrorCode.GenreNotFound);
                }

                genres.Add(genre);
            }
            
            createdMovie = _mapper.Map<MovieModel, Movie>(model);

            foreach (var genre in genres)
            {
                createdMovie.GenreMovies.Add(new GenreMovie
                {
                    Movie = createdMovie,
                    Genre = genre,
                });
            }

            var result = await _work.MovieRepository.CreateAsync(createdMovie);
            await _work.CommitAsync();

            return new ServiceResult<Movie>(result);
        }
        
        public async Task<ServiceResult<Movie>> UpdateMovieAsync(EditMovieModel model)
        {
            // TODO split for update movie and split genres for movie
            var updatedMovie = await _work.MovieRepository.All(m => m.GenreMovies)
                .Where(m => m.Title == model.Title)
                .FirstOrDefaultAsync();

            if (updatedMovie == null)
            {
                return new ServiceResult<Movie>(ErrorCode.MovieNotFound);
            }

            List<Genre> genres = new List<Genre>();
            
            foreach (var requestedGenre in model.Genres)
            {
                var genre = await _work.GenreRepository.FindAsync(g => g.Name == requestedGenre);
                if (genre == null)
                {
                    return new ServiceResult<Movie>(ErrorCode.GenreNotFound);
                }

                genres.Add(genre);
            }

            _mapper.Map<EditMovieModel, Movie>(model, updatedMovie);
            
            foreach (var genre in genres)
            {
                updatedMovie.GenreMovies.Add(new GenreMovie
                {
                    Movie = updatedMovie,
                    Genre = genre,
                });
            }
            
            await _work.MovieRepository.UpdateAsync(updatedMovie);
            await _work.CommitAsync();

            return new ServiceResult<Movie>(updatedMovie);
        }

        public async Task<ServiceResult<IEnumerable<MovieRating>>> GetMovieRatings(int id)
        {
            if (!await ExistsAsync(id))
            {
                return new ServiceResult<IEnumerable<MovieRating>>(ErrorCode.MovieNotFound);
            }

            var movieRatings = await _work.MovieRepository.All()
                .Where(m => m.Id == id)
                .Select(m => m.MovieRatings)
                .FirstOrDefaultAsync();

            return new ServiceResult<IEnumerable<MovieRating>>(movieRatings);
        }

        public async Task<ServiceResult<List<MovieCommentsViewModel>>> GetMovieComments(int id)
        {
            if (!await ExistsAsync(id))
            {
                return new ServiceResult<List<MovieCommentsViewModel>>(ErrorCode.MovieNotFound);
            }

            var movieComments = await _work.MovieRepository.GetMovieCommentsAsync(id);
            
            return new ServiceResult<List<MovieCommentsViewModel>>(movieComments);
        }

        public void Dispose()
        {
            _work.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _work.DisposeAsync();
        }
        
        #region Temp solution for movie covers

        public async Task PopulateMoviesCoversAsync()
        {
            var coversPaths = new[]
            {
                @"../Kvikmynd.Infrastructure/Covers/fightClub.jpg",
                @"../Kvikmynd.Infrastructure/Covers/americanPsycho.jpg",
                @"../Kvikmynd.Infrastructure/Covers/PulpFiction.jpg",
                @"../Kvikmynd.Infrastructure/Covers/memento.jpg",
                @"../Kvikmynd.Infrastructure/Covers/2001.jpg",
                @"../Kvikmynd.Infrastructure/Covers/NoCountryForOldMen.jpg",
                @"../Kvikmynd.Infrastructure/Covers/28DaysLater.jpg",
                @"../Kvikmynd.Infrastructure/Covers/TheGirlWithTheDragonTattoo.jpg",
                @"../Kvikmynd.Infrastructure/Covers/Dunkirk.jpg",
                @"../Kvikmynd.Infrastructure/Covers/Thursday.jpg",
                @"../Kvikmynd.Infrastructure/Covers/Scarface.jpg",
                @"../Kvikmynd.Infrastructure/Covers/TheMatrix.jpg",
                @"../Kvikmynd.Infrastructure/Covers/CatchMeIfYouCan.jpg",
                @"../Kvikmynd.Infrastructure/Covers/Se7en.jpg",
                @"../Kvikmynd.Infrastructure/Covers/TheShining.jpg",
                @"../Kvikmynd.Infrastructure/Covers/12AngryMen.jpg",
                @"../Kvikmynd.Infrastructure/Covers/AmericanHistoryX.jpg",
                @"../Kvikmynd.Infrastructure/Covers/OneFlewOvertheCuckoosNest.jpg",
                @"../Kvikmynd.Infrastructure/Covers/LockStockandTwoSmokingBarrels.jpg",
                @"../Kvikmynd.Infrastructure/Covers/WhoAmI.jpg",
                @"../Kvikmynd.Infrastructure/Covers/ThereWillBeBlood.jpg",
                @"../Kvikmynd.Infrastructure/Covers/TaxiDriver.jpg",
                @"../Kvikmynd.Infrastructure/Covers/TheThing.jpg"
            };

            for (var i = 0; i < coversPaths.Length; i++)
            {
                var movie = await _work.MovieRepository.FindByKeyAsync(i + 1);
                if (movie.Cover != null && movie.Cover.Length == 0)
                {
                    movie.Cover = Encoding.UTF8.GetBytes("data:image/jpeg;base64," + Base64Coder.EncodeImageToString(coversPaths[i]));
                }
            }
            
            await _work.CommitAsync();
        }
        
        #endregion
    }
}
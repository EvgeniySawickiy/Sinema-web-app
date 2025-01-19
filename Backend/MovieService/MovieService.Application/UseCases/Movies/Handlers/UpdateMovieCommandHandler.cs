using MediatR;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMovieCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {request.Id} not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                movie.UpdateTitle(request.Title);
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                movie.UpdateDescription(request.Description);
            }

            if (request.DurationInMinutes.HasValue)
            {
                movie.UpdateDuration(request.DurationInMinutes.Value);
            }

            if (request.Rating.HasValue)
            {
                movie.UpdateRating(request.Rating.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                movie.UpdateImage(request.ImageUrl);
            }

            if (!string.IsNullOrWhiteSpace(request.TrailerUrl))
            {
                movie.UpdateTrailer(request.TrailerUrl);
            }

            if (request.GenreIds != null)
            {
                movie.MovieGenres.Clear();

                var genres = await _unitOfWork.Genres.FindAsync(
                    g => request.GenreIds.Contains(g.Id), cancellationToken);

                foreach (var genre in genres)
                {
                    movie.MovieGenres.Add(new MovieGenre(movie.Id, genre.Id));
                }
            }

            await _unitOfWork.Movies.UpdateAsync(movie, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

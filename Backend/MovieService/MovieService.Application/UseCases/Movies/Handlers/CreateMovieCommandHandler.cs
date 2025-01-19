using MediatR;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMovieCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var genres = await _unitOfWork.Genres.FindAsync(
                g => request.GenreIds.Contains(g.Id), cancellationToken);

            if (!genres.Any())
            {
                throw new ArgumentException("Invalid genres provided.");
            }

            var movie = new Movie(
                title: request.Title,
                description: request.Description,
                durationInMinutes: request.DurationInMinutes,
                rating: request.Rating,
                imageUrl: request.ImageUrl,
                trailerUrl: request.TrailerUrl);

            foreach (var genre in genres)
            {
                movie.MovieGenres.Add(new MovieGenre(movie.Id, genre.Id));
            }

            await _unitOfWork.Movies.AddAsync(movie, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }
    }
}
using MediatR;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Core.Enums;
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

            if (!string.IsNullOrEmpty(request.Title))
            {
                movie.UpdateTitle(request.Title);
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                movie.UpdateDescription(request.Description);
            }

            if (request.DurationInMinutes.HasValue)
            {
                movie.UpdateDuration(request.DurationInMinutes.Value);
            }

            if (!string.IsNullOrEmpty(request.Genre))
            {
                movie.UpdateGenre(Enum.Parse<Genre>(request.Genre, true));
            }

            if (request.Rating.HasValue)
            {
                movie.UpdateRating(request.Rating.Value);
            }

            await _unitOfWork.Movies.UpdateAsync(movie, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

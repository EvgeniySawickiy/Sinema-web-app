using MediatR;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Core.Entities;
using MovieService.Core.Enums;
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
            var movie = new Movie(
                title: request.Title,
                description: request.Description,
                durationInMinutes: request.DurationInMinutes,
                genre: Enum.Parse<Genre>(request.Genre, true),
                rating: request.Rating);

            await _unitOfWork.Movies.AddAsync(movie, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }
    }
}

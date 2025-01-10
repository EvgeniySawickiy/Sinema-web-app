using MediatR;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMovieCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {request.Id} not found.");
            }

            await _unitOfWork.Movies.DeleteAsync(movie, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

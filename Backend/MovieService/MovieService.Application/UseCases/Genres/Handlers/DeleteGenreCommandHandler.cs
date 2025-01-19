using MediatR;
using MovieService.Application.UseCases.Genres.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Genres.Handlers;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);

        if (genre == null)
        {
            throw new KeyNotFoundException($"Genre with ID {request.Id} not found.");
        }

        await _unitOfWork.Genres.DeleteAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
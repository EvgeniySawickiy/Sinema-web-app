using MediatR;
using MovieService.Application.UseCases.Genres.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Genres.Handlers;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);

        if (genre == null)
            throw new KeyNotFoundException($"Genre with ID {request.Id} not found.");

        if (!string.IsNullOrEmpty(request.Name))
        {
            genre.UpdateName(request.Name);
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            genre.UpdateDescription(request.Description);
        }

        await _unitOfWork.Genres.UpdateAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
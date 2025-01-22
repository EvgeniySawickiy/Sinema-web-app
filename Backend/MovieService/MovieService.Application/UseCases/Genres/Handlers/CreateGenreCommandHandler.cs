using MediatR;
using MovieService.Application.UseCases.Genres.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Genres.Handlers;

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = new Genre(request.Name, request.Description);

        await _unitOfWork.Genres.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return genre.Id;
    }
}
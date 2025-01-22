using MediatR;
using MovieService.Application.DTO.Genre;
using MovieService.Application.UseCases.Genres.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Genres.Handlers;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGenreByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);

        if (genre == null)
            throw new KeyNotFoundException($"Genre with ID {request.Id} not found.");

        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }
}
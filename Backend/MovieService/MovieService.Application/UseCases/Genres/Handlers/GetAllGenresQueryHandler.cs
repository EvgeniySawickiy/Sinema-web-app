using MediatR;
using MovieService.Application.DTO.Genre;
using MovieService.Application.UseCases.Genres.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Genres.Handlers;

public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _unitOfWork.Genres.GetAllAsync(cancellationToken);

        return genres.Select(genre => new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description,
        });
    }
}
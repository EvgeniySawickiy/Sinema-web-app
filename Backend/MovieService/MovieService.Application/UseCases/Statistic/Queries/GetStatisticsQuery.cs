using MediatR;
using MovieService.Application.DTO;

namespace MovieService.Application.UseCases.Statistic.Queries;

public class GetStatisticsQuery : IRequest<StatisticsDto>
{
}

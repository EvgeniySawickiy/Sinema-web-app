using MediatR;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class CreateHallCommandHandler : IRequestHandler<CreateHallCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateHallCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateHallCommand request, CancellationToken cancellationToken)
        {
            var hall = new Hall(
                name: request.Name,
                totalSeats: request.TotalSeats);

            await _unitOfWork.Halls.AddAsync(hall, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return hall.Id;
        }
    }
}

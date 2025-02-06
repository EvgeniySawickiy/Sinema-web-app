using AutoMapper;
using MediatR;
using MovieService.Application.Mappers;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    internal class UpdateShowtimeCommandHandler : IRequestHandler<UpdateShowtimeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateShowtimeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showTime = await _unitOfWork.Showtimes.GetByIdAsync(request.Id, cancellationToken);

            _mapper.Map(request, showTime);
            Console.WriteLine(showTime);

            await _unitOfWork.Showtimes.UpdateAsync(showTime, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
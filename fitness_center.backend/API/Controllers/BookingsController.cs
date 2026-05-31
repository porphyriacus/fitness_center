using API.Extensions;
using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Bookings.Commands.MarkAttendance;
using Application.Features.Bookings.Commands.MarkNoShow;
using Application.Features.Bookings.Queries.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public BookingsController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Получить детали брони (доступно админу и владельцу брони)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        /// <summary>
        /// отмена брони клиентом(в контроллере клиента) или админом(все же лучше только через клиента)
        /// </summary>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var command = new CancelBookingCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// отметить посещение
        /// </summary>
        [HttpPut("{id}/attend")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAttendance(int id)
        {
            var command = new MarkAttendanceCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// отметить неявку (админ/тренер)
        /// </summary>
        [HttpPut("{id}/noshow")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkNoShow(int id)
        {
            var command = new MarkNoShowCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}

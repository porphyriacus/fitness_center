using API.Extensions;
using Application.Features.Specializations.Commands.Create;
using Application.Features.Specializations.Commands.DeleteSpecialization;
using Application.Features.Specializations.Queries.GetSpecializationsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    namespace API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize(Roles = "Admin")]
        public class SpecializationsController : ControllerBase
        {
            private readonly IMediator _mediator;

            public SpecializationsController(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// получить список всех специализаций
            /// </summary>
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var query = new GetSpecializationsQuery();
                var result = await _mediator.Send(query);
                return result.ToActionResult();
            }

            /// <summary>
            /// создать новую специализацию
            /// </summary>
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateSpecializationCommand command)
            {
                var result = await _mediator.Send(command);
                return result.ToActionResult();
            }

            /// <summary>
            /// удалить специализацию
            /// </summary>
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var command = new DeleteSpecializationCommand(id);
                var result = await _mediator.Send(command);
                return result.ToActionResult();
            }
        }
    }
}

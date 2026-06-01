using Application.Features.MembershipTypes;
using Application.Features.MembershipTypes.Queries.GetMembershipTypesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages.MembershipTypes
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public List<MembershipTypeDto> MembershipTypes { get; set; } = new();

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            var query = new GetMembershipTypesListQuery { SortBy = "Price", SortDescending = false };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                MembershipTypes = result.Value.ToList();
        }
    }
}
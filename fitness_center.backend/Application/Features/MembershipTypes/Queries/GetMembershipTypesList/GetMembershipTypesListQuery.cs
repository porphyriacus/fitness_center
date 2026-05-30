using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Queries.GetMembershipTypesList
{
    public class GetMembershipTypesListQuery : IRequest<Result<IReadOnlyList<MembershipTypeDto>>>
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }      // "Name", "Price", "ValidityDays"
        public bool SortDescending { get; set; }
    }
}

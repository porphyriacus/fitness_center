using Application.Common.Models;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Trainers.DTOs;
using AutoMapper;
using Core.Entities;
using MediatR;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Application.Features.Trainers.Queries.GetTrainersList
{
    public class GetTrainersListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetTrainersListQuery, Result<IReadOnlyList<TrainerDto>>>
    {
        public async Task<Result<IReadOnlyList<TrainerDto>>> Handle(GetTrainersListQuery request, CancellationToken cancellationToken)
        {
            List<Expression<Func<Trainer, bool>>>? filters = null;
            if (!String.IsNullOrEmpty(request.SearchTerm)) {

                string search = request.SearchTerm.ToLower();
                if (!String.IsNullOrEmpty(request.SearchField))
                {
                    Expression<Func<Trainer, bool>>? filter = request.SearchField.ToLower() switch
                    {
                        "name" => tr => tr.Name.ToLower().Contains(search),
                        "surname" => tr => tr.Surname.ToLower().Contains(search),
                        "specialization" => tr => tr.Specialization.Name.ToLower().Contains(search),  
                        _ => null
                    };
                    if(filter != null)
                        filters?.Add(filter);
                }
                else
                {
                    filters?.Add( tr => tr.Name.ToLower().Contains(search) 
                                || tr.Surname.ToLower().Contains(search));
                }
            }
            Func<IQueryable<Trainer>, IOrderedQueryable<Trainer>>? orderBy = null;
            if (!String.IsNullOrEmpty(request.SortBy))
            {
                orderBy = request.SortBy.ToLower() switch
                {
                    "name" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.Name)
                        : q => q.OrderBy(c => c.Name),
                    "surname" => request.SortDescending
                        ? q => q.OrderByDescending(c => c.Surname)
                        : q => q.OrderBy(c => c.Surname),
                    _ => q => q.OrderBy(c => c.Id)
                };
            }

            var traibers = await unitOfWork.TrainerRepository.ListAsync(orderBy, filters, cancellationToken, includesProperties: t => t.Specialization);
            var dtos = mapper.Map<List<TrainerDto>>(traibers);

            return Result<IReadOnlyList<TrainerDto>>.Ok(dtos);
        }
    }
}

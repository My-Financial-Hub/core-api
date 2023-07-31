using AutoMapper;
using FinancialHub.Domain.Interfaces.Mappers;

namespace FinancialHub.Domain.Mappers
{
    public class FinancialHubMapperWrapper : IMapperWrapper
    {
        private readonly IMapper mapper;

        public FinancialHubMapperWrapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Y Map<T, Y>(T ent)
        {
            return this.mapper.Map<Y>(ent);
        }

        public Y Map<Y>(object ent)
        {
            return this.mapper.Map<Y>(ent);
        }
    }
}

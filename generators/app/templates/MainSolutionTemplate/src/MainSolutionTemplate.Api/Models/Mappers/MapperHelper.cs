using AutoMapper;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Api.Models.Mappers
{
    public static class MapperHelper
    {
        public static IMappingExpression<T1,T2> MapToDal<T1,T2>(this IMappingExpression<T1,T2> createMap) where T2: BaseDalModelWithId
        {
            createMap.ForMember(x => x.Id, opt => opt.Ignore())
                     .ForMember(x => x.CreateDate, opt => opt.Ignore())
                     .ForMember(x => x.UpdateDate, opt => opt.Ignore());

            return createMap;
        }
    }
}
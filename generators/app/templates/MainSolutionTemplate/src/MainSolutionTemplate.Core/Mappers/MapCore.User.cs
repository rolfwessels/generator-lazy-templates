using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.Mappers
{
    public static partial class MapCore
	{
        public static void CreateUserMap()
        {
            Mapper.CreateMap<User, UserReference>();
        }

        public static UserReference ToReference(this User user, UserReference userReference = null)
        {
            return Mapper.Map(user, userReference);
        }
	}
}
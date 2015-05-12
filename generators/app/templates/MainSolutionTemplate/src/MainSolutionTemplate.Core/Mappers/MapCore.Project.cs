using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.Mappers
{
    public static partial class MapCore
	{
        private static void MapProject()
        {
            Mapper.CreateMap<Project, ProjectReference>();
        }

        public static ProjectReference ToReference(this Project project, ProjectReference projectReference = null)
        {
            return Mapper.Map(project, projectReference);
        }
	}
}
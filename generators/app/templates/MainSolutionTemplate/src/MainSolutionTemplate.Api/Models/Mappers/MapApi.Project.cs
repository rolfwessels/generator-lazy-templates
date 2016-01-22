using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Enums;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Models.Mappers
{
	public static partial class MapApi
	{
		private static void  MapProjectModel()
		{
			Mapper.CreateMap<Project, ProjectModel>();
            Mapper.CreateMap<Project, ProjectReferenceModel>();
			Mapper.CreateMap<ProjectReference, ProjectReferenceModel>();
			Mapper.CreateMap<ProjectCreateUpdateModel, Project>().MapToDal();

		}

		public static Project ToDal(this ProjectCreateUpdateModel model, Project project = null)
		{
			return Mapper.Map(model, project);
		}

		public static ProjectModel ToModel(this Project project, ProjectModel model = null)
		{
			return Mapper.Map(project, model);
		}

	    public static IEnumerable<ProjectReferenceModel> ToReferenceModelList(IQueryable<Project> projects)
	    {
            return Mapper.Map<IQueryable<Project>, IEnumerable<ProjectReferenceModel>>(projects);
	    }

        public static IEnumerable<ProjectModel> ToModelList(IQueryable<Project> projects)
	    {
            return Mapper.Map<IQueryable<Project>, IEnumerable<ProjectModel>>(projects);
	    }
	}
}
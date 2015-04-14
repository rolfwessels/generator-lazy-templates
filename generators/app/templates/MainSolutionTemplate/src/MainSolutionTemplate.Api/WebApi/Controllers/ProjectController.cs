using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
    /// <summary>
	///     Api controller for managing all the project
	/// </summary>
    [RoutePrefix(RouteHelper.ProjectController)]
	public class ProjectController : ApiController, IProjectControllerActions, IProjectControllerLookups
    {
	    private readonly ProjectCommonController _projectCommonController;
	    
        public ProjectController(ProjectCommonController projectCommonController)
        {
            _projectCommonController = projectCommonController;
        }

        /// <summary>
        ///     Returns list of all the projects as references
        /// </summary>
        /// <returns>
        /// </returns>
        [Route,AuthorizeActivity(Activity.ProjectGet) , QueryToODataFilter]
        public Task<IEnumerable<ProjectReferenceModel>> Get()
        {   
            return _projectCommonController.Get(Request.GetQuery());
        }

        /// <summary>
        /// Gets all projects with their detail.
        /// </summary>
        /// <returns></returns>
        [Route(RouteHelper.WithDetail),AuthorizeActivity(Activity.ProjectGet), QueryToODataFilter]
        public Task<IEnumerable<ProjectModel>> GetDetail()
		{
		    return _projectCommonController.GetDetail(Request.GetQuery());
		}


        /// <summary>
		///     Returns a project by his Id.
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.ProjectGet)]
		public Task<ProjectModel> Get(Guid id)
		{
            return _projectCommonController.Get(id);
		}

	    /// <summary>
	    ///     Updates an instance of the project item.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="project">The project.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.ProjectUpdate)]
		public Task<ProjectModel> Put(Guid id, ProjectDetailModel project)
		{
            return _projectCommonController.Put(id, project);
		}

	    /// <summary>
	    ///     Add a new project
	    /// </summary>
	    /// <param name="project">The project.</param>
	    /// <returns>
	    /// </returns>
        [Route,AuthorizeActivity(Activity.ProjectPost)]
		public Task<ProjectModel> Post(ProjectDetailModel project)
		{
            return _projectCommonController.Post(project);
		}

	    /// <summary>
	    ///     Deletes the specified project.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.ProjectDelete)]
		public Task<bool> Delete(Guid id)
		{
            return _projectCommonController.Delete(id);
		}

		
	}
}
using System.Collections.Generic;
using FizzWare.NBuilder;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class ProjectApiClientTests : CrudComponentTestsBase<ProjectModel, ProjectCreateUpdateModel, ProjectReferenceModel>
	{
		private ProjectApiClient _projectApiClient;

	    #region Setup/Teardown

	    protected override void Setup()
		{
            _projectApiClient = new ProjectApiClient(_adminRequestFactory.Value);
            SetRequiredData(_projectApiClient);
		}

	    [TearDown]
		public void TearDown()
		{

		}

	    #endregion
	}

    
}
using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class ProjectApiClientTests : CrudComponentTestsBase<ProjectModel, ProjectDetailModel, ProjectReferenceModel>
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

	    protected override IList<ProjectDetailModel> GetExampleData()
	    {
	        var projectDetailModel = Builder<ProjectDetailModel>.CreateListOfSize(2).All().WithValidData().Build();
	        return projectDetailModel;
	    }

	    #endregion
	}

    
}
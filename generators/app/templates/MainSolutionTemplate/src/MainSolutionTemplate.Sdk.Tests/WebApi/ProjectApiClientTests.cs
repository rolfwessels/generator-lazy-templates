using System;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class ProjectApiClientTests : ProjectClientBaseTests
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
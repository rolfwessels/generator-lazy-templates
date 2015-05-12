using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class ProjectClientBaseTests : CrudComponentTestsBase<ProjectModel,ProjectDetailModel>
    {

        protected override IList<ProjectDetailModel> GetExampleData()
        {
            var projectDetailModel = Builder<ProjectDetailModel>.CreateListOfSize(2).All().With(x => x.Name = GetRandom.FirstName()).Build();
            return projectDetailModel;
        }
    }
}
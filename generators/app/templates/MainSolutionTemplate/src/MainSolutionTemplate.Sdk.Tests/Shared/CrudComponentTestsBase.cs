using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models.Interfaces;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class CrudComponentTestsBase<TModel, TDetailModel, TReferenceModel> : IntegrationTestsBase where TModel : IBaseModel
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected ICrudController<TModel, TDetailModel> _crudController;

        protected void SetRequiredData(ICrudController<TModel, TDetailModel> userControllerActions)
        {
            _crudController = userControllerActions;
        }

        protected abstract void Setup();

        [Test]
        public void Get_WhenCalledWithOData_ShouldShouldFilter()
        {
            // arrange
            Setup();
            // action
            var baseStandardLookups = _crudController  as IBaseStandardLookups<TModel, TReferenceModel>;
            if (baseStandardLookups != null)
            {
                var restResponse = baseStandardLookups.Get("$top=1").Result;
                // assert
                restResponse.Count().Should().BeLessOrEqualTo(1);
            }
        }  

        [Test]
        public void PostPutDelete_WhenWhenGivenValidModel_ShouldLookupModels()
        {
            // arrange
            Setup();
            IList<TDetailModel> projectModel = GetExampleData();

            // action
            TModel projectModels = _crudController.Insert(projectModel[0]).Result;
            TModel savedProject = _crudController.Get(projectModels.Id).Result;
            TModel projectModelLoad = _crudController.Update(projectModels.Id, projectModel[1]).Result;
            bool removed = _crudController.Delete(projectModels.Id).Result;
            bool removedSecond = _crudController.Delete(projectModels.Id).Result;
            TModel removedProject = _crudController.Get(projectModels.Id).Result;

            // assert
            (savedProject).Should().NotBeNull();
            (removedProject).Should().BeNull();
            projectModel[0].ShouldBeEquivalentTo(projectModels, CompareConfig);
            projectModel[1].ShouldBeEquivalentTo(projectModelLoad, CompareConfig);
            (removed).Should().BeTrue();
            (savedProject).Should().NotBeNull();
            (removedSecond).Should().BeFalse();
        }

        protected virtual EquivalencyAssertionOptions<TDetailModel> CompareConfig(
            EquivalencyAssertionOptions<TDetailModel> options)
        {
            return options.ExcludingMissingMembers();
        }

        protected virtual IList<TDetailModel> GetExampleData()
        {
            return Builder<TDetailModel>.CreateListOfSize(2).All().Build();
        }

    }
}
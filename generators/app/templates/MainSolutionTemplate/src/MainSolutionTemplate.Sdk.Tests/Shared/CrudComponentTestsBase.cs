using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Enums;
using MainSolutionTemplate.Shared.Models.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class CrudComponentTestsBase<TModel, TDetailModel> : IntegrationTestsBase where TModel : IBaseModel
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected ICrudController<TModel, TDetailModel> _crudController;

        protected void SetRequiredData(ICrudController<TModel, TDetailModel> userControllerActions)
        {
            _crudController = userControllerActions;
        }

        protected abstract void Setup();

//        [Test]
//        public void Get_WhenCalledWithOData_ShouldShouldFilter()
//        {
//            // arrange
//            Setup();
//            // action
//            var restResponse = _crudController.Get("$top=1&$orderby=Name desc&$filter=not startswith(tolower(Name),'new')").Result;
//            // assert
//            restResponse.Count.Should().Be(1);
//        }  

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
            return options.ExcludingMissingProperties();
        }

        protected virtual IList<TDetailModel> GetExampleData()
        {
            return Builder<TDetailModel>.CreateListOfSize(2).All().Build();
        }



        protected void BaseSimpleSubscriptionTest()
        {
            // arrange
            Setup();
            TDetailModel projectDetailModel = GetExampleData().First();

            var valueUpdateModels = new List<ValueUpdateModel<TModel>>();
            var client = _crudController as IEventUpdateEventSubSubscription<TModel>;
            client.OnUpdate(valueUpdateModels.Add).Wait();
            // action
            TModel post = _crudController.Insert(projectDetailModel).Result;
            _crudController.Update(post.Id, projectDetailModel).Wait();
            client.UnsubscribeFromUpdates().Wait();
            _crudController.Delete(post.Id);

            IEnumerable<ValueUpdateModel<TModel>> updateModels = valueUpdateModels.Where(x => x.Value.Id == post.Id);
            // assert
            updateModels.WaitFor(x => x.Count() >= 2);
            updateModels.Should().HaveCount(2);
            updateModels.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Inserted);
            updateModels.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Updated);
        }
    }
}
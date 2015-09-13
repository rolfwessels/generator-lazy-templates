using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Models.Base;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Common
{
    public abstract class BaseCommonControllerTests<TDal, TModel, TReferenceModel, TDetailModel, TManager>
        where TDal : BaseDalModelWithId
        where TModel : BaseModel
        where TManager : class, IBaseManager<TDal>
    {
        protected Mock<TManager> _mockManager;
        protected BaseCommonController<TDal, TModel, TReferenceModel, TDetailModel> _commonController;
        


        public virtual void Setup()
        {
            MapApi.Initialize();
            _commonController = GetCommonController();
            _mockManager = GetManager();
        }

        protected virtual TDal SampleItem
        {
            get { return Builder<TDal>.CreateNew().Build(); }
        }

        protected abstract Mock<TManager> GetManager();

        protected abstract BaseCommonController<TDal, TModel, TReferenceModel, TDetailModel> GetCommonController();


        [TearDown]
        public void TearDown()
        {
            _mockManager.VerifyAll();
        }


        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _commonController.Should().NotBeNull();
        }

        [Test]
        public void Get_GivenRequest_ShouldReturnProjectReferenceModels()
        {
            // arrange
            Setup();
            var reference = Builder<TDal>.CreateListOfSize(2).Build();
            _mockManager.Setup(mc => mc.Get()).Returns(Task.FromResult(reference.ToList()));
            // action
            var result = _commonController.Get().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void GetDetail_GivenRequest_ShouldReturnProjectModel()
        {
            // arrange
            Setup();
            var reference = Builder<TDal>.CreateListOfSize(2).Build();
            _mockManager.Setup(mc => mc.Get())
                               .Returns(Task.FromResult(reference.ToList()));
            // action
            var result = _commonController.GetDetail().Result;
            // assert
            result.Count().Should().Be(2);
        }


        [Test]
        public void Get_GivenProjectId_ShouldCallGetProject()
        {
            // arrange
            Setup();
            var dal = SampleItem;
            _mockManager.Setup(mc => mc.Get(dal.Id))
                               .Returns(dal);
            // action
            var result = _commonController.Get(dal.Id).Result;
            // assert
            result.Id.Should().Be(dal.Id);
        }

        [Test]
        public void Put_GivenProjectId_ShouldUpdateAGivenProject()
        {
            // arrange
            Setup();
            var project = SampleItem;
            _mockManager.Setup(mc => mc.Get(project.Id))
                               .Returns(project);
            _mockManager.Setup(mc => mc.Save(project))
                               .Returns(project);
            var projectDetailModel = Builder<TDetailModel>.CreateNew().Build();
            // action
            var result = _commonController.Update(project.Id, projectDetailModel).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }

        [Test]
        public void Post_GivenProjectId_ShouldAddAProject()
        {
            // arrange
            Setup();
            var project = SampleItem;
            var projectDetailModel = Builder<TDetailModel>.CreateNew().Build();
            _mockManager.Setup(mc => mc.Save(It.IsAny<TDal>())).Returns(project);
            // action
            var result = _commonController.Insert(projectDetailModel).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }


        [Test]
        public void Delete_GivenProjectId_ShouldRemoveProject()
        {
            // arrange
            Setup();
            var project = SampleItem;
            _mockManager.Setup(mc => mc.Delete(project.Id)).Returns(project);
            // action
            var result = _commonController.Delete(project.Id).Result;
            // assert
            result.Should().Be(true);
        }

    }
}
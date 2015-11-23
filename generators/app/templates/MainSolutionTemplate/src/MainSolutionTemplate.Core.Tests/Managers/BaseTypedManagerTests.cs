using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Persistance;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    public abstract class BaseTypedManagerTests<T> : BaseManagerTests where T : BaseDalModelWithId
    {
        [Test]
        public virtual void Delete_WhenCalledWithExisting_ShouldCallMessageThatDataWasRemoved()
        {
            // arrange
            Setup();
            T project = Repository.AddFake().First();
            // action
            Manager.Delete(project.Id).Wait();
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<T>>(m => m.UpdateType == UpdateTypes.Removed)),
                                   Times.Once);
            Manager.Get(project.Id).Result.Should().BeNull();
        }

        [Test]
        public virtual void GetRecords_WhenCalled_ShouldReturnRecords()
        {
            // arrange
            Setup();
            const int expected = 2;
            Repository.AddFake(expected);
            // action
            var result = Manager.Get().Result;
            // assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public virtual void Get_WhenCalledWithId_ShouldReturnSingleRecord()
        {
            // arrange
            Setup();
            IList<T> addFake = Repository.AddFake();
            Guid guid = addFake.First().Id;
            // action
            T result = Manager.Get(guid).Result;
            // assert
            result.Id.Should().Be(guid);
        }

        [Test]
        public virtual void Save_WhenCalledWithExisting_ShouldCallMessageThatDataWasUpdated()
        {
            // arrange
            Setup();
            T project = Repository.AddFake().First();
            // action
            Manager.Save(project).Wait();
            // assert
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<T>>(m => m.UpdateType == UpdateTypes.Updated)),
                                   Times.Once);
            _mockIMessenger.Verify(
                mc => mc.Send(It.Is<DalUpdateMessage<T>>(m => m.UpdateType == UpdateTypes.Inserted)), Times.Never);
        }

        [Test]
        public virtual void Save_WhenCalledWith_ShouldCallMessageThatDataWasInserted()
        {
            // arrange
            Setup();
            T project = SampleObject;
            // action
            Manager.Save(project).Wait();
            // assert
            _mockIMessenger.Verify(
                mc => mc.Send(It.Is<DalUpdateMessage<T>>(m => m.UpdateType == UpdateTypes.Inserted)), Times.Once);
            _mockIMessenger.Verify(mc => mc.Send(It.Is<DalUpdateMessage<T>>(m => m.UpdateType == UpdateTypes.Updated)),
                                   Times.Never);
        }

        [Test]
        public virtual void Save_WhenCalledWith_ShouldSaveTheRecord()
        {
            // arrange
            Setup();
            T project = SampleObject;
            // action
            T result = Manager.Save(project).Result;
            // assert
            Repository.Count().Result.Should().Be(1L);
            result.Should().NotBeNull();
        }

        [Test]
        public virtual void Save_WhenCalledWith_ShouldToLowerTheEmail()
        {
            // arrange
            Setup();
            T project = SampleObject;
            // action
            T result = Manager.Save(project).Result;
            // assert
            result.Id.Should().Be(project.Id);
        }

        protected abstract IRepository<T> Repository { get; }

        protected virtual T SampleObject
        {
            get { return Builder<T>.CreateNew().Build(); }
        }

        protected abstract BaseManager<T> Manager { get; }
    }
}
using System;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.Tests.Fakes;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using MainSolutionTemplate.Utilities.Helpers;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class BaseManagerTests
    {
        protected BaseManagerArguments _baseManagerArguments;
        protected FakeGeneralUnitOfWork _fakeGeneralUnitOfWork;
        protected Mock<IMessenger> _mockIMessenger;
        protected Mock<IValidatorFactory> _mockIValidatorFactory;

        #region Setup/Teardown

        public virtual void Setup()
        {
            _mockIMessenger = new Mock<IMessenger>();
            _mockIValidatorFactory = new Mock<IValidatorFactory>();
            _fakeGeneralUnitOfWork = new FakeGeneralUnitOfWork();
            _baseManagerArguments = new BaseManagerArguments(_fakeGeneralUnitOfWork, _mockIMessenger.Object,
                                                             _mockIValidatorFactory.Object);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockIValidatorFactory.VerifyAll();
            _mockIMessenger.VerifyAll();
        }

        #endregion
    }

}
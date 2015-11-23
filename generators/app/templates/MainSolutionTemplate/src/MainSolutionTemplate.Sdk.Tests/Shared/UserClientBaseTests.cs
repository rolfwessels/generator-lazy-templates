using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class UserClientBaseTests :  CrudComponentTestsBase<UserModel,UserDetailModel,UserReferenceModel>
    {
        [Test]
        public void Post_WhenCalledWithInvalidDuplicateEmail_ShouldThrowException()
        {
            // arrange
            Setup();
            var userDetailModel = Builder<UserDetailModel>.CreateNew().Build();
            // action
            try
            {
                _crudController.Insert(userDetailModel).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Action testCall = () => { _crudController.Insert(userDetailModel).Wait(); };
            // assert
            testCall.ShouldThrow<Exception>().WithMessage("'Email' is not a valid email address.");
        }

        #region Overrides of CrudComponentTestsBase<UserModel,UserDetailModel>

        protected override IList<UserDetailModel> GetExampleData()
        {
            return Builder<UserDetailModel>.CreateListOfSize(2).All().With(x=>x.Email = GetRandom.Email().ToLower()).Build();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class UserClientBaseTests :  CrudComponentTestsBase<UserModel,UserDetailModel>
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
                _crudController.Post(userDetailModel).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Action testCall = () => { _crudController.Post(userDetailModel).Wait(); };
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
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MainSolutionTemplate.Web.ApiControllers
{
    public class DataEntryController : ApiController
    {

        public List<String> Get()
        {
            return new List<string>() { "test" , "test2"};
        }
       
    }
}
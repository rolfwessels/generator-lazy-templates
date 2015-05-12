using System;
using MainSolutionTemplate.Shared.Models.Interfaces;

namespace MainSolutionTemplate.Shared.Models.Base
{
    public class BaseModel : IBaseModel
    {
        public Guid Id { get; set; }
    }
}
using System;
using MainSolutionTemplate.Shared.Models.Interfaces;

namespace MainSolutionTemplate.Shared.Models.Base
{
    public abstract class BaseModel : IBaseModel
    {
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
using System;

namespace MainSolutionTemplate.Dal.Models
{
  public class BaseDalModelWithId : BaseDalModel
  {
    public Guid Id { get; set; }
  }
}
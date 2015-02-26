using MainSolutionTemplate.Dal.Models.Base;

namespace MainSolutionTemplate.Dal.Models.Reference
{
    public class UserReference : BaseReferenceWithName
    {
        public string Email { get; set; }
    }
}
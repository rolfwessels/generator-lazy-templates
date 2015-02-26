using System;

namespace MainSolutionTemplate.Shared.Models
{
    public class UserModel : UserDetailModel
    {
        public Guid Id { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Email: {1}, Name: {2}", Id, Email, Name);
        }
    }
}
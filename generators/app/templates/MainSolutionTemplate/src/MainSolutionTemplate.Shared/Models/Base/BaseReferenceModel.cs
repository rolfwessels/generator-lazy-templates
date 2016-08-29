using System;

namespace MainSolutionTemplate.Shared.Models.Base
{
    public class BaseReferenceModel
    {
        public string Id { get; set; }

        #region Equality members

        protected bool Equals(BaseReferenceModel other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseReferenceModel) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
namespace MainSolutionTemplate.Dal.Models.Base
{
    public abstract class BaseReferenceWithName : BaseReference
    {
        public string Name { get; set; }

        #region Equality members

        protected bool Equals(BaseReferenceWithName other)
        {
            return base.Equals(other) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BaseReferenceWithName) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion
    }
}
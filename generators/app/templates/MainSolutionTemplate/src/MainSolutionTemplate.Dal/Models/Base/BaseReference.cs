﻿using System;

namespace MainSolutionTemplate.Dal.Models.Base
{
    public abstract class BaseReference : IBaseReference
    {
        public Guid Id { get; set; }

        #region Equality members

        protected bool Equals(BaseReference other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseReference) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
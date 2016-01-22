using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Core.DataIntegrity
{
    public class PropertyIntegrity<TDal,TDalReference,TContainer> : IIntegrity where TDal : class where TContainer : IBaseDalModel where TDalReference : class
    {
        private Expression<Func<TContainer, TDalReference>> _property;
        private readonly Func<IGeneralUnitOfWork, IRepository<TContainer>> _getRepo;
        private readonly Func<TDalReference, Expression<Func<TContainer, bool>>> _filter;
        private readonly Func<TDal, TDalReference> _toReference;

        public PropertyIntegrity(Expression<Func<TContainer, TDalReference>> property, Func<IGeneralUnitOfWork, IRepository<TContainer>> getRepo, Func<TDalReference, Expression<Func<TContainer, bool>>> filter, Func<TDal, TDalReference> toReference)
        {
            _property = property;
            _getRepo = getRepo;
            _filter = filter;
            _toReference = toReference;
        }

        #region Implementation of IIntegrity

        public bool UpdateAllowed<T>(T updatedValue)
        {
            return updatedValue is TDal;
        }

        public Task<long> UpdateReferences<T>(IGeneralUnitOfWork generalUnitOfWork, T updatedValue)
        {
            var reference = _toReference(updatedValue as TDal);
            var repository = _getRepo(generalUnitOfWork);
            return repository.Update(_filter(reference), _property, reference);
        }

        public Task<long> GetReferenceCount<T>(IGeneralUnitOfWork generalUnitOfWork, T updatedValue)
        {
            var reference = _toReference(updatedValue as TDal);
            var repository = _getRepo(generalUnitOfWork);
            return repository.Count(_filter(reference));
        }

        public async Task<bool> HasReferenceChanged<T>(IGeneralUnitOfWork generalUnitOfWork, T updatedValue)
        {
            var reference = _toReference(updatedValue as TDal);
            var repository = _getRepo(generalUnitOfWork);
            var hasReferenceChanged = await repository.FindOne(_filter(reference));
            if (hasReferenceChanged != null)
            {
                reference.Dump("reference");
                var dalReference = _property.Compile()(hasReferenceChanged);
                dalReference.Dump("dalReference");
                return !dalReference.Equals(reference);
            }
            return false;
        }

        public bool IsIntegration(Type dalType, string property)
        {
            
            var sameType = dalType.Name == typeof(TContainer).Name;
            if (sameType)
            {
                var propertyInfo = ReflectionHelper.GetPropertyString(_property);
                return property == propertyInfo;
            }
            return sameType;
        }

        #endregion
    }
}
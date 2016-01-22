using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Core.DataIntegrity
{
    
    public class DataIntegrityManager : IDataIntegrityManager
    {
        private readonly IGeneralUnitOfWork _generalUnitOfWork;
        private readonly List<IIntegrity> _integrityUpdatetor;

        public DataIntegrityManager(IGeneralUnitOfWork generalUnitOfWork, List<IIntegrity> integrityUpdatetors)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _integrityUpdatetor = integrityUpdatetors;
        }

        public async Task<long> UpdateAllReferences<T>(T updatedValue)
        {
            var referenceTotal = 0L;
            var allowedUpdates = _integrityUpdatetor.Where(x=> x.UpdateAllowed(updatedValue)).ToArray();
            if (allowedUpdates.Any() )
            {
                foreach (var integrity in allowedUpdates)
                {
                    referenceTotal += await integrity.UpdateReferences(_generalUnitOfWork, updatedValue);
                    referenceTotal.Dump("referenceTotal");
                }
            }
            return referenceTotal;
        }

        public async Task<long> GetReferenceCount<T>(T updatedValue)
        {
            var referenceTotal = 0L;
            foreach (var integrity in _integrityUpdatetor.Where(x => x.UpdateAllowed(updatedValue)))
            {
                if (integrity.UpdateAllowed(updatedValue))
                {
                    referenceTotal += await integrity.GetReferenceCount(_generalUnitOfWork, updatedValue);
                }
            }
            return referenceTotal;

        }

        public long FindMissingIntegrityOperators<TDal,TReference>(Assembly assembly)
        {
            
            var allDalTypes = assembly.GetTypes().Where(x=> !x.IsInterface && x.IsPublic && !x.IsAbstract && typeof(TDal).IsAssignableFrom(x)).ToArray();
            var allReferences = assembly.GetTypes().Where(x => !x.IsInterface && x.IsPublic && !x.IsAbstract && typeof(TReference).IsAssignableFrom(x)).ToArray();
            var missing = 0;
            foreach (var dalType in allDalTypes)
            {
                var strings = ScanType(assembly, dalType, allReferences, dalType).ToArray();
                foreach (var s in strings)
                {
                    Console.Out.WriteLine(s);
                }
                missing += strings.Length;
            }

            return missing;
        }

        private IEnumerable<string> ScanType(Assembly assembly, Type dalType, Type[] allReferences, Type className, string prefix = "")
        {
            var propertyInfos = dalType.GetProperties();
            foreach (var property in propertyInfos)
            {
                var memberString = (prefix + property.Name);
                if (allReferences.Contains(property.PropertyType) &&
                    !_integrityUpdatetor.Any(x => x.IsIntegration(className, memberString)))
                {
                    
                    yield return String.Format("Missing {0} on {1} " +
                                               "[ new PropertyIntegrity<{4}, {3}, {2}>" +
                                               "(u => u.{1}, g => g.{2}s,r => x => x.{1}.Id == r.Id, x=>x.ToReference()) ]"
                                               , property.Name, memberString, className, property.PropertyType.Name, property.PropertyType.Name.Replace("Reference", ""));
                }
                else if (property.PropertyType.IsClass && property.PropertyType.Assembly == assembly)
                {
                    foreach (var resultString in ScanType(assembly, property.PropertyType, allReferences, className, property.Name + "."))
                    {
                        yield return resultString;
                    }
                }
            }
        }
    }
}
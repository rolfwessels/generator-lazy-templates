using System.Collections.Generic;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;

namespace MainSolutionTemplate.Core.DataIntegrity
{
    public class IntegrityOperators
    {
        private static readonly List<IIntegrity> _allIntegrity;

        static IntegrityOperators()
        {
            _allIntegrity = new List<IIntegrity>
            {
                new PropertyIntegrity<Project, ProjectReference, User>(u => u.DefaultProject, g => g.Users,r => x => x.DefaultProject.Id == r.Id, x=>x.ToReference()),
                //Sample with array of items
                //new PropertyIntegrity<Project, ProjectReference, User>(u => u.AllowedProject[-1], g => g.Users,r => x => x.AllowedProject.Any(a=>a.Id == r.Id) , x=>x.ToReference()),
            };
        }

        public static List<IIntegrity> Default
        {
            get
            {
                return _allIntegrity;
            }
        }
    }
}
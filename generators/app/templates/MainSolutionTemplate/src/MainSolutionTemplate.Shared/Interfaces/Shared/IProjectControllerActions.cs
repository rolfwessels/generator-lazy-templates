using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Base;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Shared.Interfaces.Shared
{
    public interface IProjectControllerActions : ICrudController<ProjectModel, ProjectCreateUpdateModel>
	{
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
	public class TaskController : ApiController
	{
		private readonly List<TaskModel> _taskModels;

		public TaskController()
		{
			_taskModels = new List<TaskModel>();
			Post(new TaskModel {Name = "Add type scripts"});
		}

		/// <summary>
		/// Gets this instance.
		/// </summary>
		/// <returns></returns>
		[Route(RouteHelper.TaskController)]
		public List<TaskModel> Get()
		{
			return _taskModels;
		}

		[Route(RouteHelper.TaskController)]
		public TaskModel Put(int id, TaskModel model)
		{
			var taskModels = _taskModels.Where(x => x.Id == id).ToArray();
			taskModels.Each(x => x.Name = model.Name);
			return taskModels.FirstOrDefault();
		}

		[Route(RouteHelper.TaskController)]
		public TaskModel Post(TaskModel model)
		{
			model.Id = _taskModels.Count;
			_taskModels.Add(model);
			return model;
		}

		[Route(RouteHelper.TaskController)]
		public int Delete(int id)
		{
			TaskModel[] taskModels = _taskModels.Where(x => x.Id == id).ToArray();
			taskModels.Each(x => _taskModels.Remove(x));
			return taskModels.Count();
		}
	}

	public class TaskModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
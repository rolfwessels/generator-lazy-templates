using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Api.Models
{
	public class ValueUpdateModel<T>
	{
		private readonly T _value;
		private readonly Types _updateType;

		public ValueUpdateModel(T value, Types updateType)
		{
			_value = value;
			_updateType = updateType;
		}

		public T Value
		{
			get { return _value; }
		}

		public Types UpdateType
		{
			get { return _updateType; }
		}


	}
}
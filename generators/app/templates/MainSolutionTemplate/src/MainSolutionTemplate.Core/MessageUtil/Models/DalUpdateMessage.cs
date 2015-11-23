using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.MessageUtil.Models
{
	public class DalUpdateMessage<T>
	{
		private readonly T _value;
		private readonly UpdateTypes _updateType;

		public DalUpdateMessage(T value,UpdateTypes updateType)
		{
			_value = value;
			_updateType = updateType;
		}

		public T Value
		{
			get { return _value; }
		}

		public UpdateTypes UpdateType
		{
			get { return _updateType; }
		}

	    public override string ToString()
	    {
	        return string.Format("UpdateType: {0}, Value: {1}", UpdateType, Value);
	    }
	}
}
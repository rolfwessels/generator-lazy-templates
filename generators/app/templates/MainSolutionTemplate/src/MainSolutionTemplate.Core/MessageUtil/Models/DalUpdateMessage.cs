﻿using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Core.MessageUtil.Models
{
	public class DalUpdateMessage<T>
	{
		private readonly T _value;
		private readonly Types _updateType;

		public DalUpdateMessage(T value,Types updateType)
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
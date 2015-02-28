using MainSolutionTemplate.Shared.Models.Enums;

namespace MainSolutionTemplate.Shared.Models
{
    public class ValueUpdateModel<T>
    {
        private readonly UpdateTypeCodes _updateType;
        private readonly T _value;

        public ValueUpdateModel(T value, UpdateTypeCodes updateType)
        {
            _value = value;
            _updateType = updateType;
        }

        public T Value
        {
            get { return _value; }
        }

        public UpdateTypeCodes UpdateType
        {
            get { return _updateType; }
        }
    }
}
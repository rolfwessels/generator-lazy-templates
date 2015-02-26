using MainSolutionTemplate.Shared.Models.Enums;

namespace MainSolutionTemplate.Shared.Models
{
    public class ValueUpdateModel<T>
    {
        private readonly UpdateTypeCodes _updateUpdateTypeCode;
        private readonly T _value;

        public ValueUpdateModel(T value, UpdateTypeCodes updateUpdateTypeCode)
        {
            _value = value;
            _updateUpdateTypeCode = updateUpdateTypeCode;
        }

        public T Value
        {
            get { return _value; }
        }

        public UpdateTypeCodes UpdateUpdateTypeCode
        {
            get { return _updateUpdateTypeCode; }
        }
    }
}
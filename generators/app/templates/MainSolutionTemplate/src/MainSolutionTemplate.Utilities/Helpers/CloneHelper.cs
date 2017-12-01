using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public static class CloneHelper
    {
        public static T JsonClone<T>(this object model)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            var serializeObject = JsonConvert.SerializeObject(model, settings);
            return JsonConvert.DeserializeObject<T>(serializeObject);
        }
    }
}
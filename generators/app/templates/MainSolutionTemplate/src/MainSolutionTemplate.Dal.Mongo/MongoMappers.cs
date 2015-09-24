using MainSolutionTemplate.Dal.Models;
using MongoDB.Bson.Serialization;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoMappers
    {
        public void InitializeMappers()
        {
            
            BsonClassMap.RegisterClassMap<BaseDalModel>(cm =>
            {
                cm.MapProperty(c => c.CreateDate).SetElementName("Cd");
                cm.MapProperty(c => c.UpdateDate).SetElementName("Ud");
            });
        }
    }
}
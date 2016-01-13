using System;
using System.Reflection;
using log4net;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class MongoMappers
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void InitializeMappers()
        {
            SetupDataTimeSerializer();
            SetupMapping();
        }

        private static void SetupMapping()
        {
            BsonClassMap.RegisterClassMap<BaseDalModel>(cm =>
            {
                cm.MapProperty(c => c.CreateDate).SetElementName("Cd");
                cm.MapProperty(c => c.UpdateDate).SetElementName("Ud");
            });
        }

        private static void SetupDataTimeSerializer()
        {
            try
            {
                var serializer = new DateTimeSerializer(DateTimeKind.Local);
                BsonSerializer.RegisterSerializer(typeof(DateTime), serializer);
            }
            catch (Exception e)
            {
                _log.Error("MongoMappers:InitializeMappers " + e.Message);
            }
        }
    }
}
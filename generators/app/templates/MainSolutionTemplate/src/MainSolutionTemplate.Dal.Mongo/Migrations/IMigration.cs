using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
    public interface IMigration
    {
        void Update(IMongoDatabase db);
    }
}
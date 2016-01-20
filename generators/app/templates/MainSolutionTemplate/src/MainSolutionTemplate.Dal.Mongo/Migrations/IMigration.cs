using System.Threading.Tasks;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
    public interface IMigration
    {
        Task Update(IMongoDatabase db);
    }
}
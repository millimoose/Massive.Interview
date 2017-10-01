using Microsoft.EntityFrameworkCore.Design;

namespace Massive.Interview.Entities
{
    public class DesignTimeGraphEntitiesFactory : IDesignTimeDbContextFactory<GraphEntities>
    {
        public GraphEntities CreateDbContext(string[] args)
        {
            return new GraphEntities("Server=(localdb)\\mssqllocaldb;Database=Massive.Interview;Trusted_Connection=True;");
        }
    }
}
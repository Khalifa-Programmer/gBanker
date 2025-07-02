using gBanker.Data.CodeFirstMigration.Db;


namespace gBanker.Data.CodeFirstMigration.InfrastructureBase
{
    public class UnitOfWorkCodeFirst : IUnitOfWorkCodeFirst
    {
        private readonly IDatabaseFactoryCodeFirst databaseFactory;
        private gBankerDbContext dataContext;

        public UnitOfWorkCodeFirst(IDatabaseFactoryCodeFirst databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected gBankerDbContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}

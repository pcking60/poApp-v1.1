namespace PostOfiice.DAta.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private PostOfficeDbContext dbContext;

        public UnitOfWork(IDbFactory DbFactory)
        {
            this.dbFactory = DbFactory;
        }

        public PostOfficeDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
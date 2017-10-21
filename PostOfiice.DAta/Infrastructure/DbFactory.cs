namespace PostOfiice.DAta.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private PostOfficeDbContext dbContext;

        public PostOfficeDbContext Init()
        {
            return dbContext ?? (dbContext = new PostOfficeDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
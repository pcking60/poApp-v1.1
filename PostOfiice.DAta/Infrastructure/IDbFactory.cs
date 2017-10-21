using System;

namespace PostOfiice.DAta.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        PostOfficeDbContext Init();
    }
}
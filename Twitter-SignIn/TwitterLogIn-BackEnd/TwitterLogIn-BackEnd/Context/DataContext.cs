using Microsoft.EntityFrameworkCore;

namespace TwitterLogIn_BackEnd.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
    }
}

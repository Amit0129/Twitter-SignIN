using Microsoft.EntityFrameworkCore;
using TwitterLogIn_BackEnd.Model;

namespace TwitterLogIn_BackEnd.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) 
        {

        }
        public DbSet<UserInfoModel> UserAllInfo { get; set; }
    }
}

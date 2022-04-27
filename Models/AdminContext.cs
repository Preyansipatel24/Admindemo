using Microsoft.EntityFrameworkCore;

namespace AdminDemo.Models
{
    public class AdminContext: DbContext
    {
       

        public AdminContext()
        {

        }
        public AdminContext(DbContextOptions<AdminContext> option): base(option)
        {

        }

        public DbSet<UserMaster> Tbl_User_Master { get; set; }
        public DbSet<RollMaster> Tbl_Roll_Master { get; set; }
        public DbSet<DepartmentMaster> Tbl_Department_Master { get; set; }
    }
}

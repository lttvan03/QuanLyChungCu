using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QuanLyChungCu
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> TaiKhoan { get; set; }

        // Cấu hình kết nối cơ sở dữ liệu (có thể sử dụng connection string từ appsettings.json)
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer("Data Source=LTTVAN; Initial Catalog=QLChungCu;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
        }
    }
}

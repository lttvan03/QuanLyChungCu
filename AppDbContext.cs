using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyChungCu.Pages;

namespace QuanLyChungCu
{

    public class AppDbContext : DbContext
    {
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<CuDan> CuDan { get; set; }

        // Cấu hình kết nối cơ sở dữ liệu (có thể sử dụng connection string từ appsettings.json)
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer("Data Source=LTTVAN; Initial Catalog=QLChungCu;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TaiKhoan>()
                        .HasOne(t => t.CuDan)  // Một TaiKhoan có một CuDan
                        .WithOne(c => c.TaiKhoan)  // Một CuDan có một TaiKhoan
                        .HasForeignKey<CuDan>(c => c.IDCuDan);  // Sử dụng IDTaiKhoan làm khóa ngoại
        }

    }
}

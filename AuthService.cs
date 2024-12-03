using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QuanLyChungCu
{
    public class AuthService
    {
        private readonly AppDbContext dbContext;

        // Constructor nhận đối tượng dbContext từ bên ngoài
        public AuthService(AppDbContext dbContext) {
            this.dbContext = dbContext; // Gán dbContext đã được truyền vào
        }

        public User Login(string username, string password) {
            // Tìm kiếm user trong cơ sở dữ liệu
            var user = dbContext.TaiKhoan
                .FirstOrDefault(u => u.IDTaiKhoan.ToLower() == username.Trim().ToLower());

            // Kiểm tra mật khẩu
            if (user != null && user.MatKhau == password.Trim()) {
                return user; // Trả về user nếu tìm thấy và mật khẩu đúng
            }
            return null; // Trả về null nếu không tìm thấy hoặc mật khẩu sai
        }
    }
}
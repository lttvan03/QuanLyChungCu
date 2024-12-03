using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuanLyChungCu
{
    public class User
    {
        [Key]
        public string IDTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string QuyenHan { get; set; } // Quyền hạn của người dùng
    }
}

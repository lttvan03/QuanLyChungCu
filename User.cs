using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QuanLyChungCu.Pages;

namespace QuanLyChungCu
{
    public class TaiKhoan
    {
        [Key]
        public string IDTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string QuyenHan { get; set; } // Quyền hạn của người dùng

        // Liên kết trực tiếp tới CuDan qua IDTaiKhoan
        public CuDan CuDan { get; set; }  // Một TaiKhoan có một CuDan
    }

    public class CuDan
    {
        [Key]
        public string IDCuDan { get; set; }  // IDCuDan là chính là IDTaiKhoan
        public string TenCuDan { get; set; }
        public string SoCanHo { get; set; }

        // Liên kết ngược lại tới TaiKhoan
        public TaiKhoan TaiKhoan { get; set; }  // Một CuDan có một TaiKhoan
    }


}

using QuanLyChungCu.ConnectDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChungCu.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        private DataTable dGrid = new DataTable();
        public Dashboard() {
            InitializeComponent();
            int soLuongCanHo = countCanHo();
            CountCanHo.Text = soLuongCanHo.ToString();
            int soLuongCuDan = countCuDan();
            CountCuDan.Text = soLuongCuDan.ToString();
            int soLuongPT = countPhuongTien();
            CountPT.Text = soLuongPT.ToString();

        }
        public int countCanHo() {
            int count = 0;
            string sSQL = "SELECT COUNT(*) FROM CanHo";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }
        public int countCuDan() {
            int count = 0;
            string sSQL = "SELECT COUNT(*) FROM CuDan";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }
        public int countPhuongTien() {
            int count = 0;
            string sSQL = "SELECT (SELECT COUNT(*) FROM XeMay) + (SELECT COUNT(*) FROM XeDap) + (SELECT COUNT(*) FROM XeOto) AS TongXe";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }


    }
}

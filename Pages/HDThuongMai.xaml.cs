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
    /// Interaction logic for HDThuongMai.xaml
    /// </summary>
    public partial class HDThuongMai : Page
    {
        private DataTable dGrid = new DataTable();

        public HDThuongMai() {
            InitializeComponent();
            LoadDataGrid();

        }
        private void LoadDataGrid() {
            dGrid = Connect.DataTransport("SELECT * FROM HoaDonTM INNER JOIN MatBangThuongMai ON MatBangThuongMai.IDMBTM = HoaDonTM.IDMBTM");
            dtview.ItemsSource = dGrid.DefaultView;
        }

    }
}

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
        private string currentUserQH;
        private string currentUserID;

        private DataTable dGrid = new DataTable();
        public Dashboard() {
            InitializeComponent();
            Load();
        }
        private void Load() {
            currentUserID = GetCurrentUserID();
            currentUserQH = GetCurrentUserQH();
            int soLuongCanHo = countCanHo();
            CountCanHo.Text = soLuongCanHo.ToString();
            int soLuongCuDan = countCuDan();
            CountCuDan.Text = soLuongCuDan.ToString();
            int soOto = countXeOTo();
            Counoto.Text = soOto.ToString();
            int soXM = countXeMay();
            CountXM.Text = soXM.ToString();
            int soXD = countXeDap();
            CountXD.Text = soXD.ToString();

            if(currentUserQH == "Cư dân") {
                txtXemCanHo.IsEnabled = false;
                btnXemCuDan.IsEnabled = false;
                btnXemoto.IsEnabled = false;
                btnXemXM.IsEnabled = false;
                btnXemXeDap.IsEnabled = false;
            }
        }

        private string GetCurrentUserID() {
            return App.Current.Properties["ID"]?.ToString();
        }
        private string GetCurrentUserQH() {
            return App.Current.Properties["UserRole"]?.ToString();
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
        public int countXeOTo() {
            int count = 0;
            string sSQL = "SELECT COUNT(*) FROM XeOto";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }
        public int countXeMay() {
            int count = 0;
            string sSQL = "SELECT COUNT(*) FROM XeMay";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }
        public int countXeDap() {
            int count = 0;
            string sSQL = "SELECT COUNT(*) FROM XeDap";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                count = Convert.ToInt32(dt.Rows[0][0]);
                return count;
            }
            return count;
        }

        private void txtXemCanHo_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow) {
                mainWindow.MainWindowFrame.Navigate(new QuanLyChungCu.Pages.QLCanHo());
            }
            else {
                MessageBox.Show("MainWindow không tồn tại hoặc không đúng kiểu.");
            }
        }
        private void btnXemCuDan_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow) {
                mainWindow.MainWindowFrame.Navigate(new QuanLyChungCu.Pages.QLCuDan());
            }
            else {
                MessageBox.Show("MainWindow không tồn tại hoặc không đúng kiểu.");
            }
        }

        private void btnXemoto_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow) {
                mainWindow.MainWindowFrame.Navigate(new QuanLyChungCu.Pages.QLoto());
            }
            else {
                MessageBox.Show("MainWindow không tồn tại hoặc không đúng kiểu.");
            }

        }

        private void btnXemXM_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow) {
                mainWindow.MainWindowFrame.Navigate(new QuanLyChungCu.Pages.QLXeMay());
            }
            else {
                MessageBox.Show("MainWindow không tồn tại hoặc không đúng kiểu.");
            }
        }

        private void btnXemXeDap_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow is MainWindow mainWindow) {
                mainWindow.MainWindowFrame.Navigate(new QuanLyChungCu.Pages.QLXeDap());
            }
            else {
                MessageBox.Show("MainWindow không tồn tại hoặc không đúng kiểu.");
            }
        }
    }
}

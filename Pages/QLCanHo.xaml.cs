using System;
using System.Collections.Generic;
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
using QuanLyChungCu.ConnectDatabase;
using Microsoft.Data.SqlClient;
using System.Data;
using static QuanLyChungCu.Pages.KVDanCu;
using static QuanLyChungCu.MainWindow;
using System.Windows.Threading;


namespace QuanLyChungCu.Pages
{
    /// <summary>
    /// Interaction logic for QLCanHo.xaml
    /// </summary>
    public partial class QLCanHo : Page
    {
        //Khi form vừa mở lên thì sẽ ở trạng thái Xem
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        private DataTable dGrid;

        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }

        public QLCanHo() {
            InitializeComponent();
        }

        private void LoadStatus() {
            switch (_trangThaiHienTai) {
                case TrangThaiHienTai.Xem:
                    popup.IsOpen = false;
                    overlayGrid.Visibility = Visibility.Collapsed;
                    overlayGrid.Opacity = 0;
                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    break;
                case TrangThaiHienTai.Them:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            this.Cursor = Cursors.Wait;
            LoadDataGrid();
            this.Cursor = Cursors.Arrow;
        }

        private void LoadDataGrid(string sKey = "") {
            dGrid = Connect.DataTransport("select * from HoaTuoi");

            // Xóa hoàn toàn ItemsSource trước khi gán lại
            dtview.ItemsSource = null; // Hủy gán ItemsSource cũ
            dtview.Items.Clear(); // Xóa hết các mục hiện tại

            // Gán lại ItemsSource với dữ liệu mới
            dtview.ItemsSource = dGrid.DefaultView;
            if (!string.IsNullOrEmpty(sKey)) {
                if (!dtview.IsFocused)
                    dtview.Focus();
                for (int i = 0; i < dtview.Items.Count; i++) {
                    DataRowView dr = (DataRowView)dtview.Items[i];
                    if (dr["MaHoaTuoi"].ToString() == sKey) {
                        //Focus vào dòng vừa thêm, hoặc dòng vừa sửa
                        FocusRow(i);
                    }
                }
            }
        }
        private void FocusRow(int i) {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                dtview.SelectedIndex = i;
                dtview.ScrollIntoView(dtview.Items[i]);
            }), DispatcherPriority.Render);
        }

        private void dtview_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void dtview_Loaded(object sender, RoutedEventArgs e) {
            DataSet data = new DataSet();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Sua;
            LoadStatus();
        }

        
        private void btnThem_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Them;
            LoadStatus();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }
    }
}

using Microsoft.Data.SqlClient;
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
using System.Windows.Threading;
using static QuanLyChungCu.Pages.QLoto;
using static QuanLyChungCu.MainWindow;
using System.Diagnostics.Eventing.Reader;

namespace QuanLyChungCu.Pages
{
    /// <summary>
    /// Interaction logic for QLoto.xaml
    /// </summary>
    public partial class KVThuongMai : Page
    {
        private DataTable dGrid = new DataTable();
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }
        public KVThuongMai()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxQuanLy();
        }
        private void LoadDataGrid()
        {
            dGrid = Connect.DataTransport("SELECT * FROM MatBangThuongMai INNER JOIN NguoiQuanLy ON MatBangThuongMai.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");
            foreach (DataRow row in dGrid.Rows)
            {
                int DienTich = Convert.ToInt32(row["DienTich"]);
                int GiaThue = Convert.ToInt32(row["GiaThue"]);
            }
            dtview.ItemsSource = dGrid.DefaultView;
        }
        private void LoadStatus()
        {
            DataRowView row = (DataRowView)dtview.SelectedItem;
            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Xem:
                    popup.IsOpen = false;
                    overlayGrid.Visibility = Visibility.Collapsed;

                    break;
                case TrangThaiHienTai.Them:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtIDMBTM.Text = "";
                    txtDienTich.Text = "";
                    txtTenDonViThue.Text = "";
                    txtGiaThue.Text = "";
                    txtTinhTrang.Text = "";
                    comboboxQuanLy.SelectedValue = "";
                    txtQuanLy.Text = "";

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtIDMBTM.Text = row["IDMBTM"].ToString();
                    txtDienTich.Text = row["DienTich"].ToString();
                    txtTenDonViThue.Text = row["TenDonViThue"].ToString();
                    txtGiaThue.Text = row["GiaThue"].ToString();
                    txtTinhTrang.Text = row["TinhTrang"].ToString();
                    comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

                    break;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTimKiem.Text))
            {
                string searchText = txtTimKiem.Text.ToLower().Trim();

               string sSQL = $"SELECT * FROM MatBangThuongMai " +
                    $"INNER JOIN NguoiQuanLy ON MatBangThuongMai.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                    $"WHERE LOWER(TenDonViThue) LIKE N'%{searchText}%' OR LOWER(TinhTrang) LIKE N'%{searchText}%' " +
                    $"OR LOWER(GiaThue) LIKE '%{searchText}%' OR LOWER(DienTich) LIKE '%{searchText}%' " +
                    $"OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE N'%{searchText}%'";

                DataTable dTimKiem = Connect.DataTransport(sSQL);
                dtview.ItemsSource = dTimKiem.DefaultView;
            }
            else
            {
                LoadDataGrid();
            }
        }
        private void dtview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtview.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dtview.SelectedItem;

                txtIDMBTM.Text = row["IDMBTM"].ToString();
                txtDienTich.Text = row["DienTich"].ToString();
                txtTenDonViThue.Text = row["TenDonViThue"].ToString();
                txtGiaThue.Text = row["GiaThue"].ToString();
                txtTinhTrang.Text = row["TinhTrang"].ToString();
                comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();
            }
        }
        private void comboboxQuanLy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxQuanLy.SelectedItem != null)
            {
                string selectedID = ((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();

                DataTable dt_quanly = Connect.DataTransport($"SELECT TenNguoiQuanLy FROM NguoiQuanLy WHERE IDNguoiQuanLy = '{selectedID}'");

                if (dt_quanly.Rows.Count > 0)
                {
                    txtQuanLy.Text = dt_quanly.Rows[0]["TenNguoiQuanLy"].ToString();
                }
                else
                {
                    txtQuanLy.Clear();
                }
            }
        }
        private void LoadComboBoxQuanLy()
        {
            // Lấy danh sách ID từ bảng CuDan
            DataTable dt_quanly = Connect.DataTransport("SELECT DISTINCT IDNguoiQuanLy FROM NguoiQuanLy");

            if (dt_quanly.Rows.Count > 0)
            {
                // Gán dữ liệu vào ComboBox
                comboboxQuanLy.ItemsSource = dt_quanly.DefaultView;
                comboboxQuanLy.DisplayMemberPath = "IDNguoiQuanLy"; // Tên cột hiển thị
                comboboxQuanLy.SelectedValuePath = "IDNguoiQuanLy"; // Giá trị được chọn
            }
            else
            {
                MessageBox.Show("Không có dữ liệu người quản lý!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }
        private bool AllowSave()
        {
            popup.IsOpen = false; // Tạm thời ẩn Popup
            overlayGrid.Visibility = Visibility.Collapsed;

            if (txtTinhTrang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tình trạng mặt bằng.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTinhTrang.Focus();
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }         
            else if (txtQuanLy.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn người quản lý.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuanLy.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                return true;
            }
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowSave())
            {
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                overlayGrid.Opacity = 0.5;
                return;
            }
            string sSQL = "";
            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE MatBangThuongMai SET  TinhTrang = N'{txtTinhTrang.Text}', " +
                           $"DienTich = '{txtDienTich.Text}', " +
                           $"GiaThue = '{txtGiaThue.Text}' , " +
                           $"TenDonViThue = N'{txtTenDonViThue.Text}', " +
                           $" IDNguoiQuanLy = '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}'" +
                           $" WHERE IDMBTM = {txtIDMBTM.Text}";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO MatBangThuongMai(DienTich, TenDonViThue, GiaThue, TinhTrang, IDNguoiQuanLy) VALUES('{txtDienTich.Text}', N'{txtTenDonViThue.Text}', {txtGiaThue.Text}, " +
                           $"N'{txtTinhTrang.Text}', '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}')";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
            Load();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dtview.SelectedItem != null)
            {
                _trangThaiHienTai = TrangThaiHienTai.Sua;
                LoadStatus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thông tin cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            _trangThaiHienTai = TrangThaiHienTai.Them;
            LoadStatus();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dtview.SelectedItem != null)
            {
                DataRowView selectedRow = dtview.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    string id = selectedRow["IDMBTM"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa mặt bằng này?", "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        string sSQL = $"DELETE FROM MatBangThuongMai WHERE IDMBTM = {id}";
                        Connect.DataExecution1(sSQL);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Load();
                    }
                }
            }
            else
            {
                // Hiển thị thông báo khi không có gì được chọn
                MessageBox.Show("Vui lòng chọn thông tin cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}

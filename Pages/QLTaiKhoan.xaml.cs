using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for QLTaiKhoan.xaml
    /// </summary>
    public partial class QLTaiKhoan : Page
    {
        private string userRole;
        private DataTable dGrid = new DataTable();
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }

        public QLTaiKhoan() {
            InitializeComponent();
            Load();
        }

        private void Load() {
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxQuyen();
        }
        private void LoadDataGrid() {
            dGrid = Connect.DataTransport("SELECT CONCAT(CuDan.TenCuDan, ' ', NguoiQuanLy.TenNguoiQuanLy, ' ', Admin.TenAdmin) AS TenNguoiDung, * " +
                "FROM TaiKhoan LEFT JOIN CuDan ON TaiKhoan.IDCuDan = CuDan.IDCuDan " +
                "LEFT JOIN NguoiQuanLy ON TaiKhoan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                "LEFT JOIN Admin ON TaiKhoan.IDAdmin = Admin.IDAdmin");
            dtview.ItemsSource = dGrid.DefaultView;
        }
        private void LoadStatus() {
            DataRowView row = (DataRowView)dtview.SelectedItem;
            switch (_trangThaiHienTai) {
                case TrangThaiHienTai.Xem:
                    popup.IsOpen = false;
                    overlayGrid.Visibility = Visibility.Collapsed;

                    break;
                case TrangThaiHienTai.Them:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    txtNgaySinh.Visibility = Visibility.Visible;
                    txtSDT.Visibility = Visibility.Visible;

                    txtIDTaiKhoan.Text = "";
                    txtTenNguoiDung.Text = "";
                    txtMatKhau.Password = "123456";
                    comboboxQuyen.Text = "Quản lý";
                    

                    comboboxQuyen.IsEnabled = false;
                    txtIDTaiKhoan.IsReadOnly = false;
                    txtTenNguoiDung.IsReadOnly = false;

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    txtNgaySinh.Visibility = Visibility.Collapsed;
                    txtSDT.Visibility = Visibility.Collapsed;

                    txtIDTaiKhoan.Text = row["IDTaiKhoan"].ToString();
                    txtMatKhau.Password = row["MatKhau"].ToString();
                    txtTenNguoiDung.Text = row["TenNguoiDung"].ToString();
                    comboboxQuyen.SelectedValue = row["QuyenHan"].ToString();

                    txtIDTaiKhoan.IsReadOnly = true;
                    comboboxQuyen.IsEnabled = true;
                    txtTenNguoiDung.IsReadOnly = true;
                    comboboxQuyen.IsReadOnly = false;

                    break;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtTimKiem.Text)) {
                string searchText = txtTimKiem.Text.ToLower().Trim();

                string sSQL = $"SELECT CONCAT(CuDan.TenCuDan, ' ', NguoiQuanLy.TenNguoiQuanLy, ' ', Admin.TenAdmin) AS TenNguoiDung, *  " +
                    $"FROM TaiKhoan  LEFT JOIN CuDan ON TaiKhoan.IDCuDan = CuDan.IDCuDan  " +
                    $"LEFT JOIN NguoiQuanLy ON TaiKhoan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy  " +
                    $"LEFT JOIN Admin ON TaiKhoan.IDAdmin = Admin.IDAdmin " +
                    $"WHERE LOWER(IDTaiKhoan) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(MatKhau) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(QuyenHan) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(CuDan.TenCuDan) LIKE LOWER(N'%{searchText}%') " +
                    $" OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                    $" OR LOWER(Admin.TenAdmin) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(TaiKhoan.IDCuDan) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(TaiKhoan.IDNguoiQuanLy) LIKE LOWER(N'%{searchText}%')  " +
                    $"OR LOWER(TaiKhoan.IDAdmin) LIKE LOWER(N'%{searchText}%');";

                DataTable dTimKiem = Connect.DataTransport(sSQL);
                dtview.ItemsSource = dTimKiem.DefaultView;
            }
            else {
                LoadDataGrid();
            }
        }
        private void dtview_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dtview.SelectedItem != null) {
                DataRowView row = (DataRowView)dtview.SelectedItem;

                txtIDTaiKhoan.Text = row["IDTaiKhoan"].ToString();
                txtMatKhau.Password = row["MatKhau"].ToString();
                txtTenNguoiDung.Text = row["TenNguoiDung"].ToString();
                comboboxQuyen.SelectedValue = row["QuyenHan"].ToString();
            }
        }
        private void comboboxQuyen_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (comboboxQuyen.SelectedItem != null) {
                string selected = ((DataRowView)comboboxQuyen.SelectedItem)["QuyenHan"].ToString();

                DataTable dt_qh = Connect.DataTransport($"SELECT QuyenHan FROM TaiKhoan WHERE QuyenHan = '{selected}'");
                if (dt_qh.Rows.Count > 0) {
                    comboboxQuyen.SelectedValue = dt_qh.Rows[0]["QuyenHan"].ToString();
                }
            }
        }
        private void LoadComboBoxQuyen() {
            // Lấy danh sách ID 
            DataTable dt_qh = Connect.DataTransport("SELECT DISTINCT QuyenHan FROM TaiKhoan");

            if (dt_qh.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxQuyen.ItemsSource = dt_qh.DefaultView;
                comboboxQuyen.DisplayMemberPath = "QuyenHan"; // Tên cột hiển thị
                comboboxQuyen.SelectedValuePath = "QuyenHan"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu quyền!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnHuy_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }
        private bool AllowSave() {
            popup.IsOpen = false; // Tạm thời ẩn Popup
            overlayGrid.Visibility = Visibility.Collapsed;

            if (txtIDTaiKhoan.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập ID.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMatKhau.Focus();
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (txtTenNguoiDung.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập tên người dùng.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuyen.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (txtNgaySinh.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa chọn ngày sinh.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuyen.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (txtSDT.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập số điện thoại.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuyen.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (txtSDT.Text.Length != 10) { 
                MessageBox.Show("Số điện thoại phải đủ 10 ký tự.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSDT.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (comboboxQuyen.SelectedValue == "") {
                MessageBox.Show("Bạn chưa chọn quyền.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuyen.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else {
                return true;
            }
        }
        private bool kiemtraIDTaiKhoan(string IDTaiKhoan) {
            popup.IsOpen = false; // Tạm thời ẩn Popup
            overlayGrid.Visibility = Visibility.Collapsed;

            string sSQL = $"SELECT COUNT(*) FROM TaiKhoan WHERE IDTaiKhoan = '{IDTaiKhoan}'";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0) {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            return false;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e) {            
            string sSQL = "";
            if (_trangThaiHienTai == TrangThaiHienTai.Them) {
                if (kiemtraIDTaiKhoan(txtIDTaiKhoan.Text.Trim())) {
                    MessageBox.Show("ID tài khoản đã tồn tại.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtIDTaiKhoan.Focus();
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    return;
                }
            }


            switch (_trangThaiHienTai) {
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE TaiKhoan SET MatKhau = '{txtMatKhau.Password}' WHERE IDTaiKhoan = '{txtIDTaiKhoan.Text}'";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    if (!AllowSave()) {
                        popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                        overlayGrid.Visibility = Visibility.Visible;
                        overlayGrid.Opacity = 0.5;
                        return;
                    }
                    DateTime ngaySinh = txtNgaySinh.SelectedDate.Value;
                    string formattedDate = ngaySinh.ToString("yyyy-MM-dd");
                    sSQL = $"INSERT INTO NguoiQuanLy (IDNguoiQuanLy, TenNguoiQuanLy, NgaySinh, SDTNguoiQuanLy) " +
                        $"VALUES ('{txtIDTaiKhoan.Text}', N'{txtTenNguoiDung.Text}', '{formattedDate}', '{txtSDT.Text}')";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
            Load();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e) {
            if (dtview.SelectedItem != null) {
                _trangThaiHienTai = TrangThaiHienTai.Sua;
                LoadStatus();
            }
            else {
                MessageBox.Show("Vui lòng chọn thông tin cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Them;
            LoadStatus();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e) {
            if (dtview.SelectedItem != null) {
                string sSQL = "";
                DataRowView selectedRow = dtview.SelectedItem as DataRowView;
                if (selectedRow != null) {
                    string id = selectedRow["IDTaiKhoan"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa tài khoản có ID là " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        sSQL = $"DELETE FROM TaiKhoan WHERE IDTaiKhoan = '{id}'";
                        // Thực thi câu lệnh xóa
                        int result = Connect.DataExecution1(sSQL);
                        if (result == 1) {
                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataGrid(); // Cập nhật lại DataGrid
                        }
                        else {
                            MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else {
                // Hiển thị thông báo khi không có gì được chọn
                MessageBox.Show("Vui lòng chọn thông tin cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}

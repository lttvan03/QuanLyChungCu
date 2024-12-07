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
    /// Interaction logic for QLCuDan.xaml
    /// </summary>
    public partial class QLCuDan : Page
    {
        private string currentUserQH;
        private string currentUserID;
        private string soCanHo;
        private DataTable dGrid = new DataTable();
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }
        public QLCuDan()
        {
            currentUserID = GetCurrentUserID();
            currentUserQH = GetCurrentUserQH();
            soCanHo = GetCurrentSoCanHo();

            InitializeComponent();
            Load();
        }
        private string GetCurrentUserID()
        {
            return App.Current.Properties["ID"]?.ToString();
        }
        private string GetCurrentUserQH()
        {
            return App.Current.Properties["UserRole"]?.ToString();
        }
        private string GetCurrentSoCanHo() {
            return App.Current.Properties["SoCanHo"]?.ToString();
        }

        private void Load()
        {
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxQuanLy();
            LoadComboBoxCanHo();
        }
        private void LoadDataGrid()
        {
            if (currentUserQH == "Cư dân")
            {
                string sSQL = $"SELECT * FROM CuDan INNER JOIN NguoiQuanLy ON CuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy WHERE SoCanHo = '{soCanHo}'";
                dGrid = Connect.DataTransport(sSQL);
                btnThem.Visibility = Visibility.Collapsed;
                btnSua.Visibility = Visibility.Collapsed;
                btnXoa.Visibility = Visibility.Collapsed;
                dtview.ItemsSource = dGrid.DefaultView;
            }
            else if (currentUserQH == "Admin" || currentUserQH == "Quản lý")
            {
                dGrid = Connect.DataTransport("SELECT * FROM CuDan INNER JOIN NguoiQuanLy ON CuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");
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

                    txtIDCuDan.Text = "";
                    txtTenCuDan.Text = "";
                    dpNgaySinh.SelectedDate = null;
                    comboboxGioiTinh.SelectedItem = "";
                    txtGiayToTuyThan.Text = "";
                    comboboxCanHo.SelectedValue = "";
                    comboboxQuanLy.SelectedValue = "";
                    txtQuanLy.Text = "";

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtIDCuDan.Text = row["IDCuDan"].ToString();
                    txtTenCuDan.Text = row["TenCuDan"].ToString();
                    dpNgaySinh.SelectedDate = Convert.ToDateTime(row["NgaySinh"]);
                    string gioiTinh = row["GioiTinh"].ToString();
                    comboboxGioiTinh.SelectedItem = comboboxGioiTinh.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == gioiTinh);
                    txtGiayToTuyThan.Text = row["GiayToTuyThan"].ToString();
                    comboboxCanHo.SelectedValue = row["SoCanHo"].ToString();
                    comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

                    break;
            }
            if (_trangThaiHienTai == TrangThaiHienTai.Sua)
            {
                txtIDCuDan.IsEnabled = false;
            }
            else
            {
                txtIDCuDan.IsEnabled = true;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTimKiem.Text))
            {
                string searchText = txtTimKiem.Text.ToLower().Trim();
                string sSQL = "";
                if (currentUserQH == "Cư dân") {
                    sSQL = $"SELECT * FROM CuDan " +
                               $"INNER JOIN NguoiQuanLy ON CuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                               $"WHERE (LOWER(CuDan.IDCuDan) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(CuDan.TenCuDan) LIKE LOWER(N'%{searchText}%') " +
                               $"OR LOWER(CuDan.GiayToTuyThan) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(CuDan.SoCanHo) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                               $"OR LOWER(CuDan.IDNguoiQuanLy) LIKE LOWER('%{searchText}%') " +
                               $"OR CONVERT(VARCHAR, CuDan.NgaySinh, 103) LIKE '%{searchText}%' " +
                               $"OR LOWER(CuDan.GioiTinh) LIKE LOWER(N'%{searchText}%')) " +
                               $"AND CuDan.SoCanHo = '{soCanHo}'";
                } else {
                    sSQL = $"SELECT * FROM CuDan " +
                               $"INNER JOIN NguoiQuanLy ON CuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                               $"WHERE LOWER(CuDan.IDCuDan) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(CuDan.TenCuDan) LIKE LOWER(N'%{searchText}%') " +
                               $"OR LOWER(CuDan.GiayToTuyThan) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(CuDan.SoCanHo) LIKE LOWER('%{searchText}%') " +
                               $"OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                               $"OR LOWER(CuDan.IDNguoiQuanLy) LIKE LOWER('%{searchText}%') " +
                               $"OR CONVERT(VARCHAR, CuDan.NgaySinh, 103) LIKE '%{searchText}%' " +
                               $"OR LOWER(CuDan.GioiTinh) LIKE LOWER(N'%{searchText}%')";
                }

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

                txtIDCuDan.Text = row["IDCuDan"].ToString();
                txtTenCuDan.Text = row["TenCuDan"].ToString();
                dpNgaySinh.SelectedDate = Convert.ToDateTime(row["NgaySinh"]);
                comboboxGioiTinh.SelectedItem = row["GioiTinh"].ToString();
                txtGiayToTuyThan.Text = row["GiayToTuyThan"].ToString();
                comboboxCanHo.SelectedValue = row["SoCanHo"].ToString();
                comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
            }
        }

        private void LoadComboBoxCanHo()
        {
            DataTable dt_CanHo = Connect.DataTransport("SELECT SoCanHo FROM CanHo");

            if (dt_CanHo.Rows.Count > 0)
            {
                comboboxCanHo.ItemsSource = dt_CanHo.DefaultView;
                comboboxCanHo.DisplayMemberPath = "SoCanHo";
                comboboxCanHo.SelectedValuePath = "SoCanHo";
            }
            else
            {
                MessageBox.Show("Không có dữ liệu tầng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ComboBoxCanHo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxCanHo.SelectedItem != null)
            {
                string selectedSoCanHo = ((DataRowView)comboboxCanHo.SelectedItem)["SoCanHo"].ToString();
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
        private void comboboxGioiTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxGioiTinh.SelectedItem != null)
            {
                string selectedGioiTinh = (comboboxGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString();

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

            if (txtIDCuDan.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập ID cư dân.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtIDCuDan.Focus();
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }

            else if (txtTenCuDan.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tên cư dân.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenCuDan.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (txtGiayToTuyThan.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập giấy tờ tùy thân.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtGiayToTuyThan.Focus();
                popup.IsOpen = true;
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
        private bool kiemtraIDCuDan(string IDCuDan)
        {
            string sSQL = $"SELECT COUNT(*) FROM CuDan WHERE IDCuDan = '{IDCuDan}'";
            DataTable dt = Connect.DataTransport(sSQL);
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            return false;
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

            if (_trangThaiHienTai == TrangThaiHienTai.Them)
            {
                if (kiemtraIDCuDan(txtIDCuDan.Text.Trim()))
                {
                    MessageBox.Show("Id Cư Dân đã tồn tại.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtIDCuDan.Focus();
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    return;
                }
            }

            string sSQL = "";
            string selectedSoCanHo = ((DataRowView)comboboxCanHo.SelectedItem)["SoCanHo"].ToString();
            string selectedIDNguoiQuanLy = ((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();
            string gioiTinh = (comboboxGioiTinh.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;

            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE CuDan SET TenCuDan = N'{txtTenCuDan.Text}', NgaySinh = '{dpNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd")}', " +
                                           $"GioiTinh = N'{gioiTinh}', GiayToTuyThan = '{txtGiayToTuyThan.Text}', " +
                                           $"SoCanHo = '{selectedSoCanHo}', " +
                                           $"IDNguoiQuanLy = '{selectedIDNguoiQuanLy}' " +
                                           $"WHERE IDCuDan = '{txtIDCuDan.Text}'";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO CuDan(IDCuDan, TenCuDan, NgaySinh, GioiTinh, GiayToTuyThan, SoCanHo, IDNguoiQuanLy) VALUES(" +
                          $"'{txtIDCuDan.Text}', N'{txtTenCuDan.Text}', '{dpNgaySinh.SelectedDate.Value.ToString("yyyy-MM-dd")}', " +
                          $"N'{comboboxGioiTinh.SelectedItem}', '{txtGiayToTuyThan.Text}', " +
                          $"'{selectedSoCanHo}', " +
                          $"'{selectedIDNguoiQuanLy}')";
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
                string sSQL = "";
                string sSQL1 = "";
                DataRowView selectedRow = dtview.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    string id = selectedRow["IDCuDan"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa Cư Dân có ID là " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sSQL = $"DELETE FROM CuDan WHERE IDCuDan = '{txtIDCuDan.Text}'";
                        sSQL1 = $"DELETE FROM TaiKhoan WHERE IDCuDan = '{txtIDCuDan.Text}'";
                        // Thực thi câu lệnh xóa
                        int result1 = Connect.DataExecution1(sSQL1);
                        int result = Connect.DataExecution1(sSQL);
                        LoadDataGrid(); // Cập nhật lại DataGrid
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
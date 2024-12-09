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
    /// Interaction logic for HDCuDan.xaml
    /// </summary>
    public partial class HDCuDan : Page
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

        public HDCuDan() {
            InitializeComponent();
            Load();
        }
        private void Load() {
            currentUserID = GetCurrentUserID();
            currentUserQH = GetCurrentUserQH();
            soCanHo = GetCurrentSoCanHo();
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxTrangThai();
            LoadComboBoxQuanLy();
            LoadComboBoxCanHo();
        }
        private string GetCurrentUserID() {
            return App.Current.Properties["ID"]?.ToString();
        }
        private string GetCurrentUserQH() {
            return App.Current.Properties["UserRole"]?.ToString();
        }
        private string GetCurrentSoCanHo() {
            return App.Current.Properties["SoCanHo"]?.ToString();
        }
        private void LoadDataGrid() {
            if (currentUserQH == "Cư dân") {
                string sSQL = $"SELECT * FROM HoaDonCuDan INNER JOIN NguoiQuanLy ON HoaDonCuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy WHERE HoaDonCuDan.SoCanHo = '{soCanHo}'";
                dGrid = Connect.DataTransport(sSQL);
                btnThem.Visibility = Visibility.Collapsed;
                btnSua.Visibility = Visibility.Collapsed;
                btnXoa.Visibility = Visibility.Collapsed;
                dtview.ItemsSource = dGrid.DefaultView;
            }
            else if (currentUserQH == "Admin" || currentUserQH == "Quản lý") {
                dGrid = Connect.DataTransport("SELECT * FROM HoaDonCuDan INNER JOIN NguoiQuanLy ON HoaDonCuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");
                dtview.ItemsSource = dGrid.DefaultView;
            }
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

                    txtIDHoaDon.Text = "";
                    floatSoTien.Text = "";
                    dtHanThanhToan.SelectedDate = null;
                    comboboxTrangThai.SelectedValue = "";
                    comboboxSoCanHo.SelectedValue = null;
                    txtQuanLy.Text = "";
                    comboboxNguoiQuanLy.SelectedValue = null;

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtIDHoaDon.Text = row["IDHoaDon"].ToString();
                    floatSoTien.Text = row["SoTien"].ToString();
                    dtHanThanhToan.SelectedDate = Convert.ToDateTime(row["HanDong"]);
                    comboboxTrangThai.SelectedValue = row["TrangThai"].ToString();
                    comboboxNguoiQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();
                    comboboxSoCanHo.SelectedValue = row["SoCanHo"].ToString();

                    break;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtTimKiem.Text)) {
                string searchText = txtTimKiem.Text.ToLower().Trim();
                string sSQL = "";
                List<string> conditions = new List<string>();
                // Tìm kiếm theo ID hoặc thời gian (nếu là số nguyên)
                if (int.TryParse(searchText, out int id)) {
                    conditions.Add($"IDHoaDon = {id}");
                    conditions.Add($"YEAR(HanDong) = {id}");
                    conditions.Add($"MONTH(HanDong) = {id}");
                    conditions.Add($"DAY(HanDong) = {id}");
                }
                // Tìm kiếm theo số tiền (nếu là số thực)
                if (float.TryParse(searchText, out float tien)) {
                    conditions.Add($"SoTien = {tien}");
                }
                conditions.Add($"LOWER(HoaDonCuDan.IDNguoiQuanLy) LIKE LOWER(N'%{searchText}%')");
                conditions.Add($"LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%')");
                conditions.Add($"LOWER(SoCanHo) LIKE LOWER(N'%{searchText}%')");
                conditions.Add($"LOWER(TrangThai) LIKE LOWER(N'%{searchText}%')");

                if (currentUserQH == "Cư dân") {
                    sSQL = $"SELECT * FROM HoaDonCuDan " +
                        $"INNER JOIN NguoiQuanLy ON HoaDonCuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy" +
                        $" WHERE ({string.Join(" OR ", conditions)}) AND SoCanHo = '{soCanHo}'";
                } else {
                    sSQL = $"SELECT * FROM HoaDonCuDan " +
                        $"INNER JOIN NguoiQuanLy ON HoaDonCuDan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy" +
                        $" WHERE {string.Join(" OR ", conditions)}";
                }

                // Thực thi câu SQL
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

                txtIDHoaDon.Text = row["IDHoaDon"].ToString();
                floatSoTien.Text = row["SoTien"].ToString();
                dtHanThanhToan.SelectedDate = Convert.ToDateTime(row["HanDong"]);
                comboboxTrangThai.SelectedValue = row["TrangThai"].ToString();
                comboboxNguoiQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                comboboxSoCanHo.SelectedValue = row["SoCanHo"].ToString();
                txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();
            }
        }
        private void LoadComboBoxTrangThai() {
            DataTable dt_cudan = new DataTable();
            dt_cudan.Columns.Add("TrangThai", typeof(string));
            dt_cudan.Rows.Add("Chưa thanh toán");
            dt_cudan.Rows.Add("Đã thanh toán");
            dt_cudan.Rows.Add("Quá hạn");
            comboboxTrangThai.ItemsSource = dt_cudan.DefaultView;
            comboboxTrangThai.DisplayMemberPath = "TrangThai"; // Tên cột hiển thị
            comboboxTrangThai.SelectedValuePath = "TrangThai"; // Giá trị được chọn
        }
        private void comboboxNguoiQuanLy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (comboboxNguoiQuanLy.SelectedItem != null) {
                string selectedID = ((DataRowView)comboboxNguoiQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();

                DataTable dt_quanly = Connect.DataTransport($"SELECT TenNguoiQuanLy FROM NguoiQuanLy WHERE IDNguoiQuanLy = '{selectedID}'");

                if (dt_quanly.Rows.Count > 0) {
                    txtQuanLy.Text = dt_quanly.Rows[0]["TenNguoiQuanLy"].ToString();
                }
                else {
                    txtQuanLy.Clear();
                }
            }
        }
        private void LoadComboBoxQuanLy() {
            DataTable dt_quanly = Connect.DataTransport("SELECT DISTINCT IDNguoiQuanLy FROM NguoiQuanLy");

            if (dt_quanly.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxNguoiQuanLy.ItemsSource = dt_quanly.DefaultView;
                comboboxNguoiQuanLy.DisplayMemberPath = "IDNguoiQuanLy"; // Tên cột hiển thị
                comboboxNguoiQuanLy.SelectedValuePath = "IDNguoiQuanLy"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu người quản lý!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadComboBoxCanHo() {
            DataTable dt_canho = Connect.DataTransport("SELECT DISTINCT SoCanHo FROM CanHo");

            if (dt_canho.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxSoCanHo.ItemsSource = dt_canho.DefaultView;
                comboboxSoCanHo.DisplayMemberPath = "SoCanHo"; // Tên cột hiển thị
                comboboxSoCanHo.SelectedValuePath = "SoCanHo"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu căn hộ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnHuy_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }
        private bool AllowSave() {
            popup.IsOpen = false; // Tạm thời ẩn Popup
            overlayGrid.Visibility = Visibility.Collapsed;

            if (floatSoTien.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập số tiền.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (dtHanThanhToan.SelectedDate == null) {
                MessageBox.Show("Bạn chưa nhập hạn thanh toán.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (comboboxTrangThai.SelectedItem == null) {
                MessageBox.Show("Bạn chưa chọn trạng thái hóa đơn.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (comboboxSoCanHo.SelectedItem == null) {
                MessageBox.Show("Bạn chưa chọn căn hộ.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            else if (comboboxNguoiQuanLy.SelectedItem == null) {
                MessageBox.Show("Bạn chưa chọn người quản lý.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }

            else {
                return true;
            }

        }
        private void btnLuu_Click(object sender, RoutedEventArgs e) {
            if (!AllowSave()) {
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                overlayGrid.Opacity = 0.5;
                return;
            }
            string sSQL = "";
            switch (_trangThaiHienTai) {
                // Kiểm tra nếu có giá trị được chọn trong combobox
                case TrangThaiHienTai.Sua:
                    // Khởi tạo giá trị cho các biến
                    float.TryParse(floatSoTien.Text, out float soTien);
                    DateTime? selectedDate = dtHanThanhToan.SelectedDate;
                    int.TryParse(txtIDHoaDon.Text, out int ID);
                    string trangThai = comboboxTrangThai.SelectedItem != null ? ((DataRowView)comboboxTrangThai.SelectedItem)["TrangThai"].ToString() : string.Empty;
                    string soCanHo = comboboxSoCanHo.SelectedItem != null ? ((DataRowView)comboboxSoCanHo.SelectedItem)["SoCanHo"].ToString() : string.Empty;
                    string idQuanLy = comboboxNguoiQuanLy.SelectedItem != null ? ((DataRowView)comboboxNguoiQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString() : string.Empty;

                    sSQL = $"UPDATE HoaDonCuDan SET SoCanHo = '{soCanHo}'," +
                        $" SoTien = {soTien}, " +
                        $"HanDong = '{selectedDate?.ToString("yyyy-MM-dd")}', " +
                        $"TrangThai = N'{trangThai}', IDNguoiQuanLy = '{idQuanLy}'" +
                        $"WHERE IDHoaDon = {ID}";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    float.TryParse(floatSoTien.Text, out float soTienadd);
                    DateTime? selectedDateadd = dtHanThanhToan.SelectedDate;
                    int.TryParse(txtIDHoaDon.Text, out int IDad);
                    string trangThaiadd = comboboxTrangThai.SelectedItem != null ? ((DataRowView)comboboxTrangThai.SelectedItem)["TrangThai"].ToString() : string.Empty;
                    string soCanHoadd = comboboxSoCanHo.SelectedItem != null ? ((DataRowView)comboboxSoCanHo.SelectedItem)["SoCanHo"].ToString() : string.Empty;
                    string idQuanLyadd = comboboxNguoiQuanLy.SelectedItem != null ? ((DataRowView)comboboxNguoiQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString() : string.Empty;

                    sSQL = $"INSERT INTO HoaDonCuDan(SoCanHo, SoTien, HanDong, TrangThai, IDNguoiQuanLy) " +
                        $"VALUES ('{soCanHoadd}', {soTienadd}, '{selectedDateadd?.ToString("yyyy-MM-dd")}'," +
                        $"N'{trangThaiadd}', '{idQuanLyadd}')"; 
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
                    string id = selectedRow["IDHoaDon"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa hóa đơn có ID là " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        sSQL = $"DELETE FROM HoaDonCuDan WHERE IDHoaDon = '{id}'";
                        // Thực thi câu lệnh xóa
                        Connect.DataExecution1(sSQL);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        LoadDataGrid(); // Cập nhật lại DataGrid
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

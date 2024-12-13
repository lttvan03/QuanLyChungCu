using QuanLyChungCu.ConnectDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2,
            HienThi = 3
        }

        public HDThuongMai() {
            InitializeComponent();
            Load();

        }

        private void Load() {
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxTrangThai();
            LoadComboBoxMBTM();
        }
        private void LoadDataGrid() {
            dGrid = Connect.DataTransport("SELECT * FROM HoaDonTM");
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
                    btnLuu.Visibility = Visibility.Visible;

                    txtIDHoaDon.Text = "";
                    floatSoTien.Text = "";
                    dtHanThanhToan.SelectedDate = null;
                    comboboxTrangThai.SelectedValue = "";
                    comboboxMBTM.SelectedValue = null;

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    btnLuu.Visibility = Visibility.Visible;

                    txtIDHoaDon.Text = row["IDHoaDon"].ToString();
                    floatSoTien.Text = row["SoTien"].ToString();
                    dtHanThanhToan.SelectedDate = Convert.ToDateTime(row["HanThanhToan"]);
                    comboboxTrangThai.SelectedValue = row["TrangThai"].ToString();
                    comboboxMBTM.SelectedValue = row["IDMBTM"].ToString();

                    break;
                case TrangThaiHienTai.HienThi:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    btnLuu.Visibility = Visibility.Collapsed;

                    txtIDHoaDon.Text = row["IDHoaDon"].ToString();
                    floatSoTien.Text = row["SoTien"].ToString();
                    dtHanThanhToan.SelectedDate = Convert.ToDateTime(row["HanThanhToan"]);
                    comboboxTrangThai.SelectedValue = row["TrangThai"].ToString();
                    comboboxMBTM.SelectedValue = row["IDMBTM"].ToString();

                    break;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtTimKiem.Text)) {
                string searchText = txtTimKiem.Text.ToLower().Trim();
                List<string> conditions = new List<string>();

                // Tìm kiếm theo ID hoặc thời gian (nếu là số nguyên)
                if (int.TryParse(searchText, out int id)) {
                    conditions.Add($"IDHoaDon = {id}");
                    conditions.Add($"IDMBTM = {id}");
                    conditions.Add($"YEAR(HanThanhToan) = {id}");
                    conditions.Add($"MONTH(HanThanhToan) = {id}");
                    conditions.Add($"DAY(HanThanhToan) = {id}");
                }

                // Tìm kiếm theo số tiền (nếu là số thực)
                if (float.TryParse(searchText, out float tien)) {
                    conditions.Add($"SoTien = {tien}");
                }

                // Tìm kiếm theo trạng thái (nếu là chuỗi)
                conditions.Add($"LOWER(TrangThai) LIKE LOWER(N'%{searchText}%')");

                // Kết hợp các điều kiện với "OR"
                string sSQL = $"SELECT * FROM HoaDonTM WHERE {string.Join(" OR ", conditions)}";

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
                dtHanThanhToan.SelectedDate = Convert.ToDateTime(row["HanThanhToan"]);
                comboboxTrangThai.SelectedValue = row["TrangThai"].ToString();
                comboboxMBTM.SelectedValue = row["IDMBTM"].ToString();
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
        private void LoadComboBoxMBTM() {
            // Lấy danh sách ID từ bảng CuDan
            DataTable dt_quanly = Connect.DataTransport("SELECT DISTINCT IDMBTM FROM MatBangThuongMai");

            if (dt_quanly.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxMBTM.ItemsSource = dt_quanly.DefaultView;
                comboboxMBTM.DisplayMemberPath = "IDMBTM"; // Tên cột hiển thị
                comboboxMBTM.SelectedValuePath = "IDMBTM"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu mặt bằng thương mại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (dtHanThanhToan.SelectedDate == null) {
                MessageBox.Show("Bạn chưa nhập hạn thanh toán.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            if (comboboxTrangThai.SelectedItem == null) {
                MessageBox.Show("Bạn chưa chọn trạng thái hóa đơn.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            if (comboboxMBTM.SelectedItem == null) {
                MessageBox.Show("Bạn chưa chọn mặt bằng thương mại.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }         
            return true;
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
                    int.TryParse(comboboxMBTM.SelectedValue?.ToString(), out int idmbtm);
                    string trangThai = comboboxTrangThai.SelectedItem != null ? ((DataRowView)comboboxTrangThai.SelectedItem)["TrangThai"].ToString() : string.Empty;

                    // Tạo câu lệnh SQL
                    sSQL = $"UPDATE HoaDonTM SET SoTien = {soTien}," +
                           $" HanThanhToan = N'{selectedDate?.ToString("yyyy-MM-dd")}', " +
                           $"TrangThai = N'{trangThai}', " +
                           $"IDMBTM = {idmbtm} " +
                           $"WHERE IDHoaDon = {ID}"; Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    float.TryParse(floatSoTien.Text, out float soTienAdd);
                    DateTime? selectedDateAdd = dtHanThanhToan.SelectedDate;
                    int.TryParse(comboboxMBTM.SelectedValue?.ToString(), out int idmbtmAdd);
                    string trangThaiAdd = comboboxTrangThai.SelectedItem != null ? ((DataRowView)comboboxTrangThai.SelectedItem)["TrangThai"].ToString() : string.Empty;

                    // Tạo câu lệnh SQL
                    sSQL = $"INSERT INTO HoaDonTM(SoTien, HanThanhToan, TrangThai, IDMBTM) VALUES ({soTienAdd}, " +
                        $"'{selectedDateAdd?.ToString("yyyy-MM-dd")}', N'{trangThaiAdd}', {idmbtmAdd})"; Connect.DataExecution1(sSQL);
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
                        sSQL = $"DELETE FROM HoaDonTM WHERE IDHoaDon = '{id}'";
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

        private void btnXem_Click(object sender, RoutedEventArgs e) {
            if (dtview.SelectedItem != null) {
                _trangThaiHienTai = TrangThaiHienTai.HienThi;
                LoadStatus();
            }
            else {
                MessageBox.Show("Vui lòng chọn thông tin cần xem!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}

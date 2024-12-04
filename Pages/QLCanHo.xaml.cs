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
    /// Interaction logic for QLCanHo.xaml
    /// </summary>
    public partial class QLCanHo : Page
    {
        private DataTable dGrid = new DataTable();
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }
        public QLCanHo()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxQuanLy();
            LoadComboBoxTang();
        }
        private void LoadDataGrid()
        {
            dGrid = Connect.DataTransport("SELECT * FROM CanHo INNER JOIN NguoiQuanLy ON CanHo.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");


            dGrid.Columns.Add("SoXe", typeof(string));

            foreach (DataRow row in dGrid.Rows)
            {
                int soOTo = Convert.ToInt32(row["SoOTo"]);
                int soXeMay = Convert.ToInt32(row["SoXeMay"]);
                int soXeDap = Convert.ToInt32(row["SoXeDap"]);
                string SoXe = $"Ô tô: {soOTo}, Xe máy: {soXeMay}, Xe đạp: {soXeDap}";
                row["SoXe"] = SoXe;
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

                    txtSoCanHo.Text = "";
                    comboboxTang.SelectedValue = "";
                    txtSoCuDan.Text = "";
                    txtSoOTo.Text = "";
                    txtSoXeMay.Text = "";
                    txtSoXeDap.Text = "";
                    comboboxQuanLy.SelectedValue = "";
                    txtQuanLy.Text = "";

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtSoCanHo.Text = row["SoCanHo"].ToString();
                    comboboxTang.SelectedValue = row["SoTang"].ToString();
                    txtSoCuDan.Text = row["SoCuDan"].ToString();
                    txtSoOTo.Text = row["SoOTo"].ToString();
                    txtSoXeMay.Text = row["SoXeMay"].ToString();
                    txtSoXeDap.Text = row["SoXeDap"].ToString();
                    comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

                    break;
            }
            if (_trangThaiHienTai == TrangThaiHienTai.Sua)
            {
                txtSoCanHo.IsEnabled = false; // Ngăn không cho chỉnh sửa ComboBox tầng
            }
            else
            {
                txtSoCanHo.IsEnabled = true; // Cho phép chỉnh sửa nếu không phải chế độ sửa
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTimKiem.Text))
            {
                string searchText = txtTimKiem.Text.ToLower().Trim();

                string sSQL = $"SELECT * FROM CanHo " +
                    $"INNER JOIN NguoiQuanLy ON CanHo.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                    $"WHERE LOWER(SoCanHo) LIKE LOWER('%{searchText}%') OR LOWER(SoTang) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(SoCuDan) LIKE LOWER('%{searchText}%') OR LOWER(SoOTo) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(SoXeMay) LIKE LOWER('%{searchText}%') OR LOWER(SoXeDap) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                    $"OR LOWER(CanHo.IDNguoiQuanLy) LIKE LOWER('%{searchText}%')";

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

                txtSoCanHo.Text = row["SoCanHo"].ToString();
                comboboxTang.SelectedValue = row["SoTang"].ToString();
                txtSoCuDan.Text = row["SoCuDan"].ToString();
                txtSoOTo.Text = row["SoOTo"].ToString();
                txtSoXeMay.Text = row["SoXeMay"].ToString();
                txtSoXeDap.Text = row["SoXeDap"].ToString();
                comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();
            }
        }

        private void LoadComboBoxTang()
        {
            // Lấy danh sách tầng từ bảng Tang
            DataTable dt_tang = Connect.DataTransport("SELECT SoTang FROM Tang");

            if (dt_tang.Rows.Count > 0)
            {
                // Gán dữ liệu vào ComboBox
                comboboxTang.ItemsSource = dt_tang.DefaultView;
                comboboxTang.DisplayMemberPath = "SoTang"; // Tên cột hiển thị
                comboboxTang.SelectedValuePath = "SoTang"; // Giá trị được chọn
            }
            else
            {
                MessageBox.Show("Không có dữ liệu tầng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void comboboxTang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxTang.SelectedItem != null)
            {
                string selectedSoTang = ((DataRowView)comboboxTang.SelectedItem)["SoTang"].ToString();
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

            if (txtSoCanHo.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập số căn hộ.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSoCanHo.Focus();
                popup.IsOpen = true; // Hiển thị lại Popup nếu cần
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }

            else if (txtSoCuDan.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập số cư dân.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSoCuDan.Focus();
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
        private bool kiemtraSoCanHo(string soCanHo)
        {
            string sSQL = $"SELECT COUNT(*) FROM CanHo WHERE SoCanHo = '{soCanHo}'";
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
                if (kiemtraSoCanHo(txtSoCanHo.Text.Trim()))
                {
                    MessageBox.Show("Số căn hộ đã tồn tại.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtSoCanHo.Focus();
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    return;
                }
            }

            string sSQL = "";
            string selectedSoTang = ((DataRowView)comboboxTang.SelectedItem)["SoTang"].ToString();
            string selectedIDNguoiQuanLy = ((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();

            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE CanHo SET SoCuDan = '{txtSoCuDan.Text}', " +
                            $"SoOTo = '{txtSoOTo.Text}', SoXeMay = '{txtSoXeMay.Text}', SoXeDap = '{txtSoXeDap.Text}', " +
                            $"IDNguoiQuanLy = '{selectedIDNguoiQuanLy}', " +
                            $"SoTang = '{selectedSoTang}' " +
                            $"WHERE SoCanHo = '{txtSoCanHo.Text}'";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO CanHo(SoCanHo, SoTang, SoCuDan, SoOTo, SoXeMay, SoXeDap, IDNguoiQuanLy) VALUES(" +
                            $"'{txtSoCanHo.Text}', '{selectedSoTang}', " +
                            $"'{txtSoCuDan.Text}', '{txtSoOTo.Text}', '{txtSoXeMay.Text}', '{txtSoXeDap.Text}', '{selectedIDNguoiQuanLy}')";
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
                DataRowView selectedRow = dtview.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    string id = selectedRow["SoCanHo"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa căn hộ có số " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sSQL = $"DELETE FROM CanHo WHERE SoCanHo = '{txtSoCanHo.Text}'";
                        // Thực thi câu lệnh xóa
                        int result = Connect.DataExecution1(sSQL);
                        if (result == 1)
                        {
                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataGrid(); // Cập nhật lại DataGrid
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
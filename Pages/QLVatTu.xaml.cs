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
    /// Interaction logic for QLVatTu.xaml
    /// </summary>
    public partial class QLVatTu : Page
    {
        private DataTable dGrid = new DataTable();
        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Them = 1,
            Sua = 2
        }
        public QLVatTu()
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
            dGrid = Connect.DataTransport("SELECT * FROM VatTu INNER JOIN NguoiQuanLy ON VatTu.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");
            foreach (DataRow row in dGrid.Rows)
            { 
                int SoLuong = Convert.ToInt32(row["SoLuong"]);
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

                    txtTenVatTu.Text = "";
                    intSoLuong.Text = "";
                    txtIDVatTu.Text = "";
                    comboboxQuanLy.SelectedValue = "";
                    txtQuanLy.Text = "";

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;

                    txtTenVatTu.Text = row["TenVatTu"].ToString();
                    intSoLuong.Text = row["SoLuong"].ToString();
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

                string sSQL = $"SELECT * FROM VatTu " +
                    $"INNER JOIN NguoiQuanLy ON VatTu.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                    $"WHERE LOWER(TenVatTu) LIKE LOWER(N'%{searchText}%') OR LOWER(SoLuong) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(IDVatTu) LIKE LOWER('%{searchText}%')" +
                    $"OR LOWER(VatTu.IDNguoiQuanLy) LIKE LOWER('%{searchText}%')" +
                    $" OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%')";

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

                txtIDVatTu.Text = row["IDVatTu"].ToString();
                txtTenVatTu.Text = row["TenVatTu"].ToString();
                intSoLuong.Text = row["SoLuong"].ToString();
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

            if (txtTenVatTu.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tên vật tư.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenVatTu.Focus();
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
            string selectedIDNguoiQuanLy = ((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();
            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE VatTu SET TenVatTu = N'{txtTenVatTu.Text}', SoLuong = N'{intSoLuong.Text}', " +
                           $"IDNguoiQuanLy = '{selectedIDNguoiQuanLy}' " +
                           $"WHERE IDVatTu = {txtIDVatTu.Text}";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO VatTu (TenVatTu, SoLuong, IDNguoiQuanLy) VALUES(N'{txtTenVatTu.Text}', N'{intSoLuong.Text}'," +
                        $" '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}')";
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
                    string id = selectedRow["IDVatTu"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa vật tư có ID là " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sSQL = $"DELETE FROM VatTu WHERE IDVatTu = '{id}'";
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

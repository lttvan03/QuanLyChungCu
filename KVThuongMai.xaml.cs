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

namespace QuanLyChungCu.Pages
{
    public partial class KVThuongMai : Page
    {
        private string currentUserQH;
        private string currentUserID;
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
            currentUserID = GetCurrentUserID();
            currentUserQH = GetCurrentUserQH();
            SetButtonVisibility();
            LoadDataGrid();
            LoadComboBoxQuanLy();
        }

        private string GetCurrentUserID()
        {
            return App.Current.Properties["ID"]?.ToString();
        }

        private string GetCurrentUserQH()
        {
            return App.Current.Properties["UserRole"]?.ToString();
        }

        private void LoadDataGrid()
        {
            string sSQL = GetQueryByRole();
            SqlParameter[] parameters = currentUserQH == "Cư dân"
                ? new SqlParameter[] { new SqlParameter("@currentUserID", currentUserID) }
                 : new SqlParameter[0];

            dGrid = Connect.DataTransport1(sSQL, parameters);
            dtview.ItemsSource = dGrid.DefaultView;
        }

        private string GetQueryByRole()
        {
            if (currentUserQH == "Cư dân")
                return "SELECT * FROM MatBangThuongMai WHERE IDNguoiQuanLy = @currentUserID";
            return "SELECT * FROM MatBangThuongMai";
        }

        private void SetButtonVisibility()
        {
            bool isAdminOrManager = currentUserQH == "Admin" || currentUserQH == "Quản lý";
            btnThem.Visibility = isAdminOrManager ? Visibility.Visible : Visibility.Collapsed;
            btnSua.Visibility = isAdminOrManager ? Visibility.Visible : Visibility.Collapsed;
            btnXoa.Visibility = isAdminOrManager ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadComboBoxQuanLy()
        {
            DataTable dtQuanLy = Connect.DataTransport("SELECT IDNguoiQuanLy, TenNguoiQuanLy FROM NguoiQuanLy");
            comboboxQuanLy.ItemsSource = dtQuanLy.DefaultView;
            comboboxQuanLy.DisplayMemberPath = "TenNguoiQuanLy";
            comboboxQuanLy.SelectedValuePath = "IDNguoiQuanLy";
        }

        private void LoadStatus()
        {
            DataRowView row = (DataRowView)dtview.SelectedItem;

            if (row == null && _trangThaiHienTai == TrangThaiHienTai.Sua)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                _trangThaiHienTai = TrangThaiHienTai.Xem;
                return;
            }


        }

        private void ClearInputFields()
        {
            txtIDMBTM.Clear();
            txtTenDonViThue.Clear();
            txtDienTich.Clear();
            txtGiaThue.Clear();
            comboboxQuanLy.SelectedValue = null;
            txtTinhTrang.Clear();
        }

        private void FillInputFields(DataRowView row)
        {
            txtIDMBTM.Text = row["IDMBTM"].ToString();
            txtTenDonViThue.Text = row["TenDonViThue"].ToString();
            txtDienTich.Text = row["DienTich"].ToString();
            txtGiaThue.Text = row["GiaThue"].ToString();
            txtTinhTrang.Text = row["TinhTrang"].ToString();
            comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTimKiem.Text))
            {
                string searchText = txtTimKiem.Text.Trim();
                string sSQL = "SELECT * FROM MatBangThuongMai WHERE TenDonViThue LIKE @searchText OR TinhTrang LIKE @searchText";
                SqlParameter[] parameters = { new SqlParameter("@searchText", $"%{searchText}%") };

                DataTable dTimKiem = Connect.DataTransport1(sSQL, parameters);
                dtview.ItemsSource = dTimKiem.DefaultView;
            }
            else
            {
                LoadDataGrid();
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowSave())
                return;

            string sSQL = GenerateSQL();
            try
            {
                Connect.DataExecution1(sSQL);
                MessageBox.Show("Dữ liệu đã được lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                _trangThaiHienTai = TrangThaiHienTai.Xem;
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateSQL()
        {
            string sSQL = string.Empty;
            switch (_trangThaiHienTai)
            {
                case TrangThaiHienTai.Them:
                    sSQL = "INSERT INTO MatBangThuongMai (TenDonViThue, DienTich, GiaThue, TinhTrang, IDNguoiQuanLy) " +
                           $"VALUES (N'{txtTenDonViThue.Text}', {txtDienTich.Text}, {txtGiaThue.Text}, N'{txtTinhTrang.Text}', '{comboboxQuanLy.SelectedValue}')";
                    break;
                case TrangThaiHienTai.Sua:
                    sSQL = "UPDATE MatBangThuongMai SET TenDonViThue = N'{txtTenDonViThue.Text}', " +
                           $"DienTich = {txtDienTich.Text}, GiaThue = {txtGiaThue.Text}, TinhTrang = N'{txtTinhTrang.Text}', " +
                           $"IDNguoiQuanLy = '{comboboxQuanLy.SelectedValue}' WHERE IDMBTM = {txtIDMBTM.Text}";
                    break;
            }
            return sSQL;
        }

        private bool AllowSave()
        {
            if (string.IsNullOrEmpty(txtTenDonViThue.Text.Trim()))
            {
                MessageBox.Show("Tên đơn vị thuê không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtDienTich.Text.Trim()) || string.IsNullOrEmpty(txtGiaThue.Text.Trim()))
            {
                MessageBox.Show("Diện tích và giá thuê không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }

        private void dtview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

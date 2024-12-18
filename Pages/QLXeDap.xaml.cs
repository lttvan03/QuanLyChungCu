﻿using Microsoft.EntityFrameworkCore.Update;
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
    /// Interaction logic for QLXeDap.xaml
    /// </summary>
    public partial class QLXeDap : Page
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
            Sua = 2,
            HienThi = 3
        }
        public QLXeDap() {
            InitializeComponent();
            Load();
        }
        private void Load() {
            currentUserID = GetCurrentUserID();
            currentUserQH = GetCurrentUserQH();
            soCanHo = GetCurrentSoCanHo();
            LoadStatus();
            LoadDataGrid();
            LoadComboBoxCuDan();
            LoadComboBoxQuanLy();
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
            if(currentUserQH == "Cư dân") {
                string sSQL = $"SELECT * FROM XeDap INNER JOIN CuDan ON XeDap.IDCuDan = CuDan.IDCuDan INNER JOIN NguoiQuanLy ON XeDap.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy WHERE CuDan.SoCanHo = '{soCanHo}'";
                dGrid = Connect.DataTransport(sSQL);
                btnThem.Visibility = Visibility.Collapsed;
                btnSua.Visibility = Visibility.Collapsed;
                btnXoa.Visibility = Visibility.Collapsed;
                dtview.ItemsSource = dGrid.DefaultView;
            }
            else if(currentUserQH == "Admin"|| currentUserQH == "Quản lý") {
                dGrid = Connect.DataTransport("SELECT * FROM XeDap INNER JOIN CuDan ON XeDap.IDCuDan = CuDan.IDCuDan INNER JOIN NguoiQuanLy ON XeDap.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");
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
                    btnLuu.Visibility = Visibility.Visible;

                    txtIDXeDap.Text = "";
                    txtLoaiXe.Text = "";
                    txtMauXe.Text = "";
                    comboboxCuDan.SelectedValue = "";
                    txtCuDan.Text = "";
                    comboboxQuanLy.SelectedValue = "";
                    txtQuanLy.Text = "";

                    break;
                case TrangThaiHienTai.Sua:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    btnLuu.Visibility = Visibility.Visible;

                    txtIDXeDap.Text = row["IDXeDap"].ToString();
                    txtLoaiXe.Text = row["LoaiXe"].ToString();
                    txtMauXe.Text = row["MauXe"].ToString();
                    comboboxCuDan.SelectedValue = row["IDCuDan"].ToString();
                    txtCuDan.Text = row["TenCuDan"].ToString();
                    comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

                    break;
                case TrangThaiHienTai.HienThi:
                    popup.IsOpen = true;
                    overlayGrid.Visibility = Visibility.Visible;
                    overlayGrid.Opacity = 0.5;
                    btnLuu.Visibility = Visibility.Collapsed;

                    txtIDXeDap.Text = row["IDXeDap"].ToString();
                    txtLoaiXe.Text = row["LoaiXe"].ToString();
                    txtMauXe.Text = row["MauXe"].ToString();
                    comboboxCuDan.SelectedValue = row["IDCuDan"].ToString();
                    txtCuDan.Text = row["TenCuDan"].ToString();
                    comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                    txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

                    break;
            }
        }
        private void btnTimKiem_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtTimKiem.Text)) {
                string searchText = txtTimKiem.Text.ToLower().Trim();
                string sSQL = "";
                if(currentUserQH == "Cư dân") {
                    sSQL = $"SELECT * FROM XeDap " +
                       $"INNER JOIN CuDan ON XeDap.IDCuDan = CuDan.IDCuDan " +
                       $"INNER JOIN NguoiQuanLy ON XeDap.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                       $"WHERE (LOWER(IDXeDap) LIKE LOWER('%{searchText}%') " +
                       $"OR LOWER(LoaiXe) LIKE LOWER(N'%{searchText}%') OR LOWER(MauXe) LIKE LOWER(N'%{searchText}%') " +
                       $"OR LOWER(CuDan.TenCuDan) LIKE LOWER(N'%{searchText}%') OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                       $"OR LOWER(XeDap.IDCuDan) LIKE LOWER('%{searchText}%') " +
                       $"OR LOWER(XeDap.IDNguoiQuanLy) LIKE LOWER('%{searchText}%')) " +
                       $"AND CuDan.SoCanHo = '{soCanHo}'";
                }
                sSQL = $"SELECT * FROM XeDap " +
                    $"INNER JOIN CuDan ON XeDap.IDCuDan = CuDan.IDCuDan " +
                    $"INNER JOIN NguoiQuanLy ON XeDap.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                    $"WHERE LOWER(IDXeDap) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(LoaiXe) LIKE LOWER(N'%{searchText}%') OR LOWER(MauXe) LIKE LOWER(N'%{searchText}%') " +
                    $"OR LOWER(CuDan.TenCuDan) LIKE LOWER(N'%{searchText}%') OR LOWER(NguoiQuanLy.TenNguoiQuanLy) LIKE LOWER(N'%{searchText}%') " +
                    $"OR LOWER(XeDap.IDCuDan) LIKE LOWER('%{searchText}%') " +
                    $"OR LOWER(XeDap.IDNguoiQuanLy) LIKE LOWER('%{searchText}%')";

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

                txtIDXeDap.Text = row["IDXeDap"].ToString();
                txtLoaiXe.Text = row["LoaiXe"].ToString();
                txtMauXe.Text = row["MauXe"].ToString();
                comboboxCuDan.SelectedValue = row["IDCuDan"].ToString();
                txtCuDan.Text = row["TenCuDan"].ToString();
                comboboxQuanLy.SelectedValue = row["IDNguoiQuanLy"].ToString();
                txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();
            }
        }
        private void comboboxCuDan_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (comboboxCuDan.SelectedItem != null) {
                // Lấy IDCuDan của mục được chọn
                string selectedID = ((DataRowView)comboboxCuDan.SelectedItem)["IDCuDan"].ToString();

                // Lấy tên cư dân tương ứng từ cơ sở dữ liệu (hoặc đã tải sẵn)
                DataTable dt_cudan = Connect.DataTransport($"SELECT TenCuDan FROM CuDan WHERE IDCuDan = '{selectedID}'");

                if (dt_cudan.Rows.Count > 0) {
                    // Cập nhật TextBox với tên cư dân
                    txtCuDan.Text = dt_cudan.Rows[0]["TenCuDan"].ToString();
                }
                else {
                    txtCuDan.Clear();
                }
            }
        }
        private void LoadComboBoxCuDan() {
            // Lấy danh sách ID từ bảng CuDan
            DataTable dt_cudan = Connect.DataTransport("SELECT DISTINCT IDCuDan FROM CuDan");

            if (dt_cudan.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxCuDan.ItemsSource = dt_cudan.DefaultView;
                comboboxCuDan.DisplayMemberPath = "IDCuDan"; // Tên cột hiển thị
                comboboxCuDan.SelectedValuePath = "IDCuDan"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu cư dân!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void comboboxQuanLy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (comboboxQuanLy.SelectedItem != null) {
                string selectedID = ((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"].ToString();

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
            // Lấy danh sách ID từ bảng CuDan
            DataTable dt_quanly = Connect.DataTransport("SELECT DISTINCT IDNguoiQuanLy FROM NguoiQuanLy");

            if (dt_quanly.Rows.Count > 0) {
                // Gán dữ liệu vào ComboBox
                comboboxQuanLy.ItemsSource = dt_quanly.DefaultView;
                comboboxQuanLy.DisplayMemberPath = "IDNguoiQuanLy"; // Tên cột hiển thị
                comboboxQuanLy.SelectedValuePath = "IDNguoiQuanLy"; // Giá trị được chọn
            }
            else {
                MessageBox.Show("Không có dữ liệu người quản lý!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnHuy_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
        }
        private bool AllowSave() {
            popup.IsOpen = false; // Tạm thời ẩn Popup
            overlayGrid.Visibility = Visibility.Collapsed;

            if (txtLoaiXe.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập loại xe.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLoaiXe.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            if (txtMauXe.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập màu xe.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxCuDan.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            if (txtCuDan.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa chọn cư dân.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxCuDan.Focus();
                popup.IsOpen = true;
                overlayGrid.Visibility = Visibility.Visible;
                return false;
            }
            if (txtQuanLy.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa chọn người quản lý.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboboxQuanLy.Focus();
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
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE XeDap SET LoaiXe = N'{txtLoaiXe.Text}', MauXe = N'{txtMauXe.Text}', " +
                        $"IDCuDan = '{((DataRowView)comboboxCuDan.SelectedItem)["IDCuDan"]}'," +
                        $" IDNguoiQuanLy = '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}'" +
                        $" WHERE IDXeDap = {txtIDXeDap.Text}";
                    Connect.DataExecution1(sSQL);
                    _trangThaiHienTai = TrangThaiHienTai.Xem;
                    LoadStatus();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO XeDap(LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy) VALUES( N'{txtLoaiXe.Text}'," +
                        $" N'{txtMauXe.Text}', '{((DataRowView)comboboxCuDan.SelectedItem)["IDCuDan"]}', '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}')";
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
                    string id = selectedRow["IDXeDap"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa xe máy có ID là " + id, "Thông báo",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        sSQL = $"DELETE FROM XeDap WHERE IDXeDap = '{id}'";
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

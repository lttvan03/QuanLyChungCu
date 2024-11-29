﻿using System;
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

namespace QuanLyChungCu.Pages
{
    /// <summary>
    /// Interaction logic for QLoto.xaml
    /// </summary>
    public partial class QLoto : Page
    {
        public QLoto() {
            InitializeComponent();
<<<<<<< Updated upstream
=======
            LoadDataGrid();
            LoadComboBoxCuDan();
            LoadComboBoxQuanLy();
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

        private void LoadDataGrid() {
            dGrid = Connect.DataTransport("SELECT * FROM XeOTo INNER JOIN CuDan ON XeOTo.IDCuDan = CuDan.IDCuDan INNER JOIN NguoiQuanLy ON XeOTo.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy");

            dtview.ItemsSource = null; // Hủy gán ItemsSource cũ
            dtview.Items.Clear(); // Xóa hết các mục hiện tại

            // Gán lại ItemsSource với dữ liệu mới
            dtview.ItemsSource = dGrid.DefaultView;
        }
        private void dtview_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dtview.SelectedItem != null) {
                DataRowView row = (DataRowView)dtview.SelectedItem;

                txtIDoto.Text = row["IDXeOto"].ToString();
                txtBienSoXe.Text = row["BienSoXe"].ToString();
                txtLoaiXe.Text = row["LoaiXe"].ToString();
                txtMauXe.Text = row["MauXe"].ToString();
                comboboxCuDan.SelectedItem = row["IDCuDan"].ToString();
                txtCuDan.Text = row["TenCuDan"].ToString();
                comboboxQuanLy.SelectedItem = row["IDNguoiQuanLy"].ToString();
                txtQuanLy.Text = row["TenNguoiQuanLy"].ToString();

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
        private void comboboxQuanLy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (comboboxCuDan.SelectedItem != null) {
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


        private void dtview_Loaded(object sender, RoutedEventArgs e) {
            DataSet data = new DataSet();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e) {
            if (dtview.SelectedItem != null) {
                _trangThaiHienTai = TrangThaiHienTai.Sua;
                LoadStatus();
            }
            else {
                MessageBox.Show("Vui lòng chọn hàng cần sửa!");
            }
        }
        private void btnThem_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Them;
            LoadStatus();
            ResetDetailView();
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e) {
            _trangThaiHienTai = TrangThaiHienTai.Xem;
            LoadStatus();
            txtQuanLy.Clear();
            txtCuDan.Clear();
            comboboxCuDan.SelectedItem = null;
            comboboxQuanLy.SelectedItem = null;
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(txtBienSoXe.Text) ||
                string.IsNullOrEmpty(txtLoaiXe.Text) ||
                string.IsNullOrEmpty(txtMauXe.Text) ||
                comboboxCuDan.SelectedItem == null ||
                comboboxQuanLy.SelectedItem == null)
            {
                popup.IsOpen = false;
                overlayGrid.Visibility = Visibility.Collapsed;
                overlayGrid.Opacity = 0;
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string sSQL = "";
            switch (_trangThaiHienTai) {
                case TrangThaiHienTai.Them:
                    sSQL = $"INSERT INTO XeOTo(BienSoXe, LoaiXe, MauXe, IDCuDan, IDNguoiQuanLy) VALUES('{txtBienSoXe.Text}', N'{txtLoaiXe.Text}'," +
                        $" N'{txtMauXe.Text}', '{((DataRowView)comboboxCuDan.SelectedItem)["IDCuDan"]}', '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}')";
                    Connect.DataExecution1(sSQL);
                    popup.IsOpen = false;
                    overlayGrid.Opacity = 0;
                    break;
                case TrangThaiHienTai.Sua:
                    sSQL = $"UPDATE XeOTo SET BienSoXe = '{txtBienSoXe.Text}', LoaiXe = N'{txtLoaiXe.Text}', " +
                        $" MauXe =  N'{txtMauXe.Text}', IDCuDan = '{((DataRowView)comboboxCuDan.SelectedItem)["IDCuDan"]}', " +
                        $"IDNguoiQuanLy = '{((DataRowView)comboboxQuanLy.SelectedItem)["IDNguoiQuanLy"]}' WHERE IDXeOTo = {txtIDoto.Text}";
                    Connect.DataExecution1(sSQL);
                    popup.IsOpen = false;
                    overlayGrid.Opacity = 0;
                    break;
            }
            if (_trangThaiHienTai == TrangThaiHienTai.Them || _trangThaiHienTai == TrangThaiHienTai.Sua) {
                if (_trangThaiHienTai == TrangThaiHienTai.Them) {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_trangThaiHienTai == TrangThaiHienTai.Sua) {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                _trangThaiHienTai = TrangThaiHienTai.Xem;
                LoadDataGrid();
                ResetDetailView();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e) {
            if (dtview.SelectedItem != null) {
                string sSQL = "";
                DataRowView selectedRow = dtview.SelectedItem as DataRowView;
                if (selectedRow != null) {
                    string id = selectedRow["IDXeOTo"].ToString();
                    if (MessageBox.Show("Bạn có muốn xóa ô tô có ID là " + id, "Thông báo",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        sSQL = "DELETE FROM XeOTo WHERE IDXeOTo = @IDXeOTo";

                        Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@IDXeOTo", id }
                    };

                        int result = Connect.DataExecution(sSQL, parameters);
                        if (result == 1) {
                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadDataGrid();
                        }
                        else {
                            MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                }
                else {
                    MessageBox.Show("Vui lòng chọn thông tin cần xóa!");
                }

            }
>>>>>>> Stashed changes
        }

        private void ResetDetailView() {
            txtIDoto.Clear();
            txtIDoto.Clear();
            txtBienSoXe.Clear();
            txtLoaiXe.Clear();
            txtMauXe.Clear();
            txtQuanLy.Clear();
            txtCuDan.Clear();
            comboboxCuDan.SelectedItem = null;
            comboboxQuanLy.SelectedItem = null;
        }
    }
}

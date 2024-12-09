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
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private string currentUserPass; 
        private string currentUserID;
        public Profile() {
            InitializeComponent();
            LoadData();
            currentUserPass = GetCurrentUserPass();
            currentUserID = GetCurrentUserID();
        }
        private string GetCurrentUserID() {
            return App.Current.Properties["ID"]?.ToString();
        }

        private string GetCurrentUserPass() {
            return App.Current.Properties["MK"]?.ToString();
        }

        private DataTable dGrid = new DataTable();

        private TrangThaiHienTai _trangThaiHienTai = TrangThaiHienTai.Xem;
        public enum TrangThaiHienTai
        {
            Xem = 0,
            Sua = 1
        }
        private string GetCurrentUserId() {

            return App.Current.Properties["ID"]?.ToString();
        }

        private void LoadData() {
            grMatKhau.Visibility = Visibility.Collapsed;
            btnCapNhat.Visibility = Visibility.Collapsed;
            cbPass.IsChecked = false;

            string currentUserId = GetCurrentUserId();

            string sSQL = "SELECT *, CONCAT(CuDan.TenCuDan, ' ', NguoiQuanLy.TenNguoiQuanLy, ' ', Admin.TenAdmin) AS TenNguoiDung, " +
                              "CONCAT(CuDan.NgaySinh, null, NguoiQuanLy.NgaySinh) AS NgaySinh " +
                              "FROM TaiKhoan " +
                              "LEFT JOIN CuDan ON TaiKhoan.IDCuDan = CuDan.IDCuDan " +
                              "LEFT JOIN NguoiQuanLy ON TaiKhoan.IDNguoiQuanLy = NguoiQuanLy.IDNguoiQuanLy " +
                              "LEFT JOIN Admin ON TaiKhoan.IDAdmin = Admin.IDAdmin " +
                              "WHERE TaiKhoan.IDTaiKhoan = @UserId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@UserId", currentUserId }
            };

            DataTable dGrid = Connect.DataTransport1(sSQL, parameters);

            if (dGrid.Rows.Count > 0) {
                DataRow user = dGrid.Rows[0];

                string avatarPath = user["Avatar"].ToString();

                if (!string.IsNullOrEmpty(avatarPath)) {
                    try {
                        // Kiểm tra nếu là URL
                        if (Uri.TryCreate(avatarPath, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)) {
                            imgAvt.Source = new BitmapImage(uriResult);
                        }
                        else {
                            // Kiểm tra nếu là Relative Path
                            string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, avatarPath);
                            if (System.IO.File.Exists(absolutePath)) {
                                imgAvt.Source = new BitmapImage(new Uri(absolutePath, UriKind.Absolute));
                            }
                            else {
                                // Hiển thị ảnh mặc định nếu file không tồn tại
                                imgAvt.Source = null;
                            }
                        }
                    }
                    catch (UriFormatException) {
                        // Nếu Avatar không hợp lệ, hiển thị ảnh mặc định
                        imgAvt.Source = null;
                    }
                }
                else {
                    // Hiển thị ảnh mặc định nếu Avatar rỗng
                    imgAvt.Source = null;
                }
                // Hiển thị thông tin khác
                txtHoTen.Text = user["TenNguoiDung"].ToString();

                if (user["NgaySinh"] != DBNull.Value && !string.IsNullOrEmpty(user["NgaySinh"].ToString()))
                {
                    txtNgaySinh.SelectedDate = Convert.ToDateTime(user["NgaySinh"]);
                }
                else
                {
                    txtNgaySinh.SelectedDate = null; // Nếu không có ngày sinh, gán null
                }
            }
        }

        private void cbPass_Checked(object sender, RoutedEventArgs e) {
            // Khi CheckBox được tích, hiển thị grMatKhau
            grMatKhau.Visibility = Visibility.Visible;
            btnCapNhat.Visibility = Visibility.Visible;
            txtMatKhau.Password = "";
            txtMatKhauNew.Password = "";
            txtNhapLaiMK.Password = "";

        }

        private void cbPass_Unchecked(object sender, RoutedEventArgs e) {
            // Khi CheckBox không được tích, ẩn grMatKhau
            grMatKhau.Visibility = Visibility.Collapsed;
            btnCapNhat.Visibility = Visibility.Collapsed;

            txtMatKhau.Password = "";
            txtMatKhauNew.Password = "";
            txtNhapLaiMK.Password = ""; 
        }

        private void btnAvatar_Click(object sender, RoutedEventArgs e) {
            // Mở hộp thoại chọn file
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Chọn ảnh đại diện"
            };

            if (openFileDialog.ShowDialog() == true) {
                string selectedFilePath = openFileDialog.FileName;

                try {
                    // Hiển thị ảnh mới
                    imgAvt.Source = new BitmapImage(new Uri(selectedFilePath, UriKind.Absolute));

                    // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    SaveAvatarPathToDatabase(selectedFilePath);
                }
                catch (Exception ex) {
                    MessageBox.Show($"Đã xảy ra lỗi khi cập nhật ảnh đại diện: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SaveAvatarPathToDatabase(string avatarPath) {
            string currentUserId = GetCurrentUserId();

            // Lệnh SQL cập nhật đường dẫn ảnh
            string sSQL = "UPDATE TaiKhoan SET Avatar = @Avatar WHERE IDTaiKhoan = @UserId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Avatar", avatarPath },
                { "@UserId", currentUserId }
            };

            int rowsAffected = Connect.DataExecution(sSQL, parameters);

            if (rowsAffected > 0) {
                MessageBox.Show("Cập nhật ảnh đại diện thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("Không thể cập nhật ảnh đại diện. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e) {
            // Kiểm tra mật khẩu cũ
            if (string.IsNullOrWhiteSpace(txtMatKhau.Password)) {
                MessageBox.Show("Bạn chưa nhập mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (txtMatKhau.Password != currentUserPass) {
                MessageBox.Show("Nhập sai mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Kiểm tra mật khẩu mới
            if (string.IsNullOrWhiteSpace(txtMatKhauNew.Password)) {
                MessageBox.Show("Bạn chưa nhập mật khẩu mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Kiểm tra nhập lại mật khẩu mới
            if (string.IsNullOrWhiteSpace(txtNhapLaiMK.Password)) {
                MessageBox.Show("Bạn chưa nhập lại mật khẩu mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (txtMatKhauNew.Password != txtNhapLaiMK.Password) {
                MessageBox.Show("Mật khẩu mới chưa khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string sSQL = $"UPDATE TaiKhoan SET MatKhau = '{txtMatKhauNew.Password}' WHERE IDTaiKhoan = '{currentUserID}' ";
            Connect.DataExecution1(sSQL);
            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txtMatKhau.Password = "";
            txtMatKhauNew.Password = "";
            txtNhapLaiMK.Password = "";

            LoadData();
        }
    }
}

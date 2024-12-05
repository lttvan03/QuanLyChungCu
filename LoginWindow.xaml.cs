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
using System.Windows.Shapes;

namespace QuanLyChungCu
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        private AppDbContext dbContext = new AppDbContext();
        private AuthService authService;

        public LoginWindow() {
            InitializeComponent();
            authService = new AuthService(dbContext);
        }

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }
        public void btnLogin_Click(object sender, RoutedEventArgs e) {
            if (!AllowLogin()) {
                return;
            }
            var user = authService.Login(txtUserName.Text, txtPassword.Password);

            if (user != null) {
                if(txtUserName.Text.Trim() == user.IDTaiKhoan) {
                    if (txtPassword.Password.Trim() == user.MatKhau) {
                        Application.Current.Properties["UserRole"] = user.QuyenHan;
                        Application.Current.Properties["ID"] = user.IDTaiKhoan;
                        Application.Current.Properties["MK"] = user.MatKhau;
                        MainWindow maindWin = new MainWindow();
                        maindWin.Show();
                        this.Hide();

                        maindWin.Logout += mainWindow_Logout;
                        return;
                    }
                    else {
                        MessageBox.Show("Mật khẩu bạn nhập không chính xác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }                
            } else {
                MessageBox.Show("Tài khoản bạn nhập không chính xác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    private void mainWindow_Logout(object sender, EventArgs e) {
        (sender as MainWindow).isExit = false;
        this.Show();  // Hiển thị lại cửa sổ đăng nhập
        (sender as MainWindow).Close();  // Đóng cửa sổ MainWindow
    }
    private bool AllowLogin() {
        if (txtUserName.Text.Trim() == "") {
            MessageBox.Show("Bạn chưa nhập Tài khoản", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtUserName.Focus();
            return false; // Nếu chưa nhập thông tin thì thoát tại đây
        }
        if (txtPassword.Password.Trim() == "") {
            MessageBox.Show("Bạn chưa nhập Mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            txtPassword.Focus();  // Sửa lại là txtPassword.Focus() thay vì txtUserName.Focus()
            return false; // Nếu chưa nhập thông tin thì thoát tại đây
        }
        return true;
    }
}
}

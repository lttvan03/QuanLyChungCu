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
    public partial class LoginWindow : Window
    {
        public LoginWindow() {
            InitializeComponent();
        }


        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            //Trước khi đâng nhập thì  kiểm tra xem người dùng có nhập đủ các thông tin cần thiết chưa.
            if (!AllowLogin()) { //Nếu hàm này trả về là false thì thoát ngày, không thực hiện chức năng bên dưới
                return;
            }

            // Bắt đầu lấy dữ liệu trong bảng UserLogin để so sánh dữ liệu người dùng nhập để thông báo có đăng nhập thành công hay không
            DataTable dataTable = ConnectDatabase.Connect.DataTransport("select * from TaiKhoan");
            bool isUserFound = false;
            for (int i = 0; i < dataTable.Rows.Count; i++) {
                // Kiểm tra tài khoản người dùng nhập có trùng khớp với dữ liệu trong CSDL không
                if (txtUserName.Text.Trim().ToLower() == Convert.ToString(dataTable.Rows[i]["IDTaiKhoan"]).ToLower()) {
                    isUserFound = true; // Đã tìm thấy tài khoản

                    // Nếu tài khoản đúng, kiểm tra mật khẩu
                    if (txtPassword.Password.Trim() == Convert.ToString(dataTable.Rows[i]["MatKhau"])) {
                        // Đăng nhập thành công
                        MessageBox.Show("Đăng nhập thành công", "Chúc mừng", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainWindow maindWin = new MainWindow();
                        maindWin.Show();
                        this.Hide();
                        maindWin.Logout += mainWindow_Logout;
                        return; // Thoát khỏi phương thức khi đăng nhập thành công
                    }
                    else {
                        // Mật khẩu không chính xác
                        MessageBox.Show("Mật khẩu bạn nhập không chính xác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Thoát ra ngay nếu mật khẩu không đúng
                    }
                }
            }

            // Nếu không tìm thấy tài khoản trong CSDL
            if (!isUserFound) {
                MessageBox.Show("Tài khoản bạn nhập không chính xác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void mainWindow_Logout(object sender, EventArgs e) {
            (sender as MainWindow).isExit = false;
            (sender as MainWindow).Close();
            this.Show();
        }


        private bool AllowLogin() {
            if (txtUserName.Text.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập Tài khoản", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUserName.Focus();
                return false; //Nếu chưa nhập thông tin thì thoát tại đây
            }
            if (txtPassword.Password.Trim() == "") {
                MessageBox.Show("Bạn chưa nhập Mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUserName.Focus();
                return false; //Nếu chưa nhập thông tin thì thoát tại đây
            }
            return true;
        }
    }
}

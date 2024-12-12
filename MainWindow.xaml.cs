using QuanLyChungCu.CustomControl;
using QuanLyChungCu.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyChungCu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isExit = true;
        public event EventHandler Logout;
        public static MainWindow Instance { get; private set; }
        public MainWindow() {
            InitializeComponent();

            MainWindowFrame.Navigate(new Dashboard());
            Instance = this;

        }
        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            // Gọi sự kiện Logout khi người dùng bấm nút Logout
            Logout?.Invoke(this, EventArgs.Empty);
        }

        //private void Window_Closed(object sender, EventArgs e) {
        //    if (isExit && Application.Current.Windows.Count == 1)
        //        Application.Current.Shutdown();
        //}

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
        //    if (isExit) {
        //        if (MessageBox.Show("Bạn muốn thoát chương trình", "Cảnh báo", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
        //            e.Cancel = true;
        //        }
        //    }
        //}
    }
}
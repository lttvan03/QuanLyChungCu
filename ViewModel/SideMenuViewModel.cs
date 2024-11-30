using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QuanLyChungCu.ViewModel
{
    class SideMenuViewModel
    {       
        ResourceDictionary dict = Application.LoadComponent(new Uri("/QuanLyChungCu;component/Assets/IconDictionary.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
        private void LogoutCommand() {
            try {
                if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                    foreach (Window window in Application.Current.Windows) {
                        if (window is MainWindow) {
                            window.Hide();
                        }
                    }
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public List<MenuItemsData> MenuList {
            get {
                return new List<MenuItemsData>()
                {
                    //MainMenu with out submenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_dashboard"], MenuText = "Trang chủ", NavigateToPage = "Dashboard" , SubMenuList = null},

                    //MainMenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_building"], MenuText = "Khu vực dân cư", NavigateToPage = "KVDanCu",

                        //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_building"], SubMenuText = "Căn hộ", NavigateToPage = "QLCanHo"},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_cudan"], SubMenuText = "Cư dân", NavigateToPage = "QLCuDan"} } },

                    //MainMenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_building"], MenuText = "Khu vực thương mại", NavigateToPage = "KVThuongMai", SubMenuList = null },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_cash"], MenuText = "Tài chính", NavigateToPage = "QLTaiChinh", SubMenuList = null },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_vehicle"], MenuText = "Phương tiện", NavigateToPage = "QLoto", 
                    //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_car"], SubMenuText = "Xe ô tô", NavigateToPage = "QLoto"},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_motobike"], SubMenuText = "Xe máy", NavigateToPage = "QLXeMay"},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_bike"], SubMenuText = "Xe đạp", NavigateToPage = "QLXeDap"} }
                    },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_users"], MenuText = "Tài khoản", NavigateToPage = "QLTaiKhoan",  
                        //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_users"], SubMenuText = "Tất cả tài khoản", NavigateToPage = "QLTaiKhoan"},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_users"], SubMenuText = "Thông tin tài khoản", NavigateToPage = "Profile"} }
                    },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_logout"], MenuText = "Đăng xuất", SubMenuList = null, Command = new CommandViewModel(LogoutCommand) },
                };
            }
        }
    }

    public class MenuItemsData
    {
        //Icon Data
        public PathGeometry PathData { get; set; }
        public string MenuText { get; set; }
        public string NavigateToPage { get; set; }
        public List<SubMenuItemsData> SubMenuList { get; set; }

        //Click event
        public MenuItemsData() {
            Command = new CommandViewModel(Execute);
        }

        public ICommand Command { get; set; }
        private void Execute() {
            string MT = NavigateToPage.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(MT))
                navigateToPage(MT);
        }
        private void navigateToPage(string Menu) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
    public class SubMenuItemsData
    {
        public PathGeometry PathData { get; set; }
        public string SubMenuText { get; set; }
        public string NavigateToPage { get; set; }

        public SubMenuItemsData() {
            SubMenuCommand = new CommandViewModel(Execute);
        }

        public ICommand SubMenuCommand { get; }
        private void Execute() {
            
            //our logic comes here
            string SMT = NavigateToPage.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(SMT))
                navigateToPage(SMT);
            }
      

        private void navigateToPage(string Menu) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}
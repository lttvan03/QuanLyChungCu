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
                    // Xóa quyền người dùng khi đăng xuất
                    Application.Current.Properties["UserRole"] = null;

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
                string userRole = App.Current.Properties["UserRole"]?.ToString();
                if (dict == null) {
                    MessageBox.Show("Lỗi tải tài nguyên biểu tượng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<MenuItemsData>(); // Trả về danh sách rỗng nếu không tải được resource
                }
                var filteredMenuList = new List<MenuItemsData>()
                {
                    //MainMenu with out submenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_dashboard"], MenuText = "Trang chủ", NavigateToPage = "Dashboard" , RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }, SubMenuList = null},

                    //MainMenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_building"], MenuText = "Khu vực dân cư", NavigateToPage = "QLCuDan", RequiredRole = new List<string> { "Cư dân", "Quản lý" , "Admin"},

                        //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_cudan"], SubMenuText = "Cư dân", NavigateToPage = "QLCuDan", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_building"], SubMenuText = "Căn hộ", NavigateToPage = "QLCanHo", RequiredRole = new List<string> { "Quản lý", "Admin" }} } },

                    //MainMenu Button
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_building"], MenuText = "Khu vực thương mại", NavigateToPage = "KVThuongMai", RequiredRole = new List<string> { "Quản lý", "Admin" }, SubMenuList = null },
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_building"], MenuText = "Vật tư", NavigateToPage = "QLVatTu", RequiredRole = new List<string> { "Quản lý", "Admin" }, SubMenuList = null },
                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_cash"], MenuText = "Tài chính", NavigateToPage = "HDCuDan", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" },
                    //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_cashcudan"], SubMenuText = "Hóa đơn cư dân", NavigateToPage = "HDCuDan", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" } },
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_cash"], SubMenuText = "Hóa đơn thương mại", NavigateToPage = "HDThuongMai", RequiredRole = new List<string> {  "Quản lý", "Admin" } } }
                    },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_vehicle"], MenuText = "Phương tiện", NavigateToPage = "QLoto", RequiredRole = new List<string> { "Quản lý", "Cư dân", "Admin" },
                    //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_car"], SubMenuText = "Xe ô tô", NavigateToPage = "QLoto", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }}, 
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_motobike"], SubMenuText = "Xe máy", NavigateToPage = "QLXeMay", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_bike"], SubMenuText = "Xe đạp", NavigateToPage = "QLXeDap", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }} }
                    },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_users"], MenuText = "Tài khoản", NavigateToPage = "Profile", RequiredRole = new List<string> { "Quản lý", "Cư dân", "Admin" },
                        //Submenu button
                        SubMenuList = new List<SubMenuItemsData> {
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_users"], SubMenuText = "Thông tin tài khoản", NavigateToPage = "Profile", RequiredRole = new List<string> { "Cư dân", "Quản lý", "Admin" }},
                            new SubMenuItemsData() { PathData = (PathGeometry)dict["icon_users"], SubMenuText = "Tất cả tài khoản", NavigateToPage = "QLTaiKhoan", RequiredRole = new List<string> { "Quản lý", "Admin" }},
                             }
                    },

                    new MenuItemsData() {PathData = (PathGeometry) dict["icon_logout"], MenuText = "Đăng xuất", SubMenuList = null, Command = new CommandViewModel(LogoutCommand) ,RequiredRole = new List<string> { "Quản lý", "Cư dân", "Admin" }},
                };
                foreach (var menuItem in filteredMenuList) {
                    if (menuItem.SubMenuList != null) {
                        menuItem.SubMenuList = SubMenuItemsData.FilterSubMenus(menuItem.SubMenuList, userRole);
                    }
                }

                return filteredMenuList.Where(menu => menu.RequiredRole.Contains(userRole) || userRole == "Admin").ToList();
            }
        }
    }

    public class MenuItemsData 
    {
        //Icon Data
        public PathGeometry PathData { get; set; }
        public string MenuText { get; set; }
        public string NavigateToPage { get; set; }
        public List<string> RequiredRole { get; set; }
        public List<SubMenuItemsData> SubMenuList { get; set; }

        //Click event
        public MenuItemsData() {
            Command = new CommandViewModel(Execute);
        }

        public ICommand Command { get; set; }
        private void Execute() {
            string userRole = App.Current.Properties["UserRole"]?.ToString();  // Lấy quyền của người dùng

            // Kiểm tra xem quyền người dùng có hợp lệ với menu không
            if (RequiredRole.Contains(userRole) || userRole == "Admin") {
                string MT = NavigateToPage.Replace(" ", string.Empty);
                if (!string.IsNullOrEmpty(MT))
                    navigateToPage(MT);
            }
            else {
                // Nếu quyền không hợp lệ, có thể hiển thị thông báo lỗi hoặc thực hiện hành động khác
                MessageBox.Show("Bạn không có quyền truy cập vào mục này.");
            }
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
        public List<string> RequiredRole { get; set; }
        public string NavigateToPage { get; set; }

        public SubMenuItemsData() {
            SubMenuCommand = new CommandViewModel(Execute);
        }

        public ICommand SubMenuCommand { get; }
        private void Execute() {
            string userRole = App.Current.Properties["UserRole"]?.ToString();  // Lấy quyền của người dùng

            // Kiểm tra xem quyền người dùng có hợp lệ với menu không
            if (RequiredRole.Contains(userRole) || userRole == "Admin") {
                string SMT = NavigateToPage.Replace(" ", string.Empty);
                if (!string.IsNullOrEmpty(SMT))
                    navigateToPage(SMT);
            }
            else {
                // Nếu quyền không hợp lệ, có thể hiển thị thông báo lỗi hoặc thực hiện hành động khác
                MessageBox.Show("Bạn không có quyền truy cập vào mục này.");
            }
        }
        private void navigateToPage(string Menu) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
        public static List<SubMenuItemsData> FilterSubMenus(List<SubMenuItemsData> subMenuList, string userRole) {
            return subMenuList.Where(subMenu => subMenu.RequiredRole.Contains(userRole) || userRole == "Admin").ToList();
        }
    }
}
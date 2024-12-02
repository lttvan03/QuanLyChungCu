using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;


namespace QuanLyChungCu.ConnectDatabase
{
    class Connect
    {
        private static string sConnect = @"Data Source=QUỐCANH;Initial Catalog=QLChungCu;User ID=sa;Password=123;Encrypt=False;TrustServerCertificate=False";
        private static SqlConnection con = null;

        public Connect()
        {
            OpenConnect();
        }

        private static void OpenConnect()
        {
            con = new SqlConnection(sConnect);
            try
            {
                con.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Không thể kết nối đến cơ sở dữ liệu. Chi tiết lỗi: {ex.Message}", "Lỗi kết nối", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        /// <summary>
        /// Hàm dùng để lấy dữ liệu trong SQL ra
        /// </summary>
        /// <param name="sSQL">Là câu lệnh SQL ví dụ (Select * from ...)</param>
        /// <returns></returns>
        public static DataTable DataTransport(string sSQL)
        {
            OpenConnect();
            SqlDataAdapter adapter = new SqlDataAdapter(sSQL, con);
            DataTable dtTable = new DataTable();
            dtTable.Clear();
            adapter.Fill(dtTable);
            return dtTable;
        }

        ///// <summary>
        ///// Lưu dữ liệu xuống Database
        ///// </summary>
        ///// <param name="sSQL">Là câu lệnh SQL là câu lệnh Insert, Update, Dalete></param>
        ///// <returns></returns>
        public static int DataExecution1(string sSQL)
        {
            int iResult = 0;
            OpenConnect();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            iResult = cmd.ExecuteNonQuery();
            return iResult;
        }

        /// <summary>
        /// Lưu dữ liệu xuống Database (Insert, Update, Delete) với tham số
        /// </summary>
        /// <param name="sSQL">Là câu lệnh SQL (Insert, Update, Delete) có tham số</param>
        /// <param name="parameters">Danh sách tham số cần truyền vào</param>
        /// <returns>Trả về số dòng bị ảnh hưởng</returns>
        public static int DataExecution(string sSQL, Dictionary<string, object> parameters)
        {
            int iResult = 0;
            OpenConnect();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;

            // Thêm tham số vào câu lệnh SQL
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            iResult = cmd.ExecuteNonQuery();
            return iResult;
        }
    }
}
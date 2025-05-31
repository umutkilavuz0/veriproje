using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace firinprojesi
{
    internal class Veritabani
    {
        private static string connectionString = @"Data Source=Umut\SQLEXPRESS;Initial Catalog=firinprojesi;Integrated Security=True;TrustServerCertificate=True";
        public static SqlConnection conn = new SqlConnection(connectionString);
        
        public static void BaglantiAc()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı açılırken hata: " + ex.Message);
            }
        }

        public static void BaglantiKapat()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı kapatılırken hata: " + ex.Message);
            }
        }
    }
}
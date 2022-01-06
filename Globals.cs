using System;
using System.Windows.Forms;

namespace AccessSherstnev
{
    internal class Globals
    {
        public static string connectionAdress = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + "../" + "../" + "../" + "../" + "Sherstnev_1.accdb'";

        public static void notification(string message)
        {
            MessageBox.Show(message, "Уведомление - Мой театр");
        }
    }
}

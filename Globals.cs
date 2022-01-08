using System.Data.OracleClient;
using System.Windows.Forms;

namespace AccessSherstnev
{
    internal class Globals
    {
        public static OracleConnection connection = new OracleConnection(connectionString: @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1522))) (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcle)));User Id=sherstnev; Password=1234;");

        public static void notification(string message)
        {
            MessageBox.Show(message, "Уведомление - Мой театр");
        }
    }
}

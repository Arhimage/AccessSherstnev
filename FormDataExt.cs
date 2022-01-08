using System;
using System.Windows.Forms;

using static AccessSherstnev.Globals;
using static AccessSherstnev.OracleData;

namespace AccessSherstnev
{
    public partial class FormDataExt : Form
    {

        DataAccessLight dataActers;
        DataAccessLight dataInfo;
        DataAccessLight dataDate;
        public FormDataExt()
        {
            InitializeComponent();
        }

        public string filterData;

        void getData()
        {
            string query = "SELECT \"ACTORS\".\"SURNAME\", \"ACTORS\".\"NAME\", \"ACTORS\".\"PATRONYMIC\", \"ROLES IN PERFORMANCES\".\"ROLE NAME\" FROM \"ACTORS\" INNER JOIN(\"CONTRACTS\" INNER JOIN (\"REPERTOIRE OF THE THEATER\" INNER JOIN \"ROLES IN PERFORMANCES\" ON \"REPERTOIRE OF THE THEATER\".\"CODE\" = \"ROLES IN PERFORMANCES\".\"PRODUCTION CODE\") ON \"CONTRACTS\".\"CONTRACT CODE\" = \"ROLES IN PERFORMANCES\".\"CONTRACT CODE\") ON \"ACTORS\".\"ACTOR CODE\" = \"CONTRACTS\".\"ACTOR CODE\" WHERE(((\"REPERTOIRE OF THE THEATER\".\"CODE\") = " + filterData + "))";
            string[] dataName = new string[4]
            {
                "SURNAME",
                "NAME",
                "PATRONYMIC",
                "ROLE NAME",
            };
            dataActers = new DataAccessLight(query, dataName, connection, true, false);

            dataActers.getListBox(ref Роли);

            query = "SELECT \"REPERTOIRE OF THE THEATER\".\"DESCRIPTION OF THE PRODUCTION\", \"REPERTOIRE OF THE THEATER\".\"NAME OF THE PRODUCTION\" FROM\"REPERTOIRE OF THE THEATER\" WHERE(\"REPERTOIRE OF THE THEATER\".\"CODE\" = " + filterData + ")";
            dataName = new string[2]
            {
                "DESCRIPTION OF THE PRODUCTION",
                "NAME OF THE PRODUCTION",
            };
            dataInfo = new DataAccessLight(query, dataName, connection, true, false);

            Название.Text = dataInfo.getData()[1][0];
            Описание.Text += dataInfo.getData()[0][0];

            query = "SELECT \"PERFORMANCES\".\"PERFORMANCE DATE\" FROM \"REPERTOIRE OF THE THEATER\" INNER JOIN \"PERFORMANCES\" ON\"REPERTOIRE OF THE THEATER\".\"CODE\" = \"PERFORMANCES\".\"PRODUCTION CODE\" WHERE(((\"REPERTOIRE OF THE THEATER\".\"CODE\") = " + filterData + "))";
            dataName = new string[1]
            {
                "PERFORMANCE DATE",
            };
            dataDate = new DataAccessLight(query, dataName, connection, true, false);

            dataDate.getListBox(ref Сеансы);
        }

        private void FormDataExt_Load(object sender, EventArgs e)
        {
            getData();

            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}

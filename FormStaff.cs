using System;
using System.Windows.Forms;

using static AccessSherstnev.OracleData;
using static AccessSherstnev.Globals;

namespace AccessSherstnev
{
    public partial class FormStaff : Form
    {
        public FormStaff()
        {
            InitializeComponent();
        }

        public string user_id;

        void getData()
        {
            string query = "SELECT \"CONTRACTS\".\"CONTRACT VALUE\", \"CONTRACTS\".\"CONTRACT ATART\", \"CONTRACTS\".\"CONTRACT END\", \"CONTRACTS\".\"NUMBER OF PERFORMANCES\", \"CONTRACTS\".\"AWARD AT THE END\" FROM \"ACTORS\" INNER JOIN \"CONTRACTS\" ON \"ACTORS\".\"ACTOR CODE\" = \"CONTRACTS\".\"ACTOR CODE\" WHERE(((\"ACTORS\".\"ACTOR CODE\") = " + user_id + "))";
            string[] dataName = new string[5]
            {
                "CONTRACT VALUE",
                "CONTRACT ATART",
                "CONTRACT END",
                "NUMBER OF PERFORMANCES",
                "AWARD AT THE END",
            };
            DataAccessLight dataContracts = new DataAccessLight(query, dataName, connection, true, false);

            dataContracts.getDataGrid(ref dataGridView1);

            query = "SELECT \"REGALIA\".\"REGALIA NAME\" FROM \"REGALIA\" INNER JOIN(\"ACTORS\" INNER JOIN \"ACTORS REGALIA\" ON \"ACTORS\".\"ACTOR CODE\" = \"ACTORS REGALIA\".\"ACTOR CODE\") ON \"REGALIA\".\"CODE\" = \"ACTORS REGALIA\".\"REGALIA CODE\" WHERE((\"ACTORS\".\"ACTOR CODE\") = " + user_id + ")";
            dataName = new string[1]
            {
                "REGALIA NAME",
            };
            DataAccessLight dataRegalies = new DataAccessLight(query, dataName, connection, true, false);

            dataRegalies.getListBox(ref listBox1);

            query = "SELECT \"PERFORMANCES\".\"PERFORMANCE DATE\" FROM \"PERFORMANCES\" INNER JOIN(((\"ACTORS\" INNER JOIN \"CONTRACTS\" ON \"ACTORS\".\"ACTOR CODE\" = \"CONTRACTS\".\"ACTOR CODE\") INNER JOIN\"ROLES IN PERFORMANCES\" ON \"CONTRACTS\".\"CONTRACT CODE\" = \"ROLES IN PERFORMANCES\".\"CONTRACT CODE\") INNER JOIN \"LIST OF ROLES\" ON \"ROLES IN PERFORMANCES\".\"CODE\" = \"LIST OF ROLES\".\"ROLE CODE\") ON \"PERFORMANCES\".\"PERFORMANCE CODE\" = \"LIST OF ROLES\".\"PERFORMANCE CODE\" WHERE((\"ACTORS\".\"ACTOR CODE\") = " + user_id + ")";
            dataName = new string[1]
            {
                "PERFORMANCE DATE",
            };
            DataAccessLight dataDates = new DataAccessLight(query, dataName, connection, true, false);

            dataDates.getListBox(ref listBox2);
        }

        private void FormStaff_Load(object sender, EventArgs e)
        {
            getData();

            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }
    }
}

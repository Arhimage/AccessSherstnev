using System;
using System.Windows.Forms;
using System.Data.OracleClient;
using static AccessSherstnev.Enums;
using static AccessSherstnev.Globals;
using System.Data;

namespace AccessSherstnev
{
    public partial class FormChangeAdd : Form
    {
        public FormChangeAdd()
        {
            InitializeComponent();
        }

        public DataType[] dataType;
        public string[] data;
        public OperationType operationType;
        public OracleConnection connectionString;
        public string index;
        public string tableSave;
        public string[] dataName;




        void addNote()
        {
            connectionString.Open();
            string saveData = " " + index + ", ";
            for (int i = 0; i < dataType.Length - 1; i++)
            {
                int j = i * 2 + 1;
                switch (dataType[i + 1])
                {
                    case DataType.STRING:
                        saveData += "'" + table.Controls[j].Text + "'";
                        break;
                    case DataType.DATE:
                        saveData += " TO_DATE('" + DateTime.Parse(table.Controls[j].Text).ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS')";
                        break;
                    case DataType.BOOLEAN:
                        string boolData = "'0'";
                        if (table.Controls[j].ToString() == "System.Windows.Forms.CheckBox, CheckState: 1")
                        {
                            boolData = "'1'";
                        }
                        saveData += " " + boolData + "";
                        break;
                    case DataType.LINK:

                        break;
                    case DataType.NUMBER:
                        if (table.Controls[j].Text == "")
                        {
                            table.Controls[j].Text = "0";
                        }
                        saveData += " '" + table.Controls[j].Text + "'";
                        break;
                    default:
                        break;
                }
                if(i < dataType.Length - 2)
                {
                    saveData += ", ";
                }
            }
            string query = "INSERT INTO \"" + tableSave + "\" VALUES (" + saveData + ")";
            OracleCommand dbcmd = new OracleCommand(query, connectionString);
            dbcmd.CommandType = CommandType.Text;
            OracleDataReader reader = dbcmd.ExecuteReader();
            if (reader.FieldCount != 1)
                notification("Ошибка выполнения запроса!");
            else
                notification("Данные успешно добавлены!");
            reader.Close();
        }

        private void editNote()
        {
            connectionString.Open();
            string saveData = "";
            for (int i = 0; i < dataType.Length - 1; i++)
            {
                int j = i * 2 + 1;
                saveData += "[" + tableSave + "].[" + dataName[i + 1] + "] = ";
                switch (dataType[i + 1])
                {
                    case DataType.STRING:
                        if(table.Controls[j].Text == "")
                        {
                            table.Controls[j].Text = "Неизвестный объект";
                        }
                        saveData += "'" + table.Controls[j].Text + "'";
                        break;
                    case DataType.DATE:
                        saveData += " TO_DATE('" + DateTime.Parse(table.Controls[j].Text).ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS')";
                        break;
                    case DataType.BOOLEAN:
                        string boolData = "'0'";
                        if (table.Controls[j].ToString() == "System.Windows.Forms.CheckBox, CheckState: 1")
                        {
                            boolData = "'1'";
                        }
                        saveData += " " + boolData + "";
                        break;
                    case DataType.LINK:

                        break;
                    case DataType.NUMBER:
                        if (table.Controls[j].Text == "")
                        {
                            table.Controls[j].Text = "0";
                        }
                        saveData += " '" + table.Controls[j].Text + "'";
                        break;
                    default:
                        break;
                }
                if (i < dataType.Length - 2)
                {
                    saveData += ", ";
                }
            }
            string query = "UPDATE \"" + tableSave + "\" SET " + saveData + " WHERE " + "\"" + tableSave + "\".\"" + dataName[0] + "\" = " + index;
            OracleCommand dbcmd = new OracleCommand(query, connectionString);
            dbcmd.CommandType = CommandType.Text;
            OracleDataReader reader = dbcmd.ExecuteReader();
            if (reader.FieldCount != 1)
                notification("Ошибка выполнения запроса!");
            else
                notification("Данные успешно изменены!");
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (operationType)
            {
                case OperationType.ADD:
                    addNote();
                    break;
                case OperationType.EDIT:
                    editNote();
                    break;
                default:

                    break;
            }
            this.Close();
        }

        private void FormChangeAdd_Load(object sender, EventArgs e)
        {
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

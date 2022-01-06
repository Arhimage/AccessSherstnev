using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static AccessSherstnev.AccessData;
using static AccessSherstnev.Enums;
using static AccessSherstnev.Globals;

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
        public string connectionAdress;
        public string index;
        public string tableSave;
        public string[] dataName;




        void addNote()
        {
            OleDbConnection dbConnection = new OleDbConnection(connectionAdress);//создаем соеденение
            dbConnection.Open();//открываем соеденение
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
                        saveData += "#" + DateTime.Parse(table.Controls[j].Text).ToString("dd-MM-yyyy") + "#";
                        break;
                    case DataType.BOOLEAN:
                        string boolData = "False";
                        if (table.Controls[j].ToString() == "System.Windows.Forms.CheckBox, CheckState: 1")
                        {
                            boolData = "True";
                        }
                        saveData += " " + boolData + "";
                        break;
                    case DataType.LINK:

                        break;
                    case DataType.NUMBER:
                        saveData += " " + table.Controls[j].Text + "";
                        break;
                    default:
                        break;
                }
                if(i < dataType.Length - 2)
                {
                    saveData += ", ";
                }
            }
            string query = "INSERT INTO [" + tableSave + "] VALUES (" + saveData + ")";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if (dbCommand.ExecuteNonQuery() != 1)
                notification("Ошибка выполнения запроса!");
            else
                notification("Данные успешно добавлены!");
            dbConnection.Close();
        }

        private void editNote()
        {
            OleDbConnection dbConnection = new OleDbConnection(connectionAdress);//создаем соеденение
            dbConnection.Open();//открываем соеденение
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
                        saveData += "#" + DateTime.Parse(table.Controls[j].Text).ToString("yyyy-MM-dd") + "#";
                        break;
                    case DataType.BOOLEAN:
                        string boolData = "False";
                        if (table.Controls[j].ToString() == "System.Windows.Forms.CheckBox, CheckState: 1")
                        {
                            boolData = "True";
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
                        saveData += " " + table.Controls[j].Text + "";
                        break;
                    default:
                        break;
                }
                if (i < dataType.Length - 2)
                {
                    saveData += ", ";
                }
            }
            string query = "UPDATE [" + tableSave + "] SET " + saveData + " WHERE " + "[" + tableSave + "].[" + dataName[0] + "] = " + index;
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if (dbCommand.ExecuteNonQuery() != 1)
                notification("Ошибка выполнения запроса!");
            else
                notification("Данные успешно изменены!");
            dbConnection.Close();
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

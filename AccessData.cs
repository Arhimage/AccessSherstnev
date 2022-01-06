using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static AccessSherstnev.Enums;
using static AccessSherstnev.Globals;

namespace AccessSherstnev
{
    internal class AccessData
    {

        public class DataAccessLight
        {
            List<List<string>> data = new List<List<string>>(); //Хранение информации
            string[] dataName; //Название поля
            string connectionAdress; //Адрес таблицы Access
            OleDbConnection dbConnection;
            bool notification; //Включение уведомлений об ошибке
            string query;

            public DataAccessLight(string query, string[] dataName, string connectionAdress, bool notification)
            {
                this.query = query;
                this.notification = notification;
                this.connectionAdress = connectionAdress;
                this.dataName = dataName;
                for (int i = 0; i < dataName.Length; i++)
                {
                    this.data.Add(new List<string>());
                }
                get();
            }

            public void get()
            {
                dbConnection = new OleDbConnection(this.connectionAdress);
                dbConnection.Open();
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                if (dbReader.HasRows == false && this.notification)
                    notification("Данные не найдены!");
                else
                {
                    foreach (List<string> item in data)
                    {
                        item.Clear();
                    }
                    while (dbReader.Read())
                    {
                        for (int i = 0; i < this.data.Count; i++)
                        {
                            this.data[i].Add(dbReader[dataName[i]].ToString());
                        }
                    }
                }
                dbReader.Close();
                dbConnection.Close();
                return;
            }
        }

        public class DataAccess : DataAccessLight
        {

            DataType[] dataType; //Типы полей
            string[] dataName; //Название поля
            List<List<string>> data = new List<List<string>>(); //Хранение информации
            string table; //таблица соединения
            string connectionAdress; //Адрес таблицы Access
            //Пример: "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + "../" + "../" + "../" + "../" + "Sherstnev_1.accdb'"
            bool notification; //Включение уведомлений об ошибке
            OleDbConnection dbConnection;


            public DataAccess(DataType[] dataType, string[] dataName, string table, string connectionAdress, bool notification = false) : base("", dataName, connectionAdress, notification)
            {
                this.dataType = dataType;
                this.dataName = dataName;
                this.table = table;
                this.connectionAdress = connectionAdress;
                this.notification = notification;
                for (int i = 0; i < dataName.Length; i++)
                {
                    this.data.Add(new List<string>());
                }
                get();
            }


            public DataGridView getDataGrid(ref DataGridView dataGridView)
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                for (int i = 0; i < this.data.Count; i++)
                {
                    dataGridView.Columns.Add("column_" + i.ToString(), dataName[i]);
                    for (int j = 0; j < this.data[i].Count; j++)
                    {
                        if(i == 0)
                        {
                            dataGridView.Rows.Add();
                        }
                        dataGridView.Rows[j].Cells[i].Value = data[i][j];
                    }
                }

                return dataGridView;
            }

            public void getListBox(ref ListBox listBox)
            {
                listBox.Items.Clear();
                string[] text = new string[this.data[0].Count];
                for (int i = 0; i < this.data.Count; i++)
                {
                    for (int j = 0; j < this.data[i].Count; j++)
                    {
                        text[j] += " " + this.data[i][j].ToString() + " ";
                    }
                }
                foreach (string item in text)
                {
                    listBox.Items.Add(item);
                }
                return;
            }

            public List<List<string>> getData()
            {
                return this.data;
            }

            public void updateData()
            {
                get();
            }

            public string getLastIndex()
            {
                return (int.Parse(this.data[0][this.data[0].Count() - 1]) + 1).ToString();
            }

            private void get()
            {
                dbConnection = new OleDbConnection(this.connectionAdress);
                dbConnection.Open();
                string selectionFields = "";
                for (int i = 0; i < this.dataName.Length; i++)
                {
                    selectionFields += " [" + this.table + "].[" + this.dataName[i] + "]";
                    if (i < this.dataName.Length - 1)
                    {
                        selectionFields += ", ";
                    }
                    else
                    {
                        selectionFields += " ";
                    }
                }
                string query = "SELECT " + selectionFields + " FROM [" + this.table + "]";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                if (dbReader.HasRows == false && this.notification)
                    notification("Данные не найдены!");
                else
                {
                    foreach (List<string> item in data)
                    {
                        item.Clear();
                    }
                    while (dbReader.Read())
                    {
                        for (int i = 0; i < this.data.Count; i++)
                        {
                            this.data[i].Add(dbReader[dataName[i]].ToString());
                        }
                    }
                }
                dbReader.Close();
                dbConnection.Close();
                return;
            }

            public void get(string query)
            {
                dbConnection = new OleDbConnection(this.connectionAdress);
                dbConnection.Open();
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                if (dbReader.HasRows == false && this.notification)
                    notification("Данные не найдены!");
                else
                {
                    foreach (List<string> item in data)
                    {
                        item.Clear();
                    }
                    while (dbReader.Read())
                    {
                        for (int i = 0; i < this.data.Count; i++)
                        {
                            this.data[i].Add(dbReader[dataName[i]].ToString());
                        }
                    }
                }
                dbReader.Close();
                dbConnection.Close();
                return;
            }

            public bool add(string index)
            {
                FormChangeAdd formChangeAdd = new FormChangeAdd();

                for (int i = 1; i < dataName.Length; i++)
                {
                    formChangeAdd.table.RowStyles.Add(new RowStyle());

                    Label label = new Label();
                    label.Text = dataName[i];

                    formChangeAdd.table.Controls.Add(label);

                    switch (dataType[i])
                    {
                        case DataType.STRING:
                            TextBox textBoxString = new TextBox();
                            formChangeAdd.table.Controls.Add(textBoxString);
                            break;
                        case DataType.DATE:
                            DateTimePicker dateTimePicker = new DateTimePicker();
                            formChangeAdd.table.Controls.Add(dateTimePicker);
                            break;
                        case DataType.BOOLEAN:
                            CheckBox checkBox = new CheckBox();
                            formChangeAdd.table.Controls.Add(checkBox);
                            break;
                        case DataType.LINK:

                            break;
                        case DataType.NUMBER:
                            TextBox textBoxNumber = new TextBox();
                            formChangeAdd.table.Controls.Add(textBoxNumber);
                            break;
                        default:
                            break;
                    }

                }

                for (int i = 0; i < formChangeAdd.table.RowStyles.Count - 1; i++)
                {
                    formChangeAdd.table.RowStyles[i].SizeType = SizeType.AutoSize;
                }

                formChangeAdd.table.RowStyles[formChangeAdd.table.RowStyles.Count - 1].SizeType = SizeType.AutoSize;
                formChangeAdd.connectionAdress = this.connectionAdress;
                formChangeAdd.operationType = OperationType.ADD;
                formChangeAdd.dataType = this.dataType;
                formChangeAdd.Название.Text = "Добавление " + table;
                formChangeAdd.Text = "Добавление объекта";
                formChangeAdd.index = index;
                formChangeAdd.tableSave = table;
                formChangeAdd.ShowDialog();
                return true;
            }

            public bool edit(string index)
            {
                FormChangeAdd formChangeAdd = new FormChangeAdd();
                for (int i = 1; i < dataName.Length; i++)
                {
                    formChangeAdd.table.RowStyles.Add(new RowStyle());

                    Label label = new Label();
                    label.Text = dataName[i];

                    formChangeAdd.table.Controls.Add(label);

                    switch (dataType[i])
                    {
                        case DataType.STRING:
                            TextBox textBoxString = new TextBox();
                            textBoxString.Text = data[i][int.Parse(index)];
                            formChangeAdd.table.Controls.Add(textBoxString);
                            break;
                        case DataType.DATE:
                            DateTimePicker dateTimePicker = new DateTimePicker();
                            dateTimePicker.Text = data[i][int.Parse(index)];
                            formChangeAdd.table.Controls.Add(dateTimePicker);
                            break;
                        case DataType.BOOLEAN:
                            CheckBox checkBox = new CheckBox();
                            if(data[i][int.Parse(index)] == "True")
                            {
                                checkBox.Checked = true;
                            }
                            else
                            {
                                checkBox.Checked = false;
                            }
                            formChangeAdd.table.Controls.Add(checkBox);
                            break;
                        case DataType.LINK:

                            break;
                        case DataType.NUMBER:
                            TextBox textBoxNumber = new TextBox();
                            textBoxNumber.Text = data[i][int.Parse(index)];
                            formChangeAdd.table.Controls.Add(textBoxNumber);
                            break;
                        default:
                            break;
                    }

                }

                for (int i = 0; i < formChangeAdd.table.RowStyles.Count - 1; i++)
                {
                    formChangeAdd.table.RowStyles[i].SizeType = SizeType.AutoSize;
                }

                formChangeAdd.table.RowStyles[formChangeAdd.table.RowStyles.Count - 1].SizeType = SizeType.Percent;
                formChangeAdd.table.RowStyles[formChangeAdd.table.RowStyles.Count - 1].Height = 10;

                formChangeAdd.connectionAdress = this.connectionAdress;
                formChangeAdd.operationType = OperationType.EDIT;
                formChangeAdd.dataType = this.dataType;
                formChangeAdd.Название.Text = "Изменение " + table;
                formChangeAdd.Text = "Изменение объекта";
                formChangeAdd.index = data[0][int.Parse(index)];
                formChangeAdd.tableSave = table;
                formChangeAdd.dataName = this.dataName;
                formChangeAdd.ShowDialog();
                return true;
            }

            public bool delete(string index)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM [" + this.table + "] WHERE [" + this.table + "].[" + this.dataName[0] + "] = " + this.data[0][int.Parse(index)] + "; ";
                    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                    if (dbCommand.ExecuteNonQuery() != 1)
                        notification("Ошибка выполнения запроса!");
                    else
                        notification("Данные успешно удалены!");
                    dbConnection.Close();
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
    }
}

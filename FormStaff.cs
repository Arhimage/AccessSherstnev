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

namespace AccessSherstnev
{
    public partial class FormStaff : Form
    {
        public FormStaff()
        {
            InitializeComponent();
        }

        string user_id;

        void getData(string filterData, string typeData)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + "../" + "../" + "../" + "../" + "Sherstnev_1.accdb'";//строка оеденения
            OleDbConnection dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            string query;
            switch (typeData)
            {
                case "contracts":
                    query = "SELECT Контракты.[Стоимость контракта], Контракты.[Начало контракта], Контракты.[Окончание контратка], Контракты.[Количество спектаклей], Контракты.[Премия по окончанию] FROM Актеры INNER JOIN Контракты ON Актеры.[Код актера] = Контракты.[Код актера] WHERE((Актеры.[Код актера]) = " + filterData + ")";
                    break;
                case "regalies":
                    query = "SELECT Регалии.[Название регалии] FROM Регалии INNER JOIN(Актеры INNER JOIN[Регалии актеров] ON Актеры.[Код актера] = [Регалии актеров].[Код актера]) ON Регалии.Код = [Регалии актеров].[Код регалии] WHERE((Актеры.[Код актера]) = " + filterData + ")";
                    break;
                default:
                    query = "SELECT Спектакли.[Дата спектакля] FROM Спектакли INNER JOIN(((Актеры INNER JOIN Контракты ON Актеры.[Код актера] = Контракты.[Код актера]) INNER JOIN[Роли в спектаклях] ON Контракты.[Код контракта] = [Роли в спектаклях].[Код контракта]) INNER JOIN[Список ролей] ON[Роли в спектаклях].Код = [Список ролей].[Код роли]) ON Спектакли.[Код спектакля] = [Список ролей].[Код спектакля] WHERE((Актеры.[Код актера]) = " + filterData + ")";
                    break;
            }
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            if (dbReader.HasRows == false)
                MessageBox.Show("Данные не найдены!", "Ошибка!");
            else
            {
                switch (typeData)
                {
                    case "contracts":
                        dataGridView1.ColumnCount = 5;
                        dataGridView1.Columns[0].HeaderText = "Стоимость";
                        dataGridView1.Columns[1].HeaderText = "Начало действия";
                        dataGridView1.Columns[2].HeaderText = "Окончание действия";
                        dataGridView1.Columns[3].HeaderText = "Количество спектаклей";
                        dataGridView1.Columns[4].HeaderText = "Премия";
                        while (dbReader.Read())
                        {
                            dataGridView1.Rows.Add(dbReader["Стоимость контракта"], DateTime.Parse(Convert.ToString(dbReader["Начало контракта"])).ToString("dd:MM:yyyy"), DateTime.Parse(Convert.ToString(dbReader["Окончание контратка"])).ToString("dd:MM:yyyy"), dbReader["Количество спектаклей"], dbReader["Премия по окончанию"]);
    
                        }
                        break;
                    case "regalies":
                        while (dbReader.Read())
                        {
                            listBox1.Items.Add(dbReader["Название регалии"]);
                        }

                        break;
                    default:
                        while (dbReader.Read())
                        {
                            listBox2.Items.Add(dbReader["Дата спектакля"]);
                        }
                        break;
                }
            }
            dbReader.Close();
            dbConnection.Close();
        }

        private void FormStaff_Load(object sender, EventArgs e)
        {
            user_id = this.Text;
            getData(user_id, "contracts");
            getData(user_id, "regalies");
            getData(user_id, "raspisanie");
            this.Text = "Панель сотрудника - Мой театр";
        }
    }
}

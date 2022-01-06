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
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        DataAccess dataContracts;
        DataAccess dataActors;
        DataAccess dataRegalies;
        DataAccess dataRepertuar;
        DataAccess dataPerformance;

        DataAccess[] dataAccess;
        ListBox[] dataListBox;

        void show(object sender, EventArgs e)
        {
            this.Show();
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;
            /*update();*/
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void initialisation()
        {
            //Инициализация таблицы контрактов
            DataType[] dataTypes = new DataType[7]
{
                DataType.NUMBER,
                DataType.NUMBER,
                DataType.NUMBER,
                DataType.DATE,
                DataType.DATE,
                DataType.NUMBER,
                DataType.NUMBER,
};

            string[] dataNames = new string[7]
            {
                "Код контракта",
                "Код актера",
                "Стоимость контракта",
                "Начало контракта",
                "Окончание контракта",
                "Количество спектаклей",
                "Премия по окончанию",
            };

            string table = "Контракты";

            dataContracts = new DataAccess(dataTypes, dataNames, table, connectionAdress, true);

            dataContracts.getListBox(ref Контракты);

            //Инициализация таблицы актеров

            dataTypes = new DataType[6]
            {
                DataType.NUMBER,
                DataType.STRING,
                DataType.STRING,
                DataType.STRING,
                DataType.NUMBER,
                DataType.BOOLEAN,
            };

            dataNames = new string[6]
            {
                "Код актера",
                "Фамилия",
                "Имя",
                "Отчество",
                "Стаж",
                "Управляющий",
            };

            table = "Актеры";

            dataActors = new DataAccess(dataTypes, dataNames, table, connectionAdress, true);

            dataActors.getListBox(ref Актеры);

            //Инициализация таблицы регалий

            dataTypes = new DataType[2]
            {
                DataType.NUMBER,
                DataType.STRING,
            };

            dataNames = new string[2]
            {
                "Код",
                "Название регалии",
            };

            table = "Регалии";

            dataRegalies = new DataAccess(dataTypes, dataNames, table, connectionAdress, true);

            dataRegalies.getListBox(ref Регалии);

            //Инициализация таблицы репертуара театра

            dataTypes = new DataType[6]
            {
                DataType.NUMBER,
                DataType.STRING,
                DataType.DATE,
                DataType.DATE,
                DataType.STRING,
                DataType.STRING,
            };

            dataNames = new string[6]
            {
                "Код",
                "Название постановки",
                "Начало показа",
                "Завершение показа",
                "Описание постановки",
                "Ссылка на картинку",
            };

            table = "Репертуар театра";

            dataRepertuar = new DataAccess(dataTypes, dataNames, table, connectionAdress, true);

            dataRepertuar.getListBox(ref Репертуар);

            //Инициализация таблицы спектаклей

            dataTypes = new DataType[4]
            {
                DataType.NUMBER,
                DataType.NUMBER,
                DataType.DATE,
                DataType.NUMBER,
            };

            dataNames = new string[4]
            {
                "Код спектакля",
                "Код постановки",
                "Дата спектакля",
                "Бюджет",
            };

            table = "Спектакли";

            dataPerformance = new DataAccess(dataTypes, dataNames, table, connectionAdress, true);

            dataPerformance.getListBox(ref Спектакли);


            dataAccess = new DataAccess[5]
            {
                dataActors,
                dataContracts,
                dataRegalies,
                dataRepertuar,
                dataPerformance,
            };

            dataListBox = new ListBox[5]
            {
                Актеры,
                Контракты,
                Регалии,
                Репертуар,
                Спектакли,
            };
        }

        void addData(DataAccess dataAccess)
        {
            string index = dataAccess.getLastIndex();
            if (dataAccess.add(index))
            {
                update();
            }
        }

        void editData(DataAccess dataAccess, ListBox listBox)
        {
            if (listBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выделите элемент для редактирования!", "Система театра");
                return;
            }
            string index = listBox.SelectedIndex.ToString();
            dataAccess.edit(index);
            update();
        }

        void deleteData(DataAccess dataAccess, ListBox listBox)
        {
            if (listBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выделите элемент для удаления!", "Система театра");
                return;
            }
            string index = listBox.SelectedIndex.ToString();
            if (dataAccess.delete(index))
            {
                update();
            }
        }

        void update()
        {
            for (int i = 0; i < dataAccess.Length; i++)
            {
                dataAccess[i].updateData();
                dataAccess[i].getListBox(ref dataListBox[i]);
            }
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            initialisation();
            update();
            var screen = Screen.FromControl(this);
            this.Top = screen.Bounds.Height / 2 - this.Height / 2;
            this.Left = screen.Bounds.Width / 2 - this.Width / 2;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string [] dataName = new string [3]
            {
                "Дата спектакля",
                "Бюджет",
                "Название постановки",
            };

            string query = "SELECT Спектакли.[Дата спектакля], Спектакли.Бюджет, [Репертуар театра].[Название постановки] FROM[Репертуар театра] INNER JOIN Спектакли ON[Репертуар театра].Код = Спектакли.[Код постановки] WHERE(((Спектакли.[Дата спектакля]) Between #" + DateTime.Parse(ДатаС.Text).ToString("dd-MM-yyyy") + "# And #" + DateTime.Parse(ДатаПо.Text).ToString("dd-MM-yyyy") + "#))";

            DataAccess dataAccess = new DataAccess(query, dataName, connectionAdress);

            dataAccess.getDataGrid(ref dataGridView1);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            addData(dataActors);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            editData(dataActors, Актеры);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            deleteData(dataActors, Актеры);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            addData(dataContracts);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            editData(dataContracts, Контракты);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            deleteData(dataContracts, Контракты);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            addData(dataRegalies);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            editData(dataRegalies, Регалии);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            deleteData(dataRegalies, Регалии);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            addData(dataRepertuar);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            editData(dataRepertuar, Репертуар);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            deleteData(dataRegalies, Репертуар);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            addData(dataPerformance);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            editData(dataPerformance, Спектакли);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            deleteData(dataPerformance, Спектакли);
        }
    }
}

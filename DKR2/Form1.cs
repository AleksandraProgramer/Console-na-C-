using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace DKR2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Informations(object sender, EventArgs e)
        {
            int chek = -1;
            //Проверяем выбран ли предмет
            chek = comboBox1.SelectedIndex;

            if (chek >= 0) { 
            // Сведения
            Chelovek class_Chelovek = new Chelovek();

            label5.Text = class_Chelovek.fio;
            label6.Text = class_Chelovek.data_rojd;
            label7.Text = class_Chelovek.status[0] + " - " + class_Chelovek.status[1] + " - " + class_Chelovek.status[2];

            // Выбор предметов(расчёт средней оценки)
            Stud class_Stud = new Stud();
            label9.Text = class_Stud.ch_sessia(comboBox1.SelectedIndex);

            // Расчёт нагрузки
            Prepod class_Prepod = new Prepod();
            label12.Text = class_Prepod.sum_nagruzka(comboBox1.SelectedIndex);
        }else{
                MessageBox.Show("Выберите предмет!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dataBlock = null;
            string[] dannye = null;
            string[] green_col = null;
            string[] green_cur = null;
            ListViewItem item = null;

              try
              {
            // Получение сведений(сортировка)
            using (FileStream fstream = File.OpenRead(@"my.bd"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                // получаем файл в массив
                string[] words = textFromFile.Split(';');

                //------------- Таблица -------------------------
          
                //запрешаем пользователю самому добавлять строки
                dataGridView1.AllowUserToAddRows = false;

                for (int x = 0; x < (words.Length - 1); ++x)
                {
                    // получаем строку из файла до знака (;)
                    dataBlock = words[x];
                    // получаем из строки сведения по знаку (,)
                    dannye = dataBlock.Split(',');

                    //Добавляем строку, указывая значения каждой ячейки по имени (можно использовать индекс 0, 1, 2 вместо имен)
                    dataGridView1.Rows.Add();
                    dataGridView1["fio", dataGridView1.Rows.Count - 1].Value = dannye[0];
                    dataGridView1["dr", dataGridView1.Rows.Count - 1].Value = dannye[1];
                    dataGridView1["status", dataGridView1.Rows.Count - 1].Value = dannye[2];
                    dataGridView1["osenka", dataGridView1.Rows.Count - 1].Value = dannye[3];

                    // выводим младше 25 лет
                    DateTime curDate = DateTime.Now;
                    green_cur = curDate.ToShortDateString().Split('.');
                    green_col = dannye[1].Split('.');

                        int gcur = int.Parse(green_cur[2]);
                        int gcol = int.Parse(green_col[2]);

                        int calc = (gcur - gcol);
                    if (calc < 25)
                        {
                            dataGridView1["dr", x].Style.BackColor = Color.Green;
                        }

                    }


            }
        } 
                 catch(Exception)
              {
                 MessageBox.Show("У нас ошибка!");
                 Application.Exit();
             }
          }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                    dataGridView1.Sort(dataGridView1.Columns["fio"], ListSortDirection.Ascending); 
                }
            catch (ArgumentNullException)
            {
                MessageBox.Show("У нас ошибка сортировки");
            }
        }
    }
}

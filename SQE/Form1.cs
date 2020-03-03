using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }
        string originText = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                try
                {   // Открываем файл
                    using (StreamReader sr = new StreamReader(OPF.FileName))
                    {
                        //Читаем в строку
                        originText = sr.ReadToEnd();                        
                        AnalyzeText(originText);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AnalyzeText(string source) 
        {
            string[] newLineSeparator = new string[] { "\n" };
            string[] result;
            int[,] dateAndTime = new int[7,24];
            int timeParsed;

            DayOfWeek[] days = new DayOfWeek[7];

            result = source.Split(newLineSeparator, StringSplitOptions.None);
            RequestFrequency rf = new RequestFrequency();

            foreach (string s in result)
            {
                if (s.Length > 0)
                {
                    string date = s.Substring(0, s.IndexOf(','));
                    DayOfWeek tmpDayOfWeek = rf.GetDay(date); //------------------------------
                    //if (tmpDayOfWeek == "")
                    //{
                    //    return;
                    //}
                    rf.IncreaseDayFreq(tmpDayOfWeek);
                    string time = s.Substring(s.IndexOf(',')+2, 2);
                    if (int.TryParse(time, out timeParsed))
                    {
                        int dayNumber = Convert.ToInt32(tmpDayOfWeek);
                        dateAndTime[dayNumber,timeParsed]++;
                        rf.freqInDay[timeParsed]++;
                    }
                    else
                    {
                        MessageBox.Show("Неизвестный формат времени.");
                    }
                }
            }

            string finaldat = "\t\t6 часов\t7 часов\t 8 часов" +
                "\t 9 часов\t 10 часов\t 11 часов\t 12 часов\t 13 часов\t 14 часов\t 15 часов\t 16 часов" +
                "\t 17 часов\t 18 часов\t 19 часов\n";
            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 1:
                        finaldat += "Понедельник\t";
                        break;
                    case 2:
                        finaldat += "Вторник\t\t";
                        break;
                    case 3:
                        finaldat += "Среда\t\t";
                        break;
                    case 4:
                        finaldat += "Четверг\t\t";
                        break;
                    case 5:
                        finaldat += "Пятница\t\t";
                        break;
                    case 6:
                        finaldat += "Суббота\t\t";
                        break;
                    case 0:
                        finaldat += "Воскресенье\t";
                        break;
                    default:
                        MessageBox.Show("Упс... Неизвестный день.");
                        break;
                }
                for (int j = 6; j < 20; j++)
                {
                    finaldat += dateAndTime[i, j] + "\t ";
                }
                finaldat += "\n";

            }

            richTextBox1.Text = finaldat + "\nЧастота запросов в понедельник: \t"  + rf.freqInWeek[1] + "\n" +
                                "Частота запросов во вторник: \t"     + rf.freqInWeek[2] + "\n" +
                                "Частота запросов в среду: \t\t"      + rf.freqInWeek[3] + "\n" +
                                "Частота запросов в четверг: \t"      + rf.freqInWeek[4] + "\n" +
                                "Частота запросов в пятницу: \t"      + rf.freqInWeek[5] + "\n" +
                                "Частота запросов в субботу: \t"      + rf.freqInWeek[6] + "\n" +
                                "Частота запросов в воскресенье: \t"  + rf.freqInWeek[0] + "\n\n";
            string freqTimeStr = "";
            for (int i = 6; i < 24; i++)
            {
                freqTimeStr += "Частота запросов по времени в " + i + " часов: \t" + rf.freqInDay[i] + "\n";
            }

            richTextBox1.Text += freqTimeStr;


        }

        class RequestFrequency
        {   
            //Дата запроса                    
            DateTime dt; //преобразуем в DateTime
            readonly string formatDay = "dd.MM.yyyy";

            public int[] freqInWeek = new int[7];
            public int[] freqInDay = new int[24];

            CultureInfo provider = CultureInfo.CurrentCulture;

            public DayOfWeek GetDay(string dts)
            {
                dt = GetDateFromString(dts);
                return dt.DayOfWeek;
            }

            private DateTime GetDateFromString(string dts) {
                try
                {
                    dt = DateTime.ParseExact(dts, formatDay, provider);                    
                }
                catch (FormatException)
                {
                    MessageBox.Show("Неизвестный формат даты.");
                }
                return dt;
            }



            public void IncreaseDayFreq(DayOfWeek dw) {
                switch (dw.ToString()) 
                {
                    case "Monday":
                        freqInWeek[1]++;
                    break;
                    case "Tuesday":
                        freqInWeek[2]++;
                    break;
                    case "Wednesday":
                        freqInWeek[3]++;
                    break;
                    case "Thursday":
                        freqInWeek[4]++;
                    break;
                    case "Friday":
                        freqInWeek[5]++;
                    break;
                    case "Saturday":
                        freqInWeek[6]++;
                    break;
                    case "Sunday":
                        freqInWeek[0]++;
                    break;
                    default:
                        MessageBox.Show("Упс... Неизвестный день.");
                    break;
                }


            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}

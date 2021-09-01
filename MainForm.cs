using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SuperProject
{
    /// <summary>
    /// Главное окно
    /// </summary>
    public partial class MainForm : Form
    {
        SitemapNews sitemap = new SitemapNews();
        bool isAlive = false;
        
        /// <summary>
        /// Инициализация окна
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// При нажатии клавиши Запонить базу данных 
        /// проверяется была ли раньше она запонена, если нет, то запускается фоновый поток,
        /// в котором будет происходить выполнение программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbFiller_Click(object sender, EventArgs e)
        {
            
            if (!sitemap.isCompleted())
            {
                Thread work = new Thread(sitemap.SitemapPagesFind);
                work.Start();
            }
            else
            {
                Thread work = new Thread(sitemap.SpectationFill);
                work.Start();
            }
            isAlive = true;
            
        }
        /// <summary>
        /// При нажатии кнопки стоп происходит остановка заполнения базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, EventArgs e)
        {
            if (isAlive)
            {
                sitemap.Stop();
                isAlive = false;
            }
        }


        /// <summary>
        /// В текстовое поле вводится время в минутах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Устанавливает таймер на заданное в текстовом поле время
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSet_Click(object sender, EventArgs e)
        {
            string time = textBox1.Text;
            if (time == "")
                time = "0";
            if (Convert.ToInt32(time) != 0)
            {
                timer.Interval = Convert.ToInt32(time) * 60000;
                timer.Start();
            }
        }
        /// <summary>
        /// По истечении времени работы таймера происходит автоматическое нажатие кнопки 
        /// "Заполнить базу данных"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            dbFiller_Click(sender, e);
            timer.Stop();
           
        }
        /// <summary>
        /// При нажатии крестик окно закрывается
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            sitemap.Stop();
            timer.Stop();
        }
    }

}

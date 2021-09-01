using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;



namespace SuperProject
{
    
    class SitemapNews 
    {
       
        private bool stopped = false;
        private bool completed = false;
       // public string reference = "";
        private DataBaseConnector dbaseConnector = new DataBaseConnector();
        
        /// <summary>
        /// Останавливает работу в <see cref="SitemapPagesFind"/>, если запущена
        /// </summary>
        public void Stop()
        {
            stopped = true;
        }
        /// <summary>
        /// Проверка на окончание работы
        /// </summary>
        /// <returns>Возращает true если функция SitemapPagesFind завершила работу</returns>
        public bool isCompleted() { return completed; }



        /// <summary>
        /// Основная функция в которой собираются ссылки, 
        /// в которой запускается функция <see cref="M:articleParse"/>
        /// </summary>
        public void SitemapPagesFind()
        {
            string sitemapUrl = "https://rb.ru/sitemap.xml";

            XmlDocument sitemapDoc = new XmlDocument();
            sitemapDoc.Load(sitemapUrl);
            dbaseConnector.openConnection();
            var sitemapLst = sitemapDoc.GetElementsByTagName("loc");
            List<string> pages = new List<string>();

            foreach (XmlNode node in sitemapLst)
            {
                if (node.InnerText.Contains("https://rb.ru/sitemap-news.xml"))
                    pages.Add(node.InnerText);
            }


            XmlDocument sitemapNewsDoc;
            

            
            for (int i = pages.Count - 1; i >= 0 & !stopped; i--)
            {

                if (stopped)
                    break;

                sitemapNewsDoc = new XmlDocument();
                sitemapNewsDoc.Load(pages[i].ToString());
                var links = sitemapNewsDoc.GetElementsByTagName("loc");
                

                for (int j = links.Count - 1; j >= 0 & !stopped; j--)
                {

                    if (stopped)
                        break;

                    if (dbaseConnector.inDB(links[j].InnerText))
                        Scrapper.articleParse(links[j].InnerText, dbaseConnector);
                    
                }
                

            }

           
            if(!stopped)
            {
                completed = true;
                dbaseConnector.closeConnection();
                MessageBox.Show("Заполнение базы данных завершено",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly
                    );
            }

            stopped = false;

        }

        /// <summary>
        /// Добавление статей в режиме наблюдателя
        /// </summary>
        public void SpectationFill()
        {
            dbaseConnector.openConnection();



            string sitemapUrl = "https://rb.ru/sitemap-news.xml";

            XmlDocument sitemapNewsDoc = new XmlDocument();
            sitemapNewsDoc.Load(sitemapUrl);

            var links = sitemapNewsDoc.GetElementsByTagName("loc");

            for (int j = links.Count - 1; j >= 0; j--)
            {
                if (stopped)
                    break;

                if (dbaseConnector.inDB(links[j].InnerText))
                    Scrapper.articleParse(links[j].InnerText, dbaseConnector);
               
            }

            if (!stopped)
            {
                dbaseConnector.closeConnection();
                MessageBox.Show("Заполнение базы данных завершено",
                   "Сообщение",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.DefaultDesktopOnly
                   );
            }


            stopped = false;
        }











    }
}

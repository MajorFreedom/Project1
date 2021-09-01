using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;



namespace SuperProject
{
    class Scrapper 
    {



       /// <summary>
       /// В данной функции происходит разбор статьи,
       /// 
       /// </summary>
       /// <param name="url"> - ссылка на статью</param>
       /// <param name="connector"> - подключение к базе данных </param>
        public static void articleParse(string url, DataBaseConnector connector)
        {

            HtmlWeb wb = new HtmlWeb();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc = wb.Load(url);
            



            //Тэги
            var tags = htmlDoc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("article-header__item-tags tags__item")).ToList();

            
            List<string> tagText = new List<string>();
            foreach(var node in tags)
            {
                tagText.Add(node.InnerText.Trim('\n','\r','\t'));
            }

            // Автор
            var author = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("article-header__author-section")).ToList();
            //Дата 
            var date = htmlDoc.DocumentNode.Descendants("time")
              .Where(node => node.GetAttributeValue("class", "")
              .Equals("article-header__date")).ToList();
           
            // Заголовок
            var header = htmlDoc.DocumentNode.Descendants("h1")
               .Where(node => node.GetAttributeValue("class", "")
               .Equals("article-header__rubric-title")).ToList();
            // Вступление
            var Introduction = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("article__introduction")).ToList();
           
            // Сама статья
            var Arcticle = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("article__content-block abv")).ToList();

            List<string> info = new List<string>();
            
            //1
            info.Add(url);
            //2
            if (author.Count != 0)
            {               
                info.Add(author.LastOrDefault().InnerText.Trim('\n', '\t', '\r'));
            }
            else info.Add(" ");
            //3
            if (date.Count != 0)
            {               
                info.Add(date.LastOrDefault().InnerText.Trim('\n', '\t', '\r'));
            }
            else info.Add(" ");
            string ts=" ";
            //4
            if (tagText.Count!=0)
            {
                foreach(var tag in tagText)
                {
                    ts += tag + " ";
                }
            }
            info.Add(ts);
            //5
            if (header.Count != 0)
            {              
                info.Add(header.LastOrDefault().InnerText.Trim('\n', '\t', '\r'));
            }
            else info.Add(" ");
            //6
            if (Introduction.Count != 0)
            {
                info.Add(Introduction.LastOrDefault().InnerText.Trim('\n', '\t', '\r'));
            }
            else info.Add(" ");
            //7
            if (Arcticle.Count != 0)
            {           
                info.Add(Arcticle.LastOrDefault().InnerText.Trim('\n', '\t', '\r'));
            }
            else info.Add(" ");

           
            
            
            connector.Insert(info);

           
    
        }

    }
}

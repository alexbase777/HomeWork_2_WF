
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Configuration;
using System.Security.AccessControl;

namespace HomeWork_2_WF
{
    public partial class Form1 : Form
    {
        string fileName_;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory(); // Путь к fb2-файлу
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Диалоговое окно
            {
                fileName_ = openFileDialog1.FileName; // Путь к файлу             

                // --------------- Первый вариант вывода книги с тегами ----------------
                //StreamReader sr = new StreamReader(fileName_);
                /*string allText = sr.ReadToEnd();                
                allText = allText.Replace("<p>", "");
                allText = allText.Replace("</p>", "");                
                tB1.Text = allText;
                sr.Close();*/

                // ------- Второй вариант вывода книги сплошным текстом-кодом без тегов ----------                                
                /*XElement el = XElement.Load(fileName_);
                tB1.Text = el.Value;*/

                // ------------ Третий вариант вывода книги сплошным текстом без тегов и кода -----
                /*XDocument doc = XDocument.Load(fileName_);
                var namespaceManager = new XmlNamespaceManager(new NameTable());
                namespaceManager.AddNamespace("fb", "http://www.gribuser.ru/xml/fictionbook/2.0");
                var body = doc.Root.XPathSelectElements("fb:body", namespaceManager).ToList();
                foreach (var item in body)
                {
                    tB1.Text = item.Value.ToString();
                }*/

                // ------------ Четвёртый вариант вывода книги с форматированием -----------
                XDocument doc = XDocument.Load(fileName_); // Загружаем книгу fb2 в объект XDocument
                XNamespace fb2NS = "http://www.gribuser.ru/xml/fictionbook/2.0"; // Элемент body у fb2 храниться в неймспейсе, который надо прописать
                // Вывод анотации к книге
                var annotation = doc.Root.Element(fb2NS + "description").Element(fb2NS + "title-info").Element(fb2NS + "annotation");
                if (annotation != null)
                    tB1.Text += "АННОТАЦИЯ:" + Environment.NewLine + Environment.NewLine + annotation.Value.ToString() + Environment.NewLine + Environment.NewLine;
                var body = doc.Root.Element(fb2NS + "body"); // Выбираем элемент body
                var title_book = body.Element(fb2NS + "title"); // В элементе body находим название книги
                var paragraphs_title_book = title_book.Elements(fb2NS + "p"); // Разбиваем название книги по абзацам
                if (paragraphs_title_book != null) // Выводим название книги в textbox
                {
                    foreach (var paragraph in paragraphs_title_book)
                        tB1.Text += "\t\t\t" + paragraph.Value.ToString().ToUpper() + Environment.NewLine;
                    tB1.Text += Environment.NewLine;
                }
                var sections = body.Elements(fb2NS + "section"); // Разбиваем тело книги по главам (тег section)                                
                foreach (var section in sections)
                {
                    var title = section.Element(fb2NS + "title"); // В каждой главе находим заголовок
                    if (title == null) // Если мы нашли не главу
                    {
                        var paragraphs = section.Elements(fb2NS + "p");
                        if (paragraphs != null)
                            foreach (var paragraph in paragraphs)
                                tB1.Text += paragraph.Value.ToString() + Environment.NewLine;
                        tB1.Text += Environment.NewLine;
                    }
                    if (title != null) // Если мы нашли главу с заголовком
                    {
                        var paragraphs_title = title.Elements(fb2NS + "p"); // В каждой главе находим абзатцы
                        if (paragraphs_title != null)  // Выводим в textbox заголовок главы + её содержимое
                        {
                            foreach (var paragraph in paragraphs_title) // Вывод названия главы
                                tB1.Text += "\t\t\t" + paragraph.Value.ToString().ToUpper() + Environment.NewLine;
                            tB1.Text += Environment.NewLine;
                            var paragraphs_section = section.Elements(fb2NS + "p");
                            foreach (var paragraph in paragraphs_section) // Вывод содержимого глава
                                tB1.Text += paragraph.Value.ToString() + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
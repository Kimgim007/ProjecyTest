﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ProjecyTest
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Укажите путь к папке где лежат XML файлы для последующей их обработки");
            string pathInFolder = Console.ReadLine();

            string[] xmlFiles = Directory.GetFiles(pathInFolder, "*.xml");

            XmlDocument xDoc = new XmlDocument();
      
            foreach (string xmlFile in xmlFiles)
            {
                string path = xmlFile;

                xDoc.Load(path);

                XmlNode xmlNode;

                if (xDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    xmlNode = xDoc.LastChild;
                }
                else
                {
                    xmlNode = xDoc.FirstChild;
                }

                if (xmlNode != null)
                {
                    try
                    {
                        FileParserService(xmlNode);

                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    catch
                    {
                        Console.WriteLine("Неверный формат файла!!!");
                    }
                }
            }
        }
        public static async Task FileParserService(XmlNode xmlNode)
        {        
            if (xmlNode.HasChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Имя узла {xmlNode.Name}");
                    
                    if (xmlNode.Attributes.Count != 0)
                    {
                        Console.WriteLine("Атребуты узла:");
                        foreach (XmlAttribute attr in xmlNode.Attributes)
                        {
                            Console.WriteLine($"Имя атребута: {attr.Name} - значение: {attr.Value}");
                        }
                    }
                  
                    if (xmlNode.HasChildNodes)
                    {
                        foreach (XmlNode item in xmlNode.ChildNodes)
                        {
                            await FileParserService(item);
                        }
                    }
                }
            }
            if (xmlNode.NodeType == XmlNodeType.Text)
            {
                if (xmlNode.InnerText.Length > 0)
                {
                    if ((xmlNode.InnerText[0] != '<') && (xmlNode.InnerText[0] != '>'))
                    {
                        Console.WriteLine($"Текстовое значение узла:{xmlNode.InnerText}");
                    }
                    else
                    {
                        // Декодирование HTML-специальных символов в тексте
                        string decodedRapidControlStatus = WebUtility.HtmlDecode(xmlNode.InnerText);

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(decodedRapidControlStatus);

                        foreach (XmlNode item in xmlDocument.DocumentElement)
                        {
                            await FileParserService(item);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}



using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace FilterTuneWPF
{

    /// <summary>
    /// Contains list of FilterTemplates, and path to the file it was received from
    /// </summary>
    [Serializable]
    public static class TemplateManager
    {
        public static List<FilterTemplate> Templates;
        public static string tempFilePath;

        static TemplateManager()
        {
            Templates = new List<FilterTemplate>();
            tempFilePath = "Templates.xml";
        }

        /// <summary>
        /// Deserializes list of FilterTemplates from XML file
        /// </summary>
        public static void GetTemplates()    
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<FilterTemplate>));

            using (FileStream fs = new FileStream(tempFilePath, FileMode.OpenOrCreate))
            {
                Templates = (List<FilterTemplate>)formatter.Deserialize(fs);
            }
        }
        
        /// <summary>
        /// Saves Templates list to XML file
        /// </summary>
        public static void SaveTemplates()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<FilterTemplate>));

            using( FileStream fs = new FileStream(tempFilePath, FileMode.Create))
            {
                formatter.Serialize(fs, Templates);
            }

            Console.WriteLine("Serialization complete");
        }

        /// <summary>
        /// Adds FilterTemplate to the list Templates
        /// </summary>
        /// <param name="newTemplate"></param>
        public static void AddTemplate(FilterTemplate newTemplate)
        {
            Templates.Add(newTemplate);
            SaveTemplates();
        }

        /// <summary>
        /// Returns array of names of FilterTemplates from Templates list
        /// </summary>
        /// <returns></returns>

        public static string[] GetNames()
        {
            List<string> names = new List<string>();

            foreach(FilterTemplate temp in Templates)
            {
                names.Add(temp.Name);
            }            

            return names.ToArray();
        }
        

    }
}

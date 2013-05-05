using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Deficit.Extentions
{
    public static class LinqExtention
    {
        static public bool HasElement(this XElement self, string Name)
        {
            foreach (XElement e in self.Elements())
                if (e.Name == Name) return true;
            return false;
        }

        static public KeyValuePair<string, string> GetTextureNameKey(string str)
        {
            string[] array = Regex.Split(str, "::");
            return new KeyValuePair<string, string>(array[0], array[1]);
        }

        static public string GetAttributeValue(this XElement Element, string AttributeName, string DefaultValue = "0")
        {
            XAttribute attr;
            try
            {
                attr = Element.Attribute(AttributeName);
            }
            catch (Exception)
            {
                return DefaultValue;
            }
            if (attr != null)
            {
                return attr.Value;
            }
            return DefaultValue;
        }
    }
}

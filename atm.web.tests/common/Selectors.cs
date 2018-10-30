using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.web.tests.common
{
    public static class Selectors
    {
        /// <summary>
        /// Extension used to find elements by a custom a attribute.
        /// </summary>
        /// <param name="p_strAttributeName">Data Attribute Key</param>
        /// <param name="p_strAttributeValue">Data Attribute Value</param>
        /// <returns></returns>
        public static By SelectorByAttributeValue(string p_strAttributeName, string p_strAttributeValue)
        {
            return (By.XPath(String.Format("//*[@{0} = '{1}']", p_strAttributeName, p_strAttributeValue)));
        }

        /// <summary>
        /// Extension used to find elements by a custom a attribute while specifying what type of html element should be found.
        /// </summary>
        /// <param name="p_strTagName">HTML Tag Name</param>
        /// <param name="p_strAttributeName">Data Attribute Key</param>
        /// <param name="p_strAttributeValue">Data Attribute Value</param>
        /// <returns></returns>
        public static By SelectorByTagAndAttributeValue(string p_strTagName, string p_strAttributeName, string p_strAttributeValue)
        {
            return (By.XPath(String.Format("//{0}[@{1} = '{2}']", p_strTagName, p_strAttributeName, p_strAttributeValue)));
        }
    }
}
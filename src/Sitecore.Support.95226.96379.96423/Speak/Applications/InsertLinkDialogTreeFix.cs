using Sitecore.Speak.Applications;
using Sitecore.Web;
using System;
using System.Xml.Linq;

namespace Sitecore.Support.Speak.Applications
{
    public class InsertLinkDialogTreeFix : InsertLinkDialogTree
    {
        private static string GetXmlAttributeValue(XElement element, string attrName)
        {
            if (element.Attribute(attrName) == null)
            {
                return string.Empty;
            }
            return element.Attribute(attrName).Value;
        }

        public override void Initialize()
        {
            base.Initialize();
            if (WebUtil.GetQueryString("hdl") != string.Empty)
            {
                string str = UrlHandle.Get()["va"];
                if (!string.IsNullOrEmpty(str))
                {
                    XElement element = XElement.Parse(str);
                    if (GetXmlAttributeValue(element, "linktype") == "internal")
                    {
                        base.Target.Parameters["selectedItem"] = GetXmlAttributeValue(element, "target");
                    }
                }
            }
        }
    }
}
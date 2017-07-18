using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell;
using Sitecore.Speak.Applications;
using Sitecore.StringExtensions;
using Sitecore.Web;
using Sitecore.Web.PageCodes;
using System;
using System.Net;
using System.Xml.Linq;

namespace Sitecore.Support.Speak.Applications
{
    public class InsertLinkDialogTree : PageCodeBase
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
            string setting = Settings.GetSetting("BucketConfiguration.ItemBucketsEnabled");
            this.ListViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeViewToggleButton.Parameters["IsVisible"] = setting;
            this.TreeView.Parameters["ShowHiddenItems"] = UserOptions.View.ShowHiddenItems.ToString();
            this.TreeView.Parameters["ContentLanguage"] = WebUtil.GetQueryString("la");
            this.ReadQueryParamsAndUpdatePlaceholders();
        }

        private void ReadQueryParamsAndUpdatePlaceholders()
        {
            string queryString = WebUtil.GetQueryString("ro");
            string str2 = WebUtil.GetQueryString("hdl");
            if (!string.IsNullOrEmpty(queryString) && (queryString != "{0}"))
            {
                this.TreeView.Parameters["RootItem"] = queryString;
            }
            this.InsertAnchorButton.Parameters["Click"] = string.Format(this.InsertAnchorButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            this.InsertEmailButton.Parameters["Click"] = string.Format(this.InsertEmailButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            this.ListViewToggleButton.Parameters["Click"] = string.Format(this.ListViewToggleButton.Parameters["Click"], WebUtility.UrlEncode(queryString), WebUtility.UrlEncode(str2));
            string text = string.Empty;
            if (str2 != string.Empty)
            {
                text = UrlHandle.Get()["va"];
            }
            if (text != string.Empty)
            {
                XElement element = XElement.Parse(text);
                if (GetXmlAttributeValue(element, "linktype") == "internal")
                {
                    if (!GetXmlAttributeValue(element, "id").IsNullOrEmpty())
                    {
                        Item item = ClientHost.Databases.ContentDatabase.GetItem(queryString ?? string.Empty) ?? ClientHost.Databases.ContentDatabase.GetRootItem();
                        Item mediaItemFromQueryString = SelectMediaDialog.GetMediaItemFromQueryString(GetXmlAttributeValue(element, "id"));
                        if (((item != null) && (mediaItemFromQueryString != null)) && mediaItemFromQueryString.Paths.LongID.StartsWith(item.Paths.LongID))
                        {
                            this.TreeView.Parameters["PreLoadPath"] = item.ID + mediaItemFromQueryString.Paths.LongID.Substring(item.Paths.LongID.Length);
                        }
                    }
                    this.TextDescription.Parameters["Text"] = GetXmlAttributeValue(element, "text");
                    this.AltText.Parameters["Text"] = GetXmlAttributeValue(element, "title");
                    this.StyleClass.Parameters["Text"] = GetXmlAttributeValue(element, "class");
                    this.QueryString.Parameters["Text"] = GetXmlAttributeValue(element, "querystring");
                }
            }
        }

        public Sitecore.Mvc.Presentation.Rendering AltText { get; set; }

        public Sitecore.Mvc.Presentation.Rendering InsertAnchorButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering InsertEmailButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering ListViewToggleButton { get; set; }

        public Sitecore.Mvc.Presentation.Rendering QueryString { get; set; }

        public Sitecore.Mvc.Presentation.Rendering StyleClass { get; set; }

        public Sitecore.Mvc.Presentation.Rendering Target { get; set; }

        public Sitecore.Mvc.Presentation.Rendering TextDescription { get; set; }

        public Sitecore.Mvc.Presentation.Rendering TreeView { get; set; }

        public Sitecore.Mvc.Presentation.Rendering TreeViewToggleButton { get; set; }
    }
}

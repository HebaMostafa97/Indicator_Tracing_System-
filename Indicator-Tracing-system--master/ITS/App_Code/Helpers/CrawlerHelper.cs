using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ISDCore;

namespace ITS.App_Code
{
    public static class CrawlerHelper
    {
        public static bool CreateXML(int id, string url, string abbreviation)
        {
            try
            {

                //Merging two files
                var crawler = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/XML/CrawlerSetting.xml"));
                var site = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/XML/SiteSettings.xml"));
                crawler.Root.Element("crawlers").Element("crawler").Add(site.Root.Elements());

                //setting crawler settings
                crawler.Root.SetAttributeValue("id", id.ToString());
                crawler.Root.Element("progressDir").SetValue(Settings.ReadAppSettings("ProgressDir") + id.ToString());
                crawler.Root.Element("logsDir").SetValue(Settings.ReadAppSettings("LogsDir") + id.ToString());
                crawler.Root.Element("crawlers").Element("crawler").SetAttributeValue("id", id.ToString());
                crawler.Root.Element("crawlers").Element("crawler").Element("startURLs").Element("url").SetValue(url);
                crawler.Root.Element("crawlers").Element("crawler").Element("workDir").SetValue(Settings.ReadAppSettings("WorkDir") + id.ToString());

                //setting site element value
                crawler.Element("httpcollector").Element("crawlers").Element("crawler").Element("importer").Element("postParseHandlers")
                        .Elements("tagger").Where(x => x.Attribute("class").Value == "com.norconex.importer.handler.tagger.impl.ConstantTagger").FirstOrDefault()
                        .SetElementValue("constant", id.ToString());

                site.Root.Element("importer").Element("postParseHandlers")
                        .Elements("tagger").Where(x => x.Attribute("class").Value == "com.norconex.importer.handler.tagger.impl.ConstantTagger").FirstOrDefault()
                        .SetElementValue("constant", id.ToString());


            // Saving Site settings
                string sitePath = HttpContext.Current.Server.MapPath(string.Format("~/Uploads/SiteSettings/{0}_{1}", abbreviation, id.ToString()));
                site.Save(sitePath);

                string path = HttpContext.Current.Server.MapPath(string.Format("~/Uploads/Indicator/{0}_{1}", abbreviation, id.ToString()));
                crawler.Save(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool CreateXML(int id, string url, string abbreviation, string sitePath)
        {
            try
            {
                //Merging two files
                var crawler = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/XML/CrawlerSetting.xml"));
                var site = XDocument.Load(sitePath);
                crawler.Root.Element("crawlers").Element("crawler").Add(site.Root.Elements());

                //setting crawler settings
                crawler.Root.SetAttributeValue("id", id.ToString());
                crawler.Root.Element("progressDir").SetValue(Settings.ReadAppSettings("ProgressDir") + id.ToString());
                crawler.Root.Element("logsDir").SetValue(Settings.ReadAppSettings("LogsDir") + id.ToString());
                crawler.Root.Element("crawlers").Element("crawler").SetAttributeValue("id", id.ToString());
                crawler.Root.Element("crawlers").Element("crawler").Element("startURLs").Element("url").SetValue(url);
                crawler.Root.Element("crawlers").Element("crawler").Element("workDir").SetValue(Settings.ReadAppSettings("WorkDir") + id.ToString());

                //setting site element value
                crawler.Element("httpcollector").Element("crawlers").Element("crawler").Element("importer").Element("postParseHandlers")
                        .Elements("tagger").Where(x => x.Attribute("class").Value == "com.norconex.importer.handler.tagger.impl.ConstantTagger").FirstOrDefault()
                        .SetElementValue("constant", id.ToString());

                site.Root.Element("importer").Element("postParseHandlers")
                        .Elements("tagger").Where(x => x.Attribute("class").Value == "com.norconex.importer.handler.tagger.impl.ConstantTagger").FirstOrDefault()
                        .SetElementValue("constant", id.ToString());
                

                // Saving Site settings
                site.Save(sitePath);

                string path = HttpContext.Current.Server.MapPath(string.Format("~/Uploads/Indicator/{0}_{1}", abbreviation, id.ToString()));
                crawler.Save(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
}
    }
}
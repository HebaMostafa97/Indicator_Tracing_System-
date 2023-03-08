using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Globalization;

namespace ISDCore
{
    public static class Resources
    {
        public static Dictionary<string, string> MapLabel { get; set; }
        public static void MapLabels(ref ResourceManager resourceManager)
        {
            MapLabel = new Dictionary<string, string>();
            foreach (var resource in resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true))
            {
                var r = (System.Collections.DictionaryEntry)resource;
                MapLabel.Add(r.Key.ToString(), r.Value.ToString());
            }
        }
        public static string AdaptDateForDB(DateTime Value, string Time)
        {
            string Ret = "";
            string _Time = (string.IsNullOrEmpty(Time)) ? "" : " ";
            if (Value != NullHelper.DateTime)
            {
                Ret = string.Format("{1}/{0}/{2}", Value.Day, Value.Month, Value.Year);
                Ret = string.Format("'{0}" + _Time + "{1}'", Ret, Time);
            }
            return Ret;
        }
        public static string AdaptForLike(string Value)
        {
            string Ret = Value.Replace("[", "[[]");
            Ret = Ret.Replace("%", "[%]");
            //Ret = Ret.Replace("_", "[_]");
            Ret = Ret.Replace("'", "''");

            return Ret;
        }
        //Static Function b Return List w mn no3 ListItemObject esmha pagingList()//
        public static List<ListItemObject> pagingList()
        {
          // 3ndy List esmha ListItemObjects mn no3 ListItemObject w return List //
          List<ListItemObject> listItemObjects = new List<ListItemObject>();
          listItemObjects.Add(new ListItemObject { value = "5", Text = "5" });
          listItemObjects.Add(new ListItemObject { value = "10", Text = "10" });
          listItemObjects.Add(new ListItemObject { value = "20", Text = "20" });
          listItemObjects.Add(new ListItemObject { value = "50", Text = "50" });
          listItemObjects.Add(new ListItemObject { value = "100", Text = "100" });
          listItemObjects.Add(new ListItemObject { value = "200", Text = "200" });
          listItemObjects.Add(new ListItemObject { value = "1000", Text = "1000" });
          return listItemObjects;
        }
  }
    // List Item objects // 
    public class ListItemObject
    {
        public string Text { get; set; }
        public string value { get; set; }

    }
    
}

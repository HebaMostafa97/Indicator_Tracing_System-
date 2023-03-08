using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
    [Table("ITS_LKP_Issue")]
    public class Issue :LookupObject
    {
        public Issue()
        {
            Indicators = new HashSet<Indicator>();
            Issues = new HashSet<Issue>();
        }

        public int? ParentID { get; set; }
        


        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }
        public virtual Issue _Issue { set; get; }

        public virtual ICollection<Indicator> Indicators { set; get; }
        public virtual ICollection<Issue> Issues { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
    [Table("ITS_LKP_Periodicity")]
    public class Periodicity : LookupObject
    {
        public Periodicity()
        {
            Indicators = new HashSet<Indicator>();
        }

       


        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }

        public virtual ICollection<Indicator> Indicators { set; get; }
    }
}
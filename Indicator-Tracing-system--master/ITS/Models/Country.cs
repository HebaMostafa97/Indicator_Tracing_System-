using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
    [Table("ITS_LKP_Country")]
    public class Country : LookupObject
    {
        public Country()
        {
            Publishers = new HashSet<Publisher>();
        }
        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }

        public virtual ICollection<Publisher> Publishers { set; get; }
    }
}
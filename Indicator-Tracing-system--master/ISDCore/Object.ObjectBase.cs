using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISDCore
{
    public class ObjectBase
    {
        public int ID { get; set; }
        public int? OwnerID { get; set; }
        public int? ModifierID { get; set; }
        [Column(TypeName = "datetime2"), Index(IsUnique = true)]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ModifyDate { get; set; }
        public int SortIndex { get; set; }
        public int Focus { get; set; }
        public int Active { get; set; }

    }
}

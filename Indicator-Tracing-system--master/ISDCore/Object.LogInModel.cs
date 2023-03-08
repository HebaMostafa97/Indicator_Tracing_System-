using System.ComponentModel.DataAnnotations;

namespace ISDCore
{
    public class LogInModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}

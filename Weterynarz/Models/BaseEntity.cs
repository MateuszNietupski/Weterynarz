using Microsoft.EntityFrameworkCore;

namespace Weterynarz.Models
{
    public class BaseEntity
    {
        [Comment("Data dodania")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Comment("Data aktualizacji")]
        public DateTime DateUpdated { get; set; } 
    }
}

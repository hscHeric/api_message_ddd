using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("TB_MESSAGE")]
    public class Message : Notifies
    {
        [Column("MSG_ID")]
        public int Id { get; set; }

        [Column("MSG_TITLE")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("MSG_ACTIVE")]
        public bool Active { get; set; }

        [Column("MSG_DATE_REGISTER")]
        public DateTime DateRegister { get; set; }

        [Column("MSG_DATE_CHANGE")]
        public DateTime DateChange { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

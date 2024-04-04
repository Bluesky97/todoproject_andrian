using System.ComponentModel.DataAnnotations;

namespace todoproject_andrian.Models
{
    public class ToDoItemModel
    {
        [Key]
        public int TodoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActivitiesNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}

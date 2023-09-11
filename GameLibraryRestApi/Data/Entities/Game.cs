using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameLibraryRestApi.Data.Entities
{
    public class Game
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? Description { get; set; }

        [Required]
        [ForeignKey("FK_Developer")]
        public int Developer { get; set; }
    }
}

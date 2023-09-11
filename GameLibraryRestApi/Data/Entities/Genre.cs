using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameLibraryRestApi.Data.Entities
{
    public class Genre
    {
        [Key]
        [Required]       
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(70)")]
        public string? Name { get; set; } 
    }
}

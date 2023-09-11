using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryRestApi.Data.Entities
{
    [Keyless]
    public class GenreRef
    {
        [Required]
        [ForeignKey("FK_Game")]
        public int GameId { get; set; }

        [Required]
        [ForeignKey("FK_Genre")]
        public int GenreId { get; set; }  
    }
}

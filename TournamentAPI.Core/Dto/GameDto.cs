using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class GameDto
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name must have between 4 and 30 characters.")]
        public string Title { get; set; }
        public DateTime StartDatum { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {
        [Required]
        [StringLength(14, MinimumLength = 4, ErrorMessage = "Name must have between 4 and 30 characters.")]
        public string Title { get; set; }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                EndDate = _startDate.AddMonths(3);
            }
        }

        public DateTime EndDate { get; private set; }
    }
}

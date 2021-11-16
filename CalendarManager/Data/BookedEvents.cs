using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Data {

    public class BookedEvents {

        [Key]
        [Required]
        public Guid Id { get; set; }

        [MaxLength(128)]
        [Required]
        public string ShortURL { get; set; }

        [Required]
        public int StartHour { get; set; }

        [Required]
        public int StartMinute { get; set; }

        [Required]
        public int EndHour { get; set; }

        [Required]
        public int EndMinute { get; set; }

        [MaxLength(128)]
        public string Day { get; set; }

        [Required]
        public int Date { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

    }
}

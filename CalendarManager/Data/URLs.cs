using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Data {

    public class URLs {

        [Key]
        [Required]
        [MaxLength(128)]
        public string ShortURL { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public bool EnforcePeriods { get; set; }

        [MaxLength(128)]
        public string PeriodName { get; set; }

        public int Period { get; set; }

    }
}

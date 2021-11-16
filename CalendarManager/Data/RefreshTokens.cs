using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Data {

    public class RefreshTokens {

        [Key]
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        
        [MaxLength(200)]
        public string Token { get; set; }
    }
}

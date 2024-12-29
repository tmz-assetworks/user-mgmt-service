using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Core.Entities
{
    public class SpecificTimeZone
    {
        public int Id { get; set; }
        public string TimeZoneText { get; set; }
        public string Times { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Core.Entities
{
    public partial class EmailTemplate
    {
        public int ID { get; set; }
        public string Subjects { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}

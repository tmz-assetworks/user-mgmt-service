using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace UsersService.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class LocationSchedule
    {

        [DataMember(Name = "id", EmitDefaultValue = false)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DataMember(Name = "day", EmitDefaultValue = false)]
        public string Day { get; set; }

        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public long LocationId { get; set; }



        [DataMember(Name = "startTime", EmitDefaultValue = false)]
        public string StartTime { get; set; }

        [DataMember(Name = "endTime", EmitDefaultValue = false)]
        public string EndTime { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }

        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }

        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }


    }
}
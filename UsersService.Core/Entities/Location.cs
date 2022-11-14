using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Location
    {

        [DataMember(Name = "id", EmitDefaultValue = false)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DataMember(Name = "locationAddressId", EmitDefaultValue = false)]
        public long LocationAddressId { get; set; }
        public virtual LocationAddress LocationAddress { get; set; }

        [DataMember(Name = "locationStatusId", EmitDefaultValue = false)]
        public long LocationStatusId { get; set; }
        public virtual LocationStatus? LocationStatus { get; set; }

        public virtual Department? Department { get; set; }

        [DataMember(Name = "departmentId", EmitDefaultValue = false)]
        public long DepartmentId { get; set; }

        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public string LocationId { get; set; }

        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }

        [DataMember(Name = "alternateMobileNumber", EmitDefaultValue = false)]
        public string AlternateMobileNumber { get; set; }

        [DataMember(Name = "contactPersonName", EmitDefaultValue = false)]
        [StringLength(40, MinimumLength = 2)]
        public string ContactPersonName { get; set; }

        [DataMember(Name = "contactPersonNumber", EmitDefaultValue = false)]
        [StringLength(15, MinimumLength = 10)]
        public string ContactPersonNumber { get; set; }


        [DataMember(Name = "globalTax", EmitDefaultValue = false)]
        public string GlobalTax { get; set; }


        [DataMember(Name = "totalCapacity", EmitDefaultValue = false)]
        public string TotalCapacity { get; set; }

        [DataMember(Name = "utilityService", EmitDefaultValue = false)]
        public string UtilityService { get; set; }


        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }



        /// <summary>
        /// Gets or Sets LocationName
        /// </summary>
        [DataMember(Name = "locationName", EmitDefaultValue = false)]
        [StringLength(40, MinimumLength = 2)]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or Sets TimeZone
        /// </summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        public string FuelProtectType { get; set; }



        public virtual ICollection<LocationSchedule> LocationSchedule { get; set; }
        public virtual ICollection<OperatorUserMapper>? OperatorUserMapper { get; set; }



    }
}

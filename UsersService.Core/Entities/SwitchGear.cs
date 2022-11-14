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
    [DataContract]
    public partial class SwitchGear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "assetId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string AssetId { get; set; }

        [DataMember(Name = "switchGearName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string SwitchGearName { get; set; }

        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string SerialNumber { get; set; }

        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(Name = "statusId", EmitDefaultValue = false)]
        public long StatusId { get; set; }
        public Status Status { get; set; }

        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public long LocationId { get; set; }
        public virtual Location Location { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }

    }
}
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
    public partial class Pad
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets AssetId
        /// </summary>
        [DataMember(Name = "assetId", EmitDefaultValue = false)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or Sets InsertDate
        /// </summary>
        [DataMember(Name = "installationDate", EmitDefaultValue = false)]
        public DateTime InstallationDate { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }



        /// <summary>
        /// Gets or Sets PadName
        /// </summary>
        [DataMember(Name = "padName", EmitDefaultValue = false)]
        public string PadName { get; set; }

        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        public string SerialNumber { get; set; }
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        /// 
        [DataMember(Name = "statusId", EmitDefaultValue = false)]
        public long StatusId { get; set; }
        public Status Status { get; set; }

        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public long LocationId { get; set; }
        public virtual Location Location { get; set; }

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
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }



    }
}

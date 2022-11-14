
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
    public partial class PowerCabinet
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
        /// Gets or Sets BreakerRating
        /// </summary>
        [DataMember(Name = "breakerRating", EmitDefaultValue = false)]
        public double BreakerRating { get; set; }

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
        /// Gets or Sets DcPortQuantityRating
        /// </summary>
        [DataMember(Name = "dcPortQuantityRating", EmitDefaultValue = false)]
        public int DcPortQuantityRating { get; set; }

        /// <summary>
        /// Gets or Sets InstallationDate
        /// </summary>
        [DataMember(Name = "installationDate", EmitDefaultValue = false)]
        public DateTime InstallationDate { get; set; }

        /// <summary>
        /// Gets or Sets MakeId
        /// </summary>
        [DataMember(Name = "makeMasterId", EmitDefaultValue = false)]
        public long MakeMasterId { get; set; }
        public virtual MakeMaster MakeMaster { get; set; }
        /// <summary>
        /// Gets or Sets ModelId
        /// </summary>
        [DataMember(Name = "modelId", EmitDefaultValue = false)]
        public long? ModelId { get; set; }
        public virtual Model Model { get; set; }
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
        /// Gets or Sets PeakCurrent
        /// </summary>
        [DataMember(Name = "peakCurrent", EmitDefaultValue = false)]
        public int PeakCurrent { get; set; }

        /// <summary>
        /// Gets or Sets SerialNumber
        /// </summary>
        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or Sets ServiceVolts
        /// </summary>
        [DataMember(Name = "serviceVolts", EmitDefaultValue = false)]
        public long ServiceVolts { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "statusId", EmitDefaultValue = false)]
        public long StatusId { get; set; }
        public virtual Status Status { get; set; }

        /// <summary>
        /// Gets or Sets WarrantyDuration
        /// </summary>
        [DataMember(Name = "warrantyDuration", EmitDefaultValue = false)]
        public long WarrantyDuration { get; set; }

        /// <summary>
        /// Gets or Sets WarrantyExpiryDate
        /// </summary>
        [DataMember(Name = "warrantyExpiryDate", EmitDefaultValue = false)]
        public DateTime WarrantyExpiryDate { get; set; }

        /// <summary>
        /// Gets or Sets WarrantyStartDate
        /// </summary>
        [DataMember(Name = "warrantyStartDate", EmitDefaultValue = false)]
        public DateTime WarrantyStartDate { get; set; }

        [DataMember(Name = "IsActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        public long LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}

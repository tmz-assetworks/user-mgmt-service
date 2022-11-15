using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UsersService.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Cable
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
        [Column(TypeName = "nvarchar(100)")]
        public string AssetId { get; set; }

        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public long LocationId { get; set; }
        public virtual Location? Location { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets InstallationDate
        /// </summary>
        [DataMember(Name = "installationDate", EmitDefaultValue = false)]
        public DateTime InstallationDate { get; set; }

        /// <summary>
        /// Gets or Sets MakeIdMasterList
        /// </summary>
        //[DataMember(Name="makeIdMasterList", EmitDefaultValue=false)]
        //public List<MakeMaster> MakeIdMasterList { get; set; }
        [DataMember(Name = "makeMaster", EmitDefaultValue = false)]
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
        [Column(TypeName = "nvarchar(100)")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or Sets SerialNumber
        /// </summary>
        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or Sets Status,10-may-2022 change class Status from StatusDto by Ajay Panchal 
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
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


    }
}
 
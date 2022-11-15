
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UsersService.Core.Entities
{
    
    [DataContract]
    public partial class RFIDReader
    {
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
        /// Gets or Sets CardReader
        /// </summary>
        [DataMember(Name = "card_reader", EmitDefaultValue = false)]
        public string CardReader { get; set; }

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
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

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
        /// Gets or Sets NetworkId
        /// </summary>

        // public long NetworkId { get; set; }

        /// <summary>
        /// Gets or Sets NetworkName
        /// </summary>

        //public string NetworkName { get; set; }

        /// <summary>
        /// Gets or Sets SerialNumber
        /// </summary>
        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public long StatusId { get; set; }
        public Status Status { get; set; }

        /// <summary>
        /// Gets or Sets SubNetworkId
        /// </summary>
        // [DataMember(Name = "subNetworkId", EmitDefaultValue = false)]
        //public long SubNetworkId { get; set; }

        /// <summary>
        /// Gets or Sets SubNetworkName
        /// </summary>

        //public string SubNetworkName { get; set; }

        /// <summary>
        /// Gets or Sets WarrantyDuration
        /// </summary>
        [DataMember(Name = "warrantyDuration", EmitDefaultValue = false)]
        public long WarrantyDuration { get; set; }

        /// <summary>
        /// Gets or Sets WarrantyExpiryDate
        /// 
        /// </summary>
        [DataMember(Name = "warrantyExpiryDate", EmitDefaultValue = false)]
        public DateTime WarrantyExpiryDate { get; set; }

        [DataMember(Name = "LocationId", EmitDefaultValue = false)]

        public long LocationId { get; set; }
        public virtual Location Location { get; set; }
        /// <summary>
        /// Gets or Sets WarrantyStartDate
        /// </summary>
        [DataMember(Name = "warrantyStartDate", EmitDefaultValue = false)]
        public DateTime WarrantyStartDate { get; set; }

    }
}

 

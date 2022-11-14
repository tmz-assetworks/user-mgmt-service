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
    public partial class Charger
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets AssetId
        /// </summary>
        [DataMember(Name = "assetId", EmitDefaultValue = false)]
        public string AssetId { get; set; }


        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        //[DataMember(Name = "description", EmitDefaultValue = false)]
        //public string Description { get; set; }

        /// <summary>
        /// Gets or Sets EndPointUrl
        /// </summary>
        [DataMember(Name = "endPointUrl", EmitDefaultValue = false)]
        public string EndPointUrl { get; set; }

        /// <summary>
        /// Gets or Sets FirmwareVersion
        /// </summary>
        [DataMember(Name = "firmwareVersion", EmitDefaultValue = false)]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or Sets HardwareSerialNumber
        /// </summary>
        [DataMember(Name = "hardwareSerialNumber", EmitDefaultValue = false)]
        public string HardwareSerialNumber { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or Sets IsAutomatic
        /// </summary>
        [DataMember(Name = "isAutomatic", EmitDefaultValue = false)]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// Gets or Sets IsDeviceExists
        ///// </summary>
        //[DataMember(Name = "isDeviceExists", EmitDefaultValue = false)]
        //public bool IsDeviceExists { get; set; }

        /// <summary>
        /// Gets or Sets Latitude
        ///// </summary>
        //[DataMember(Name = "latitude", EmitDefaultValue = false)]
        //public double Latitude { get; set; }

        ///// <summary>
        ///// Gets or Sets Longitude
        ///// </summary>
        //[DataMember(Name = "longitude", EmitDefaultValue = false)]
        //public double Longitude { get; set; }

        /// <summary>
        /// Gets or Sets Make
        /// </summary>

        [DataMember(Name = "makeMasterId", EmitDefaultValue = false)]
        public long? MakeMasterId { get; set; }
        public virtual MakeMaster MakeMaster { get; set; }


        /// <summary>
        /// Gets or Sets MeterType
        /// </summary>
        [DataMember(Name = "meterType", EmitDefaultValue = false)]
        public string MeterType { get; set; }

        // / <summary>
        // / Gets or Sets Model
        // / </summary>
        [DataMember(Name = "modelId", EmitDefaultValue = false)]
        public long? ModelId { get; set; }
        public virtual Model Model { get; set; }

        /// <summary>
        /// Gets or Sets MultiplePorts
        /// </summary>
        [DataMember(Name = "multiplePorts", EmitDefaultValue = false)]
        public bool MultiplePorts { get; set; }

        /// <summary>
        /// Gets or Sets NetworkId
        /// </summary>
        // [DataMember(Name = "networkId", EmitDefaultValue = false)]
        // public long NetworkId { get; set; }

        /// <summary>
        /// Gets or Sets NetworkName
        /// </summary>
        // [DataMember(Name = "networkName", EmitDefaultValue = false)]
        // public string NetworkName { get; set; }

        /// <summary>
        /// Gets or Sets PingSchedule
        /// </summary>
        [DataMember(Name = "pingSchedule", EmitDefaultValue = false)]
        public string PingSchedule { get; set; }

        /// <summary>
        /// Gets or Sets PrivateStation
        /// </summary>
        [DataMember(Name = "privateStation", EmitDefaultValue = false)]
        public bool PrivateStation { get; set; }

        /// <summary>
        /// Gets or Sets ReadingSchedule
        /// </summary>
        [DataMember(Name = "readingSchedule", EmitDefaultValue = false)]
        public string ReadingSchedule { get; set; }

        /// <summary>
        /// Gets or Sets SerialNumber
        /// </summary>
        [DataMember(Name = "serialNumber", EmitDefaultValue = false)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or Sets Location
        /// </summary>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }

        /// <summary>
        /// Gets or Sets StationId
        /// </summary>
        //[DataMember(Name = "stationId", EmitDefaultValue = false)]
        //public long StationId { get; set; }

        [DataMember(Name = "chargeBoxId", EmitDefaultValue = false)]
        public string ChargeBoxId { get; set; }

        /// <summary>
        /// Gets or Sets StationName
        /// </summary>
        //[DataMember(Name = "stationName", EmitDefaultValue = false)]
        //public string StationName { get; set; }

        /// <summary>
        /// Gets or Sets SubnetworkId
        /// </summary>
        // [DataMember(Name = "subnetworkId", EmitDefaultValue = false)]
        // public long SubnetworkId { get; set; }

        // /// <summary>
        // /// Gets or Sets SubnetworkName
        // /// </summary>
        // [DataMember(Name = "subnetworkName", EmitDefaultValue = false)]
        // public string SubnetworkName { get; set; }

        //[DataMember(Name = "vendorId", EmitDefaultValue = false)]
        //public long vendorId { get; set; }
        //public Vendor Vendor { get; set; }

        [DataMember(Name = "rFIDReader", EmitDefaultValue = false)]
        public long? RFIDReaderId { get; set; }
        public virtual RFIDReader RFIDReader { get; set; }

        [DataMember(Name = "powerCabinetId", EmitDefaultValue = false)]
        public long? PowerCabinetId { get; set; }
        public virtual PowerCabinet PowerCabinet { get; set; }

        [DataMember(Name = "padId", EmitDefaultValue = false)]
        public long? PadId { get; set; }

        [DataMember(Name = "modemId", EmitDefaultValue = false)]
        public long? ModemId { get; set; }
        public virtual Modem Modem { get; set; }
        public virtual Pad Pad { get; set; }
        [DataMember(Name = "protocolName", EmitDefaultValue = false)]
        public string ProtocolName { get; set; }
        [DataMember(Name = "cableId", EmitDefaultValue = false)]
        public long CableId { get; set; }
        public virtual Cable Cable { get; set; }

        [DataMember(Name = "switchGearId", EmitDefaultValue = false)]
        public long SwitchGearId { get; set; }
        public virtual SwitchGear SwitchGear { get; set; }
        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string? CreatedBy { get; set; }

        [DataMember(Name = "installationDate", EmitDefaultValue = false)]
        public DateTime? InstallationDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime? ModifiedOn { get; set; }
        public virtual ICollection<Port> Ports { get; set; }
        //public virtual ICollection<DispenserStatus> ChargerStatuses { get; set; }
    }
}

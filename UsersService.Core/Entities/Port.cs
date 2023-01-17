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
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [DataMember(Name = "dispenserId", EmitDefaultValue = false)]
        public long DispenserId { get; set; }
        public virtual Charger Dispenser { get; set; }

        public int ConnectorId { get; set; }


        [DataMember(Name = "connectorType", EmitDefaultValue = false)]
        [ForeignKey("Connector")]
        public long ConnectorType { get; set; }
        public virtual Connector Connector { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }


        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }



        [DataMember(Name = "incrementalPower", EmitDefaultValue = false)]
        public string IncrementalPower { get; set; }


        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }


        [DataMember(Name = "maxPower", EmitDefaultValue = false)]
        public string MaxPower { get; set; }


        [DataMember(Name = "minPower", EmitDefaultValue = false)]
        public string MinPower { get; set; }


        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }


        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }


        [DataMember(Name = "plugTypeId", EmitDefaultValue = false)]
        public long PlugTypeId { get; set; }
        public PlugType PlugType { get; set; }


        [DataMember(Name = "portName", EmitDefaultValue = false)]
        public string PortName { get; set; }


        [DataMember(Name = "power", EmitDefaultValue = false)]
        public string Power { get; set; }

    }
}

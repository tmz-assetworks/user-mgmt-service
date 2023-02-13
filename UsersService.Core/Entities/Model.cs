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
    public partial class Model
    {        /// <summary>
             /// Gets or Sets Id
             /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }


        [DataMember(Name = "connectorCount", EmitDefaultValue = false)]
        public long ConnectorCount { get; set; }


        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }


        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }


        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }


        [DataMember(Name = "manufactureId", EmitDefaultValue = false)]
        public long ManufactureId { get; set; }

        [DataMember(Name = "modelName", EmitDefaultValue = false)]
        public string ModelName { get; set; }


        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }


        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }

        [DataMember(Name = "protocolId", EmitDefaultValue = false)]
        public long ProtocolId { get; set; }
        public virtual Protocol Protocol { get; set; }


        [DataMember(Name = "levelId", EmitDefaultValue = false)]
        public long LevelId { get; set; }
        public virtual Level Level { get; set; }
    }
}

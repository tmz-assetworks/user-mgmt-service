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
    public class Protocol
    {
        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public string CreatedBy { get; set; }


        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }


        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }


        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        public bool IsActive { get; set; }


        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        public string ModifiedBy { get; set; }


        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }


        [DataMember(Name = "protocolName", EmitDefaultValue = false)]
        public string ProtocolName { get; set; }

    }
}

 
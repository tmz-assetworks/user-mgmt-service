using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace UsersService.Core.Entities
{
    [DataContract]
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public long Id { get; set; }

        [Required]
        [DataMember(Name = "userName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string userName { get; set; }

        [DataMember(Name = "DOB", EmitDefaultValue = false)]
        public DateTime DOB { get; set; }

        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public long phoneNumber { get; set; }


        [DataMember(Name = "AddressLine1", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "AddressLine2", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string AddressLine2 { get; set; }

        [ForeignKey("CountryID")]
        public virtual Countries Country { get; set; }
        public long? CountryID { get; set; }

        [ForeignKey("StateID")]
       
        public virtual States State { get; set; }
        public long? StateID { get; set; }

        [DataMember(Name = "city", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string city { get; set; }


        [DataMember(Name = "zipCode", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string zipCode { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string createdBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime createdOn { get; set; }

        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string modifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime modifiedOn { get; set; }   

        [DataMember(Name = "isActive", EmitDefaultValue = true)]
        [DefaultValue(true)] 
        public bool isActive { get; set; }
       
        //public virtual IEnumerable<Users>? UsersList { get; set; }
    }

   
}

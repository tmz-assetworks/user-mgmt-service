using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace UsersService.Core.Entities
{
    [DataContract]
    public class Users 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string name { get; set; }
        [Required]
        [DataMember(Name = "EmailId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string EmailId { get; set; }

        [Required]
        [DataMember(Name = "UserPrincipalName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string UserPrincipalName { get; set; }
        
        [Required]
        [DataMember(Name = "ObjectId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]

        public string ObjectId { get; set; }

        [DataMember(Name = "DOB", EmitDefaultValue = false)]
        public DateTime DOB { get; set; }

        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public long PhoneNumber { get; set; }


        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string AddressLine2 { get; set; }

        [ForeignKey("CountryID")]
        public virtual Countries Country { get; set; }
        public long? CountryID { get; set; }

        [ForeignKey("StateID")]

        public virtual States State { get; set; }
        public long? StateID { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; }
        

        [DataMember(Name = "zipCode", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string ZipCode { get; set; }

        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets CreatedOn
        /// </summary>
        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }

        [DataMember(Name = "modifiedBy", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedOn
        /// </summary>
        [DataMember(Name = "modifiedOn", EmitDefaultValue = false)]
        public DateTime ModifiedOn { get; set; }

        //[DataMember(Name = "passwordHash", EmitDefaultValue = false)]
        //[Column(TypeName = "nvarchar(200)")]
        //public string passwordHash { get; set; }

        //[DataMember(Name = "passwordSalt", EmitDefaultValue = false)]
        //[Column(TypeName = "nvarchar(200)")]
        //public string passwordSalt { get; set; }

        //[DataMember(Name = "password", EmitDefaultValue = false)]
        //[Column(TypeName = "nvarchar(200)")]
        //public string password { get; set; }

        [DataMember(Name = "loginFailCount", EmitDefaultValue = false)]
        public long LoginFailCount { get; set; }

        [DataMember(Name = "isLocked", EmitDefaultValue = false)]
        public bool IsLocked { get; set; }

        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customers? Customer { get; set; }
        public long? CustomerID { get; set; }
    }
}

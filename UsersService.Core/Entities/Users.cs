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

        [Required]
        [DataMember(Name = "ObjectId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        public string ObjectId { get; set; }

        [Required]
        [DataMember(Name = "userPrincipalName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string userPrincipalName { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string name { get; set; }

        [Required]
        [DataMember(Name = "EmailId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string EmailId { get; set; }

        [DataMember(Name = "DOB", EmitDefaultValue = false)]
        public DateTime DOB { get; set; }

        [Required]
        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public long PhoneNumber { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customers? Customer { get; set; }
        public long? CustomerID { get; set; }

        [Required]
        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(255)")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(255)")]
        public string AddressLine2 { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }
        public long? CountryID { get; set; }
        [ForeignKey("StateID")]

        public virtual State State { get; set; }
        public long? StateID { get; set; }

        public long? CityID { get; set; }

        [Required]
        [DataMember(Name = "zipCode", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(6)")]
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

        [DataMember(Name = "loginFailCount", EmitDefaultValue = false)]
        public long LoginFailCount { get; set; }

        [DataMember(Name = "isLocked", EmitDefaultValue = false)]
        public bool IsLocked { get; set; }

        [DataMember(Name = "isActive", EmitDefaultValue = false)]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

         public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<OperatorUserMapper> OperatorUserMapper { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
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
        [DataMember(Name = "UserPrincipalName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string UserPrincipalName { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string name { get; set; }

        [Required]
        [DataMember(Name = "EmailId", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string EmailId { get; set; }

        [Required]
        [DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
        public string PhoneNumber { get; set; }

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
        public long StateID { get; set; }
        //public virtual City City { get; set; }
        //public long CityID { get; set; }

        [Required]
        [DataMember(Name = "zipCode", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(9)")]
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
        public string Otp { get; set; }
        public DateTime OtpDateTime { get; set; }
        [Required]
        [DataMember(Name = "CityName", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(200)")]
        public string CityName { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ImagePath { get; set; }

        [DataMember(Name = "notificationEnable", EmitDefaultValue = false)]
        [DefaultValue(true)]
        public bool NotificationEnable { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<OperatorUserMapper> OperatorUserMapper { get; set; }
    }
}

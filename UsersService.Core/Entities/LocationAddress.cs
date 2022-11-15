using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace UsersService.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class LocationAddress
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets AddressLine1
        /// </summary>
        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(255)")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or Sets AddressLine2
        /// </summary>
        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        [Column(TypeName = "nvarchar(255)")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or Sets CityId
        /// </summary>
        //[DataMember(Name = "cityId", EmitDefaultValue = false)]
        //public long CityId { get; set; }

        /// <summary>
        /// Gets or Sets CityName
        /// </summary>
        [DataMember(Name = "cityName", EmitDefaultValue = false)]
        public string CityName { get; set; }

        /// <summary>
        /// Gets or Sets CountryId
        /// </summary>
        [DataMember(Name = "countryId", EmitDefaultValue = false)]
        public long CountryId { get; set; }

        /// <summary>
        /// Gets or Sets CountryName
        /// </summary>
        [DataMember(Name = "countryName", EmitDefaultValue = false)]
        public string CountryName { get; set; }

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
        /// Gets or Sets LandlineNumber
        /// </summary>
        [DataMember(Name = "landlineNumber", EmitDefaultValue = false)]
        public string LandlineNumber { get; set; }

        /// <summary>
        /// Gets or Sets Latitude
        /// </summary>
        [DataMember(Name = "latitude", EmitDefaultValue = false)]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or Sets Longitude
        /// </summary>
        [DataMember(Name = "longitude", EmitDefaultValue = false)]
        public double Longitude { get; set; }

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
        /// Gets or Sets PinCode
        /// </summary>
        [DataMember(Name = "pinCode", EmitDefaultValue = false)]
        [StringLength(10, MinimumLength = 5)]
        public string PinCode { get; set; }

        /// <summary>
        /// Gets or Sets StateId
        /// </summary>
        [DataMember(Name = "stateId", EmitDefaultValue = false)]
        public long StateId { get; set; }

        /// <summary>
        /// Gets or Sets StateName
        /// </summary>
        [DataMember(Name = "stateName", EmitDefaultValue = false)]
        public string StateName { get; set; }

    }
}

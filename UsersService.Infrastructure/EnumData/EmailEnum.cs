using System.ComponentModel.DataAnnotations;

namespace UsersService.Infrastructure.EnumData
{
    public enum EmailEnum
    {
        [Display(Name = "CUSTOMER_REGISTRATION")]
        CUSTOMER_REGISTRATION = 1,
        [Display(Name = "USER_REGISTRATION")]
        USER_REGISTRATION = 2,
        [Display(Name = "FORGATE_PASSWORD")]
        FORGATE_PASSWORD = 3,
        [Display(Name = "FAULTY_CHARGER")]
        FAULTY_CHARGER = 4,
    }
}

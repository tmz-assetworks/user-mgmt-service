using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Commands.Users;
using UsersService.Application.Responses.Customer;
using UsersService.Core.Entities;
using UsersService.Responses.Users;

namespace UsersService.Core.Mapper
{
    public class UsersMappingProfile : Profile
    {
        public UsersMappingProfile()
        {
            CreateMap<Users, UserResponse>().ReverseMap();
            CreateMap<Users, CreateUserCommand>().ReverseMap()
                .ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, o => o.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.CountryID, o => o.MapFrom(s => s.CountryID))
                .ForMember(d => d.DOB, o => o.MapFrom(s => s.DOB))
                .ForMember(d => d.EmailId, o => o.MapFrom(s => s.EmailId))
                .ForMember(d => d.UserPrincipalName, o => o.MapFrom(s => s.UserPrincipalName))
                .ForMember(d => d.ObjectId, o => o.MapFrom(s => s.ObjectId));
            CreateMap<Users, UpdateUserCommand>().ReverseMap();
            CreateMap<Users, DeleteUserCommand>().ReverseMap();
            CreateMap<Customers, CustomerResponse>().ReverseMap();
            CreateMap<Customers, UpdateCustomerCommand>().ReverseMap();
            CreateMap<Customers, DeleteCustomersCommand>().ReverseMap();
            CreateMap<Customers, CreateCustomersCommand>().ReverseMap()
                .ForMember(d => d.AddressLine1, o => o.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, o => o.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.CountryID, o => o.MapFrom(s => s.CountryID))
                .ForMember(d => d.DOB, o => o.MapFrom(s => s.DOB))
                .ForMember(d => d.userName, o => o.MapFrom(s => s.userName));
        }
    }
}

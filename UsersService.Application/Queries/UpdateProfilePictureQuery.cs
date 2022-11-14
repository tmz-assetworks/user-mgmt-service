using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Core.Response;

namespace UsersService.Application.Queries
{
    public class UpdateProfilePictureQuery : IRequest<GetUserProfileResponseDT>
    {
        public string ImagePath { get; set; }
        public UpdateProfilePictureQuery(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}

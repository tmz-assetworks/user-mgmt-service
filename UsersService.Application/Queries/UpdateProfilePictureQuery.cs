using MediatR;
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

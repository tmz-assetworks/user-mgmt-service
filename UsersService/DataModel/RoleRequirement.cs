using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace UsersService.Api.DataModel
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> Role { get; set; }
        public RoleRequirement(IEnumerable<string> roles)
        {
            Role = roles?.ToList() ?? new List<string>();
        }
    }

    public class AuthApplicationRole : AuthorizationHandler<RoleRequirement>
    {
        private readonly DbContext dbContext;

        public AuthApplicationRole(DbContext appDbContext)
        {
            
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            SucceedRequirementIfRolePresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfRolePresentAndValid(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            //Utilities u = new Utilities(dbContext);

            //var roles = u.getUserRole(context.User.Identity.Name);
            //if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            //{
            //                    if (roles != null && requirement.Role.Any(requiredRoleID => roles.Contains(Convert.ToInt32(requiredRoleID)))) //User ...engineer
            //    {

            //        context.Succeed(requirement);
            //    }
            //}
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreAuthentication.JWT.Framework
{
    public class AgeRestrictionHandler : AuthorizationHandler<AgeRestrictionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRestrictionRequirement requirement)
        {
            var dateOfBirthString = context.User.FindFirstValue(ClaimTypes.DateOfBirth);
            if (string.IsNullOrWhiteSpace(dateOfBirthString))
            {
                return Task.CompletedTask;
            }

            var dt = Convert.ToDateTime(dateOfBirthString);
            var calculatedAge = DateTime.Now.Year - dt.Year;
            if (calculatedAge >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

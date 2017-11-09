using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab28Tom.Models
{
    public class MeetsPowerReq : AuthorizationHandler<PowerLevelRequirement>
    {
        private const int minPower = 280;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PowerLevelRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;

            }

            var power = Convert.ToInt32(context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value);


            if (power >= 280)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

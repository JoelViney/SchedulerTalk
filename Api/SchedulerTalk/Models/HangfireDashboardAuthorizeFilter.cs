using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Models
{
    public class HangfireDashboardAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //var httpcontext = context.GetHttpContext();
            //return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}

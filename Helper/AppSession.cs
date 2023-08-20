using Microsoft.Identity.Web;

namespace APIProject.Helper
{
    public class AppSession : IAppSession
    {
        protected IHttpContextAccessor HttpContextAccessor;
        public AppSession(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public string AuthorizationToken
        {
            get
            {
                if (HttpContextAccessor.HttpContext.Request.Headers["Authorization"].Any())
                    return HttpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                return string.Empty;
            }
        }

        public string UserName => HttpContextAccessor?.HttpContext?.User.GetDisplayName();
    }
}

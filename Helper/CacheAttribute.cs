using TalabatBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace APIProject.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute( int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();
            var casheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);
            var cashedResponse=await casheService.GetCasheResponse(casheKey);

            if (!string.IsNullOrEmpty(cashedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cashedResponse,
                    ContentType= "application/json",
                    StatusCode = 200
                };
                context.Result= contentResult;

                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
                await casheService.CasheResponseAsync(casheKey, okObjectResult, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }
        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}

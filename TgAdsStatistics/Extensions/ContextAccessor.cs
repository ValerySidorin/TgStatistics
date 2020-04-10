using Microsoft.AspNetCore.Http;

namespace TgAdsStatistics.Extensions
{
    public class ContextAccessor
    {
        public IHttpContextAccessor httpContextAccessor;

        public ContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Xim.AppApi.Contexts;

namespace Xim.AppApi.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IServiceProvider _serviceProvider;
    
        public BaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected ContextData GetContext()
        {
            var contextService = _serviceProvider.GetService<IContextService>();
            var data = contextService.Get();
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using Xim.AppApi.Constants;

namespace Xim.AppApi.Contexts
{
    /// <summary>
    /// Tương tác với context
    /// </summary>
    public class ApiService : IContextService
    {
        private readonly IServiceProvider _serviceProvider;
        private ContextData _contextData;
        public ApiService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        static readonly object _lockGetObject = new object();
        public ContextData Get()
        {
            if (_contextData != null)
            {
                return _contextData;
            }

            lock (_lockGetObject)
            {
                if (_contextData != null)
                {
                    return _contextData;
                }
                _contextData = BuildContext();
            }

            return _contextData;
        }

        ContextData BuildContext()
        {
            var httpContext = _serviceProvider.GetService<IHttpContextAccessor>();
            if (httpContext?.HttpContext?.User?.Claims?.Count() == 0)
            {
                return null;
            }

            var contextData = new ContextData();
            var claims = httpContext.HttpContext.User.Claims;
            foreach (var item in claims)
            {
                switch (item.Type)
                {
                    case TokenKey.EMAIL:
                        contextData.Email = item.Value;
                        break;
                    case TokenKey.USERID:
                        if (Guid.TryParse(item.Value, out var gtemp))
                        {
                            contextData.UserId = gtemp;
                        }
                        break;
                    case TokenKey.TYPE:
                        if (int.TryParse(item.Value, out var itemp))
                        {
                            contextData.Type = itemp;
                        }
                        break;
                    case TokenKey.LANGUAGE:
                        contextData.Language = item.Value;
                        break;
                    case TokenKey.USERNAME:
                        contextData.Username = item.Value;
                        break;
                }
            }

            var headers = httpContext.HttpContext.Request.Headers;
            //ưu tiên lấy ngôn ngữ từ header
            if (headers.ContainsKey(HeaderKey.LANGUAGE) && !string.IsNullOrEmpty(headers[HeaderKey.LANGUAGE]))
            {
                contextData.Language = headers[HeaderKey.LANGUAGE];
            }
            return contextData;
        }
    }
}

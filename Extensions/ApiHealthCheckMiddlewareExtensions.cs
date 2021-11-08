using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Extensions
{
    public static class ApiHealthCheckMiddlewareExtensions
    {
        public static IHealthChecksBuilder AddApiHealth(this IHealthChecksBuilder healthChecksBuilder,
            string name = "ApiHealth")
        {
            return healthChecksBuilder.AddCheck<ApiHealthCheck>(name);
        }
    }
}

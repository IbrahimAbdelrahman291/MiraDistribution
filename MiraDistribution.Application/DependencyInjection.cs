using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiraDistribution.Application.Common.Behaviors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MiraDistribution.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}

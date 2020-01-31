using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using R5T.Dacia;
using R5T.Stockholm.Default;
using R5T.Tiros;


namespace R5T.Stockholm.Tiros
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="TextStreamSerializer{T}"/> implementation of <see cref="IStreamSerializer{T}"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddTextStreamSerializer<T>(this IServiceCollection services,
            ServiceAction<ITextSerializer<T>> addTextSerializer,
            ServiceAction<IOptions<StreamSerializerOptions<T>>> addStreamSerializerOptions)
        {
            services
                .AddSingleton<IStreamSerializer<T>, TextStreamSerializer<T>>()
                .RunServiceAction(addTextSerializer)
                .RunServiceAction(addStreamSerializerOptions)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="TextStreamSerializer{T}"/> implementation of <see cref="IStreamSerializer{T}"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IStreamSerializer<T>> AddTextStreamSerializerAction<T>(this IServiceCollection services,
            ServiceAction<ITextSerializer<T>> addTextSerializer,
            ServiceAction<IOptions<StreamSerializerOptions<T>>> addStreamSerializerOptions)
        {
            var serviceAction = new ServiceAction<IStreamSerializer<T>>(() => services.AddTextStreamSerializer<T>(
                addTextSerializer,
                addStreamSerializerOptions));
            return serviceAction;
        }
    }
}

using IceVault.Common.ExceptionHandling.Converters;
using IceVault.Presentation.Middleware.ExceptionHandling.Converters;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IceVault.Presentation.Middleware.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly List<IExceptionConverter> _converters = new()
    {
        new BusinessExceptionConverter(),
        new ValidationExceptionConverter(),
        new DefaultExceptionConverter()
    };

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var converter = _converters.FirstOrDefault(el => el.CanConvert(exception)) ?? new DefaultExceptionConverter();

            var error = converter.Convert(exception);
            var json = JsonConvert.SerializeObject(error.Failures, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            
            context.Response.StatusCode = error.StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = json.Length;

            await using var writer = new StreamWriter(context.Response.Body);
            await writer.WriteAsync(json);
        }
    }
}
using System.Net;
using FamilyAccountant.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace FamilyAccountant.Handlers;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);

        context.Response.ContentType = "application/json";

        string details;

        switch (exception)
        {
            case BusinessException businessException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                details = businessException.Message;
                break;
            case InvalidCredentials invalidCredentials:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                details = invalidCredentials.Message;
                break;
            case NotFound notFound:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                details = notFound.Message;
                break;
            case PermissionsException permissionsException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                details = permissionsException.Message;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                details = "An unexpected error has occurred";
                break;
        }
        
        await context.Response.WriteAsJsonAsync(new 
        {
            error = new
            {
                message = "An error occurred while processing your request.",
                details
            }
        }, cancellationToken);

        return true;
    }
}
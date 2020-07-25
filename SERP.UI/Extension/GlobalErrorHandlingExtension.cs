using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;


namespace SERP.UI.Extension
{
    public static class GlobalErrorHandlingExtension
    {
        public static void ConfigureGlobalExpceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature!=null)
                    {
                        await context.Response.WriteAsync(@"<script language='javascript'>alert('The following errors have occurred: \n" + context.Response.StatusCode + " .');</script>");
                    }
                });
            });
        }
    }
}

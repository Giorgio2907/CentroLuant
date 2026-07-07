using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CentroLuant.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var usuario = session.GetString("Usuario");

            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            if (controller == "Account" && (action == "Login" || action == "Logout"))
            {
                base.OnActionExecuting(context);
                return;
            }

            if (string.IsNullOrEmpty(usuario))
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
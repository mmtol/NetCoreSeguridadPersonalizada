using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadPersonalizada.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        //este metodo es el que permitira impedir el acceso
        //a los action/controller
        //el filer se encarga de interceptar peticiones y decidir que hacer
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //el usuario que se ha validado en nuestra app estara dentro de context
            //y en una propiedad llamada "User"
            //cualquier User esta compuesto por 2 caracteristicas
            //1)Identity:el nombre del usuario y si esta activo
            //2)Principal: el rol del usuario
            var user = context.HttpContext.User;
            //el filtro solamente prgeuntara si existe el User
            //solo entra en accion si no existe
            if (!user.Identity.IsAuthenticated)
            {
                //lo llevamos al login si no se ha autentificado
                //a la ruta debemos enviarle un controller y un action
                //tambien podriamos enviarle params si lo desearamos
                RouteValueDictionary ruta = new RouteValueDictionary
                    (
                        new
                        {
                            controller = "Managed",
                            action = "Login"
                        }
                    );
                //devolvemos la peticion al login
                context.Result = new RedirectToRouteResult(ruta);
            }
        }
    }
}

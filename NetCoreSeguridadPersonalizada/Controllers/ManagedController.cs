using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NetCoreSeguridadPersonalizada.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string pass)
        {
            if (name.ToLower() == "admin" && pass == "12345")
            {
                //por medidas de seguridad se genera una cookie cifrada
                //que es para saber si el user se ha validado en este explorador o no
                ClaimsIdentity identity = new ClaimsIdentity
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                    );
                //un claim es info del user 
                Claim claimUserName = new Claim(ClaimTypes.Name, name);
                Claim claimRole = new Claim(ClaimTypes.Role, "Usuario");
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                //creamos un usuario principal con esta identidad
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                //damos de alta al user en el sistema
                await HttpContext.SignInAsync
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Now.AddMinutes(10)
                        }

                    );
                return RedirectToAction("Perfil", "Usuarios");
            }
            else
            {
                ViewData["mensaje"] = "Credenciales incorrectas";
                return View();
            }
        }
    }
}

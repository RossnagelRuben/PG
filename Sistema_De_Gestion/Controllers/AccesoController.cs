using Microsoft.AspNetCore.Mvc;
using Sistema_De_Gestion.Models;
using Sistema_De_Gestion.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace Sistema_De_Gestion.Controllers
{
    public class AccesoController : Controller
    {
        private readonly EstudioJuridicoContext _context;
        public AccesoController(EstudioJuridicoContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            DB_Usuarios_Logica _da_usuario = new DB_Usuarios_Logica();
            var usuario = _da_usuario.UsuarioValido(_usuario.Correo, _usuario.Password);

            if (usuario != null)
            {
                //Hacemos uso de Clain para las cookies

                /*
                 * Cada claim es un fragmento de información sobre el usuario, como pueden ser, nombre de usuario,
                 * correo electrónico, rol, localidad a la que pertenece, etc.
                 * La forma más sencilla de crear un Claim es proporcionando un tipo y un valor en el constructor del Claim.
                 * El valor del claim se representa con un valor string.
                */

                //creamos un array de claims
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Usuario), //usuario.Nombre
                    new Claim("Correo", usuario.Usuario) //usuario.correo
                };

                TableRole rol = await _context.TableRoles.FindAsync(usuario.IdRol);
                string descripcionRol = rol.Descripcion;
                claims.Add(new Claim(ClaimTypes.Role, descripcionRol));
                //creamos un claim identity, y le pasamos nuestra array de claims
                var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //creamos todo el tema de coockies en el logueo, pasamos como claim principal el claim identity
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsidentity));

                //si esta todo OK...
                return RedirectToAction("Index", "Home");
            }
            else { ModelState.AddModelError("", "Por favor ingresar bien los datos"); return View(_usuario); }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}
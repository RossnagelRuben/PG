using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_De_Gestion.Data;
using Sistema_De_Gestion.Models;
namespace Sistema_De_Gestion.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EstudioJuridicoContext _context;
        public UsuariosController(EstudioJuridicoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Master, Admin")]
        public async Task<IActionResult> Index()
        {
            var estudioJuridicoContext = _context.TableUsuarios.Include(t => t.IdCorreoNavigation).Include(t => t.IdRolNavigation).Include(t => t.IdTelefonoNavigation);
            return View(await estudioJuridicoContext.ToListAsync());
        }

        [Authorize(Roles = "Master, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TableUsuarios == null)
            {
                return NotFound();
            }

            var tableUsuario = await _context.TableUsuarios
                .Include(t => t.IdCorreoNavigation)
                .Include(t => t.IdRolNavigation)
                .Include(t => t.IdTelefonoNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (tableUsuario == null)
            {
                return NotFound();
            }

            return View(tableUsuario);
        }

        [Authorize(Roles = "Master, Admin")]
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.TableRoles, "IdRol", "Descripcion");
            return View();
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario2 tableUsuario, string ConfirmPass)
        {
            try
            {
                ViewData["IdRol"] = new SelectList(_context.TableRoles, "IdRol", "Descripcion", tableUsuario.IdRol);
                if (tableUsuario.Pas == ConfirmPass)
                {
                    if (ModelState.IsValid)
                    {
                        TableUsuario USUARIO = new TableUsuario();
                        byte[] token = Encrypt.GenerarToken(); //generamos el token
                        var hashedPassword = Encrypt.HashPassword(Encoding.UTF8.GetBytes(tableUsuario.Pas.ToString()), token); //pasamos el pass y el token 
                        USUARIO.Token = token; //pasamos el token
                        USUARIO.Pass = hashedPassword; //pasamos el password
                        USUARIO.Usuario = tableUsuario.Usuario;
                        //PASAMOS ROLES Y OTROS DATOS
                        USUARIO.IdRol = tableUsuario.IdRol;
                        _context.Add(USUARIO);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(tableUsuario);
                }
                //si ambas contraseñas no coinciden
                else { ModelState.AddModelError("", "Por favor ingresar bien la contraseña"); return View(tableUsuario); }
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [Authorize(Roles = "Master, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TableUsuarios == null)
            {
                return NotFound();
            }

            var tableUsuario = await _context.TableUsuarios.FindAsync(id);
            if (tableUsuario == null)
            {
                return NotFound();
            }

            //PASAMOS LOS DATOS DE TABLEUSUARIO A USUARIO2
            Usuario2 usuario = new Usuario2();
            usuario.IdUsuario = tableUsuario.IdUsuario;
            usuario.Usuario = tableUsuario.Usuario;
            usuario.IdRol = tableUsuario.IdRol;
            ViewData["IdRol"] = new SelectList(_context.TableRoles, "IdRol", "Descripcion", tableUsuario.IdRol);
            return View(usuario);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario2 tableUsuario, string ConfirmPass)
        {
            try
            {
                ViewData["IdRol"] = new SelectList(_context.TableRoles, "IdRol", "Descripcion", tableUsuario.IdRol);
                if (tableUsuario.Pas == ConfirmPass)
                {
                    if (ModelState.IsValid)
                    {
                        TableUsuario USUARIO = new TableUsuario();
                        byte[] token = Encrypt.GenerarToken(); //generamos el token
                        var hashedPassword = Encrypt.HashPassword(Encoding.UTF8.GetBytes(tableUsuario.Pas.ToString()), token); //pasamos el pass y el token 
                        USUARIO.Token = token; //pasamos el token
                        USUARIO.Pass = hashedPassword; //pasamos el password
                        USUARIO.Usuario = tableUsuario.Usuario;
                        //PASAMOS ROLES Y OTROS DATOS
                        USUARIO.IdUsuario = tableUsuario.IdUsuario; //pasamos el id de usuario
                        USUARIO.IdRol = tableUsuario.IdRol;
                        if (ModelState.IsValid)
                        {
                            _context.Update(USUARIO);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        return View(tableUsuario);
                    }
                }
                //si ambas contraseñas no coinciden
                else { ModelState.AddModelError("", "Por favor ingresar bien la contraseña"); return View(tableUsuario); }
                return View(tableUsuario);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Por favor ingresar bien la contraseña"); return View(tableUsuario);
                throw;
            }
        }

        [Authorize(Roles = "Master, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TableUsuarios == null)
            {
                return NotFound();
            }
            var tableUsuario = await _context.TableUsuarios
                .Include(t => t.IdCorreoNavigation)
                .Include(t => t.IdRolNavigation)
                .Include(t => t.IdTelefonoNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (tableUsuario == null)
            {
                return NotFound();
            }
            return View(tableUsuario);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TableUsuarios == null)
            {
                return Problem("Entity set 'EstudioJuridicoContext.TableUsuarios'  is null.");
            }
            var tableUsuario = await _context.TableUsuarios.FindAsync(id);
            if (tableUsuario != null)
            {
                _context.TableUsuarios.Remove(tableUsuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableUsuarioExists(int id)
        {
          return _context.TableUsuarios.Any(e => e.IdUsuario == id);
        }
    }
}
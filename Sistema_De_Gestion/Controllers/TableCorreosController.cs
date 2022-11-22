using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_De_Gestion.Data;
using Sistema_De_Gestion.Models;
using System.Data;

namespace MVCPrueba2.Controllers
{
    public class TableCorreosController : Controller
    {
        //Context
        private readonly EstudioJuridicoContext _context;
        public TableCorreosController(EstudioJuridicoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        [HttpGet]
        public IActionResult Index(int id, string nombre)
        {
            var lista = _context.TableCorreos.ToList().Where(h => h.IdCliente == id);
            ViewData["idCliente"] = id;
            ViewData["Nombre"] = nombre;
            return View(lista);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public IActionResult Crear(int id, string nombre)
        {
            ViewData["idCliente"] = id;
            ViewData["Nombre"] = nombre;
            return View();
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Crear(TableCorreo ObjCorreo, int id, string nombre)
        {
            try
            {
                ObjCorreo.FechaAlta = DateTime.Now;
                ObjCorreo.FechaUltModificacion = ObjCorreo.FechaAlta;
                ObjCorreo.IdCliente = id;
                ObjCorreo.Obs = "---";
                //Guardamos el objeto...
                _context.Add(ObjCorreo);
                _context.SaveChanges();
                //llamamos al metodo Serialize_Alt_Correo
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                obLog.Serialize_Alt_Correo(ObjCorreo, "Usuario...");
                //volvemos a la lista de Correos
                return RedirectToAction("Index", new { id = id, nombre = nombre });
            }
            catch
            {
                ModelState.AddModelError("", "Por favor ingresar bien el Correo");
                ViewData["idCliente"] = id;
                ViewData["Nombre"] = nombre;
                return View(ObjCorreo);
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Editar(int idcorreo, int idcliente, string nombre)
        {
            var corr = await _context.TableCorreos.FindAsync(idcorreo);
            ViewData["idcorreo"] = idcorreo;
            ViewData["idCliente"] = idcliente;
            ViewData["Nombre"] = nombre;
            return View(corr);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Editar(int idcorreo, TableCorreo ObjCorreo, int idcliente, string nombre)
        {
            try
            {
                //traemos el obj original
                var obj = await _context.TableCorreos.FindAsync(idcorreo);
                obj.Correo = ObjCorreo.Correo;
                obj.FechaUltModificacion = DateTime.Now;
                obj.Habilitado = ObjCorreo.Habilitado;
                //Guardamos el objeto...
                _context.Update(obj);
                _context.SaveChanges();
                //llamamos al metodo Serialize_Mod_Correo
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                Boolean serializo = await obLog.Serialize_Mod_Correo(idcorreo, "Usuario...");
                //verificamos si se serializo...
                if (serializo)
                {
                    int cambios = await _context.SaveChangesAsync();
                }
                //volvemos a la lista de Correos
                return RedirectToAction("Index", new { id = idcliente, nombre = nombre });
            }
            catch
            {
                ModelState.AddModelError("", "Por favor ingresar bien el Correo");
                var corr = await _context.TableCorreos.FindAsync(idcorreo);
                ViewData["idcorreo"] = idcorreo;
                ViewData["idCliente"] = idcliente;
                ViewData["Nombre"] = nombre;
                return View(corr);
            }

        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int idcorreo, int idcliente, string nombre)
        {
            if (idcorreo == 0)
                return NotFound();

            var corr = await _context.TableCorreos.FindAsync(idcorreo);
            if (corr == null)
                return NotFound();

            ViewData["idCliente"] = idcliente;
            ViewData["Nombre"] = nombre;
            return View(corr);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int idcorreo, int idcliente, string nombre)
        {
            try
            {
                var tel = await _context.TableCorreos.FindAsync(idcorreo);
                _context.TableCorreos.Remove(tel);
                //llamamos al metodo Serialize_Delet_Correo
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                Boolean serializo = await obLog.Serialize_Delet_Correo(idcorreo, "Usuario...");
                //verificamos si se serializo...
                if (serializo)
                {
                    int cambios = await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                ViewData["idCliente"] = idcliente;
                ViewData["Nombre"] = nombre;
                return RedirectToAction("Index", new { id = tel.IdCliente, nombre = nombre }) ;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
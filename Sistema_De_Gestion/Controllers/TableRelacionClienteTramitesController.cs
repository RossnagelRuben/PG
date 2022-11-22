using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_De_Gestion.Data;
using Sistema_De_Gestion.Models;
using System.Data;

namespace MVCPrueba2.Controllers
{
    public class TableRelacionClienteTramitesController : Controller
    {
        //Context
        private readonly EstudioJuridicoContext _context;
        public TableRelacionClienteTramitesController(EstudioJuridicoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public async Task<IActionResult> Index(int id)
        {
            //pasamos el nombre del cliente
            TableCliente cliente = new TableCliente();
            var listaclientes = _context.TableClientes.ToList();
            cliente = listaclientes.Where(a => a.IdCliente == id).Single();
            string nombCliente = cliente.Apellidos + " " + cliente.Nombres;
            ViewData["cliente"] = nombCliente;
            ViewData["Idcliente"] = id;
            //pasamos los tramites
            ViewData["Tramites"] = _context.TableSubTramites.ToList();
            //pasamos lista de tramites solo de ese cliente
            var lista = _context.TableRelacionClienteTramites.ToList().Where(a => a.IdCliente == id);
            return View(lista);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public IActionResult Create(int IDCLIENT)
        {
            //pasamos el nombre del cliente
            TableCliente cliente = new TableCliente();
            var listaclientes = _context.TableClientes.ToList();
            cliente = listaclientes.Where(a => a.IdCliente == IDCLIENT).Single();
            string nombCliente = cliente.Apellidos + " " + cliente.Nombres;
            ViewData["cliente"] = nombCliente;
            ViewData["idcliente"] = IDCLIENT;
            //pasamos por Viewdata una lista de tramites
            ViewData["Tramites"] = _context.TableSubTramites.ToList();
            //cremos un abj de tipo RelacionClienteTramite
            TableRelacionClienteTramite nuevaRelacion = new TableRelacionClienteTramite();
            return View(nuevaRelacion);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(int idcliente, TableRelacionClienteTramite ObjTable)
        {
            if (ModelState.IsValid)
            {
                if (ObjTable.Estado == null)
                {
                    ObjTable.Estado = "Iniciado";
                }
                if (ObjTable.Descripcion == null)
                {
                    ObjTable.Descripcion = "---";
                }
                ObjTable.Habilitado = true;
                ObjTable.FechaAlta = DateTime.Now;
                ObjTable.FechaUltModificacion = Convert.ToDateTime(ObjTable.FechaAlta);
                ObjTable.IdCliente = idcliente;

                _context.Add(ObjTable);
                await _context.SaveChangesAsync();

                //llamamos al metodo Serialize_Alt_Relacion
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                obLog.Serialize_Alt_Relacion(ObjTable, "Usuario...");

                return RedirectToAction("Index", new { id = ObjTable.IdCliente });
            }
            return View(ObjTable);
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public async Task<IActionResult> Detalles(int id, int idcliente, string Nombre)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var tramite = await _context.TableRelacionClienteTramites.FirstOrDefaultAsync(j => j.Idrelacion == id);
            if (tramite == null)
            {
                return NotFound();
            }
            ViewData["cliente"] = Nombre;
            ViewData["idcliente"] = idcliente;

            //pasamos el nombre del Tramite
            TableSubTramite Subtramite = new TableSubTramite();
            var listat = _context.TableSubTramites.ToList();
            Subtramite = listat.Where(a => a.IdsubTramite == tramite.IdsubTramite).Single();
            ViewData["tramite"] = Subtramite.TituloSubTramite;

            return View(tramite);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Editar(int idtramite, int idcliente, string nombre)
        {
            var Rtramite = await _context.TableRelacionClienteTramites.FindAsync(idtramite);
            if (Rtramite == null)
                return NotFound();

            ViewData["nombre"] = nombre;
            ViewData["idcliente"] = idcliente;
            ViewData["Tramites"] = _context.TableSubTramites.ToList();
            return View(Rtramite);
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Editar(TableRelacionClienteTramite Obj, int idcliente, string nombre, int idrelacion)
        {
            //traemos el obj originar
            var ObjOriginal = await _context.TableRelacionClienteTramites.FindAsync(idrelacion);

            //verificamos si hay campos null
            if (Obj.Estado == null)
            {
                Obj.Estado = ObjOriginal.Estado;
            }
            if (Obj.Descripcion == null)
            {
                Obj.Descripcion = "---";
            }

            //pasamos los valores modificados
            ObjOriginal.Descripcion = Obj.Descripcion;
            ObjOriginal.IdsubTramite = Obj.IdsubTramite;
            ObjOriginal.Estado = Obj.Estado;
            ObjOriginal.Habilitado = Obj.Habilitado;
            //pasamos la fecha actual
            ObjOriginal.FechaUltModificacion = DateTime.Now;

            //pasamos el ObjOriginal modificado a la base de datos
            _context.Update(ObjOriginal);
            await _context.SaveChangesAsync();

            //llamamos al metodo Serialize_Mod_Relacion
            //----------------------------------------------------------
            Logs obLog = new Logs(_context);
            obLog.Serialize_Mod_Relacion(idrelacion, "Usuario...");

            //volvemos
            return RedirectToAction("Index", new { id = idcliente });
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var Relacion = await _context.TableRelacionClienteTramites.FindAsync(id);
            if (Relacion == null)
                return NotFound();

            
            return View(Relacion);
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var Relacion = await _context.TableRelacionClienteTramites.FindAsync(id);
                _context.TableRelacionClienteTramites.Remove(Relacion);

                //llamamos al metodo Serialize_Delet_Relacion
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                Boolean serializo = await obLog.Serialize_Delet_Relacion(id, "Usuario...");
                //verificamos si se serializo...
                if (serializo)
                {
                   int cambios = await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", new { id = Relacion.IdCliente });
            }
            catch (Exception ex)
            {
                //Redireccione a la pagina de error.
                return null;
            }
        }
    }
}
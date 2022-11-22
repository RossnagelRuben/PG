using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_De_Gestion.Models;
using System.Data;

namespace MVCPrueba2.Controllers
{
    public class TableSubTramitesController : Controller
    {
        //Context
        private readonly EstudioJuridicoContext _context;
        public TableSubTramitesController(EstudioJuridicoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public IActionResult Index()
        {
            var listaTramites = _context.TableSubTramites.ToList();
            return View(listaTramites);
        }

        [Authorize(Roles = "Master, Admin")]
        public IActionResult Create()
        {
            TableSubTramite nuevo = new TableSubTramite();
            ViewData["Tramites"] = _context.TableTipoTramites.ToList();
            return View(nuevo);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TableSubTramite tramite)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tramite.FechaAlta = DateTime.Now;
                    tramite.FechaUltModificacion = tramite.FechaAlta;
                    _context.Add(tramite);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                //ViewData["Tramites"] = _context.TableTipoTramite.ToList();
                return View(tramite);
            }
            catch
            {
                //return View(Error.Message);
                ModelState.AddModelError("", "Por favor ingresar bien los datos");
                ViewData["Tramites"] = _context.TableTipoTramites.ToList();
                return View(tramite);
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var tramite = await _context.TableSubTramites.FindAsync(id);
            if (tramite == null)
                return NotFound();
            ViewData["Tramites"] = _context.TableTipoTramites.ToList();
            return View(tramite);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TableSubTramite tramite)
        {
            try
            {
                if (id != tramite.IdsubTramite)
                    return NotFound();
                //hacemos una validacion
                if (ModelState.IsValid)
                {
                    var objTramiteOrig = await _context.TableSubTramites.FindAsync(id);

                    //pasamos los parametros correspondientes
                    objTramiteOrig.Idtramite = tramite.Idtramite;
                    objTramiteOrig.TituloSubTramite = tramite.TituloSubTramite;
                    objTramiteOrig.DescripcionSubTramite = tramite.DescripcionSubTramite;
                    objTramiteOrig.Habilitado = tramite.Habilitado;
                    objTramiteOrig.FechaUltModificacion = DateTime.Now;

                    //si el modelo es valido, aplicamos los cambios a la DB
                    _context.Update(objTramiteOrig);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(tramite);
            }
            catch
            {
                //return View(Error.Message);
                ModelState.AddModelError("", "Por favor ingresar bien los datos");
                ViewData["Tramites"] = _context.TableSubTramites.ToList();
                return View(tramite);
            }
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public async Task<IActionResult> Detalles(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var tramite = await _context.TableSubTramites.FirstOrDefaultAsync(j => j.IdsubTramite == id);
            if (tramite == null)
            {
                return NotFound();
            }
            return View(tramite);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string tramite, bool bandera = true)
        {
            if (id == 0)
                return NotFound();

            var subtramite = await _context.TableSubTramites.FindAsync(id);
            if (subtramite == null)
                return NotFound();

            ViewData["Tramite"] = tramite;

            if (bandera == false)
                ModelState.AddModelError("", "Primero hay que eliminar todas las relacioens asociadas");

            return View(subtramite);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id, string tramite)
        {
            try
            {
                var subtramite = await _context.TableSubTramites.FindAsync(id);
                _context.TableSubTramites.Remove(subtramite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Primero hay que eliminar todas las relacioens asociadas");
                //return View(id);
                return RedirectToAction("Delete", new { id = id, tramite = tramite, bandera = false });
            }
        }
    }
}
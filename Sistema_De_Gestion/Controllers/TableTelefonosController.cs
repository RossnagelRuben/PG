using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_De_Gestion.Data;
using Sistema_De_Gestion.Models;
using System.Data;

namespace MVCPrueba2.Controllers
{
    public class TableTelefonosController : Controller
    {
        //Context
        private readonly EstudioJuridicoContext _context;

        public TableTelefonosController(EstudioJuridicoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public IActionResult Index(int id, string nombre)
        {
            var lista = _context.TableTelefonos.ToList().Where(h => h.IdCliente == id);
            ViewData["idCliente"] = id;
            ViewData["Nombre"] = nombre;
            return View(lista);
        }

        [Authorize(Roles = "Master, Admin")]
        public IActionResult Crear(int id, string nombre)
        {
            ViewData["idCliente"] = id;
            ViewData["Nombre"] = nombre;
            return View();
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Crear(TableTelefono objPost, int id, string nombre, string Telefono)
        {
            //pasamos las propiedades
            objPost.IdCliente = id;

            if (string.IsNullOrWhiteSpace(Telefono) == false)
            {
                //FUNCION PARA AGREGAR Telefono
                try
                {
                    int contadort = 0;
                    string codArea = "", Tel = "";
                    for (int i = 0; i < Telefono.Length; i++)
                    {
                        if (Telefono[i] == '-')
                        {
                            contadort++;
                        }
                        else
                        {
                            if (contadort == 0)//primeros numeros
                            {
                                codArea = codArea + Telefono[i];
                            }
                            else if (contadort == 1) //tel
                            {
                                Tel = Tel + Telefono[i];
                            }
                        }
                    }

                    //una vez sale del bucle for...
                    objPost.CodigoDeArea = Convert.ToInt32(codArea);
                    objPost.Telefono = Convert.ToInt32(Tel);
                }
                catch (Exception Error)
                {
                    ModelState.AddModelError("", "Por favor ingresar bien los datos");
                    ViewData["idCliente"] = id;
                    ViewData["Nombre"] = nombre;
                    return View(objPost);
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor ingresar bien el Telefono");
                ViewData["idCliente"] = id;
                ViewData["Nombre"] = nombre;
                return View(objPost);
            }

            //pasamos las otras propiedades al obj

            if (objPost.Obs == null)
            {
                objPost.Obs = "---";
            }

            objPost.FechaAlta = DateTime.Now;
            objPost.FechaUltModificacion = objPost.FechaAlta;

            //Guardamos el objeto...
            _context.Add(objPost);
            _context.SaveChanges();

            //llamamos al metodo Serialize_Alt_Telefono
            //----------------------------------------------------------
            Logs obLog = new Logs(_context);
            obLog.Serialize_Alt_Telefono(objPost, "Usuario...");

            //volvemos a la lista de telefonos
            return RedirectToAction("Index", new { id = id, nombre = nombre  });
        }

        [Authorize(Roles = "Master, Admin")]
        public async Task<IActionResult> Editar(int idtelef, int idcliente, string nombre)
        {
            if (idtelef != 0)
            {
                //pasamos el obj con ese ID (idtelef)
                var tel = await _context.TableTelefonos.FindAsync(idtelef);
                ViewData["idtelef"] = idtelef;
                ViewData["idCliente"] = idcliente;
                ViewData["Nombre"] = nombre;
                return View(tel);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Editar(int idtelef, TableTelefono ObjEditTel, int idcliente, string nombre, string Telefono)
        {
            if (idtelef == 0)
            {
                return NotFound();
            }
            else
            {
                //pasamos el obj con ese ID
                var telORIGINAL = await _context.TableTelefonos.FindAsync(idtelef);

                //Pasamos las propiedades modificadas al obj con datos originales
                telORIGINAL.Obs = ObjEditTel.Obs;
                telORIGINAL.Habilitado = ObjEditTel.Habilitado;

                if (string.IsNullOrWhiteSpace(Telefono) == false)
                {
                    //ahora pasamos el cod de area y tel
                    try
                    {
                        int contadort = 0;
                        string codArea = "", Tel = "";
                        for (int i = 0; i < Telefono.Length; i++)
                        {
                            if (Telefono[i] == '-')
                            {
                                contadort++;
                            }
                            else
                            {
                                if (contadort == 0)//primeros numeros
                                {
                                    codArea = codArea + Telefono[i];
                                }
                                else if (contadort == 1) //tel
                                {
                                    Tel = Tel + Telefono[i];
                                }
                            }
                        }
                        //una vez sale del bucle for...
                        telORIGINAL.CodigoDeArea = Convert.ToInt32(codArea);
                        telORIGINAL.Telefono = Convert.ToInt32(Tel);
                    }
                    catch (Exception Error)
                    {
                        ModelState.AddModelError("", "Por favor ingresar bien el Telefono");
                        ViewData["idCliente"] = idcliente;
                        ViewData["Nombre"] = nombre;
                        return View(ObjEditTel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Por favor ingresar bien el Telefono");
                    ViewData["idCliente"] = idcliente;
                    ViewData["Nombre"] = nombre;
                    return View(ObjEditTel);
                }

                //pasamos la fecha actual
                telORIGINAL.FechaUltModificacion = DateTime.Now;

                //Actualizamos en la Base de Datos
                _context.Update(telORIGINAL);
                await _context.SaveChangesAsync();

                //llamamos al metodo Serialize_Mod_Telefono
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                Boolean serializo = await obLog.Serialize_Mod_Telefono(idtelef, "Usuario...");
                //verificamos si se serializo...
                if (serializo)
                {
                    int cambios = await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", new { id = idcliente, nombre = nombre });
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int idcliente, string nombre)
        {
            if (id == 0)
                return NotFound();

            var tel = await _context.TableTelefonos.FindAsync(id);
            if (tel == null)
                return NotFound();

            ViewData["idCliente"] = idcliente;
            ViewData["Nombre"] = nombre;
            return View(tel);
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id, int idcliente, string nombre)
        {

            var tel = await _context.TableTelefonos.FindAsync(id);
            _context.TableTelefonos.Remove(tel);
            //llamamos al metodo Serialize_Delet_Telefono
            //----------------------------------------------------------
            Logs obLog = new Logs(_context);
            Boolean serializo = await obLog.Serialize_Delet_Telefono(id, "Usuario...");
            //verificamos si se serializo...
            if (serializo)
            {
                int cambios = await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            ViewData["idCliente"] = id;
            ViewData["Nombre"] = nombre;
            return RedirectToAction("Index", new { id = idcliente, nombre = nombre });
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_De_Gestion.Models;
using Sistema_De_Gestion.Data;
using Sistema_De_Gestion.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MVCPrueba2.Controllers
{
    public class TableClientesController : Controller
    {
        //Context
        private readonly EstudioJuridicoContext _context;
        private readonly ILogger<HomeController> _logger;

        public TableClientesController(EstudioJuridicoContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public ActionResult Index(String msj)
        {
            var lista = _context.TableClientes.ToList();
            return View(lista);
        }

        [Authorize(Roles = "Master, Admin")]
        public IActionResult Create()
        {
            ViewData["Tramites"] = _context.TableSubTramites.ToList();
            TableCliente nuevo = new TableCliente();
            return View(nuevo);
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TableCliente Cliente, string CuilPost, int SubTramiteID, string Telefono, string Correo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //funcion para validar cuil
                    int contador = 0;
                    string cuilll1 = "", dni = "", cuilll2 = "";

                    if (string.IsNullOrWhiteSpace(CuilPost) == false)
                    {
                        for (int i = 0; i < CuilPost.Length; i++)
                        {
                            if (CuilPost[i] == '-')
                            {
                                contador++;
                            }
                            else
                            {
                                if (contador == 0)//primeros numeros
                                {
                                    cuilll1 = cuilll1 + CuilPost[i];
                                }
                                else if (contador == 1) //dni
                                {
                                    dni = dni + CuilPost[i];
                                }
                                else if (contador == 2) //ya pso el dni, ult numeros
                                {
                                    cuilll2 = cuilll2 + CuilPost[i];
                                }
                            }
                        }
                    }
                    else { }

                    //Pasamos las propiedades al objeto
                    Cliente.Cuil1 = Convert.ToInt32(cuilll1);
                    Cliente.Dni = Convert.ToInt32(dni);
                    Cliente.Cuil2 = Convert.ToInt32(cuilll2);
                    Cliente.FechaAlta = DateTime.Now;
                    Cliente.FechaUltModificacion = Cliente.FechaAlta;

                    //verificamos que todos los campos no obligatorios tengan un valor
                    if (Cliente.Direccion == null)
                    {
                        Cliente.Direccion = "-";
                    }
                    if (Cliente.Obs == null)
                    {
                        Cliente.Obs = "-";
                    }
                    if (Cliente.Link == null)
                    {
                        Cliente.Link = "-";
                    }
                    if (Cliente.PassAfip == null)
                    {
                        Cliente.PassAfip = "-";
                    }
                    if (Cliente.PassAnses == null)
                    {
                        Cliente.PassAnses = "-";
                    }
                    //Guardamos el objeto...
                    _context.Add(Cliente);
                    await _context.SaveChangesAsync();

                    if (Correo != null)
                    {
                        //Funcion para agregar Correo
                        AgregarCorreo(Cliente.IdCliente, Correo);
                    }

                    if (Telefono != null)
                    {
                        //Funcion para agregar Telefono
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
                        //pasamos los datos a la funcion de agregar
                        AgregarTelefono(Cliente.IdCliente, codArea, Tel);

                    }

                    //Una vezz Guardado el Cliente podemos dar de alta la relacion con el tramite
                    //la funcion recibe dos parametros para hacer el alta de la relacion
                    AgregarRelacion(Cliente.IdCliente, SubTramiteID);

                    //llamamos al metodo Serialize_Alta_Cliente
                    //----------------------------------------------------------
                    Logs obLog = new Logs(_context);
                    Boolean serializo = await obLog.Serialize_Alt_Cliente(Cliente.IdCliente, "Usuario...");
                    //verificamos si se serializo...
                    if (serializo)
                    {
                        int cambios = await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(Cliente);
            }
            catch (Exception Ex)
            {
                //return View(Error.Message);
                ModelState.AddModelError("", "Por favor ingresar bien los datos");
                ViewData["Tramites"] = _context.TableSubTramites.ToList();

                //llamamos al metodo Serialize_Error_Alt_Cliente
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                obLog.Serialize_Error_Alt_Cliente(Cliente, "Usuario...", Ex.Message.ToString());
                return View(Cliente);
            }
        }

        public void AgregarTelefono(int idCliente, string codArea, string tel)
        {
            if (idCliente != 0 && codArea != null && tel != null)
            {
                //creamos el obj
                TableTelefono TablaTelefono = new TableTelefono();

                //pasamos las propiedades
                TablaTelefono.IdCliente = idCliente;
                TablaTelefono.CodigoDeArea = Convert.ToInt32(codArea);
                TablaTelefono.Telefono = Convert.ToInt32(tel);
                TablaTelefono.Habilitado = true;
                TablaTelefono.Obs = "---";
                TablaTelefono.FechaAlta = DateTime.Now;
                TablaTelefono.FechaUltModificacion = TablaTelefono.FechaAlta;

                //Guardamos el objeto...
                _context.Add(TablaTelefono);
                _context.SaveChanges();
            }
        }

        public void AgregarCorreo(int IDCliente, string correo)
        {
            if (IDCliente != 0 && correo != null)
            {
                //si se pasan valores...

                //creamos el obj
                TableCorreo TablaCorreo = new TableCorreo();

                //pasamos las propiedades
                TablaCorreo.Correo = correo;
                TablaCorreo.IdCliente = IDCliente;
                TablaCorreo.Habilitado = true;
                TablaCorreo.Obs = "---";
                TablaCorreo.FechaAlta = DateTime.Now;
                TablaCorreo.FechaUltModificacion = TablaCorreo.FechaAlta;

                //Guardamos el objeto...
                _context.Add(TablaCorreo);
                _context.SaveChanges();
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound();

            var listaTramiteR = _context.TableRelacionClienteTramites.ToList();
            var Cliente = await _context.TableClientes.FindAsync(id);
            //Pasamos los tramites del cliente
            ViewData["Tramites"] = _context.TableSubTramites.ToList();
            string Cuil = Convert.ToString(Cliente.Cuil1) + "-" + Convert.ToString(Cliente.Dni) + "-" + Convert.ToString(Cliente.Cuil2);
            ViewData["Cuil"] = Cuil;

            if (Cliente == null)
                return NotFound();

            return View(Cliente);
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TableCliente cliente, string CuilPost)
        {
            try
            {
                //hacemos una validacion
                if (ModelState.IsValid)
                {
                    //funcion para validar cuil
                    int contador = 0;
                    string cuilll1 = "", dni = "", cuilll2 = "";
                    if (CuilPost == null)
                    {
                    }
                    else
                    {
                        for (int i = 0; i < CuilPost.Length; i++)
                        {
                            if (CuilPost[i] == '-')
                            {
                                contador++;
                            }
                            else
                            {
                                if (contador == 0)//primeros numeros
                                {
                                    cuilll1 = cuilll1 + CuilPost[i];
                                }
                                else if (contador == 1) //dni
                                {
                                    dni = dni + CuilPost[i];
                                }
                                else if (contador == 2) //ya pso el dni, ult numeros
                                {
                                    cuilll2 = cuilll2 + CuilPost[i];
                                }
                            }
                        }
                        //Pasamos las propiedades al objeto
                        cliente.Cuil1 = Convert.ToInt32(cuilll1);
                        cliente.Dni = Convert.ToInt32(dni);
                        cliente.Cuil2 = Convert.ToInt32(cuilll2);
                    }

                    var ObjClienteOrig = await _context.TableClientes.FindAsync(id);

                    //pasamos los parametros
                    ObjClienteOrig.Cuil1 = cliente.Cuil1;
                    ObjClienteOrig.Dni = cliente.Dni;
                    ObjClienteOrig.Cuil2 = cliente.Cuil2;
                    ObjClienteOrig.Nombres = cliente.Nombres;
                    ObjClienteOrig.Apellidos = cliente.Apellidos;
                    ObjClienteOrig.Direccion = cliente.Direccion;
                    ObjClienteOrig.Obs = cliente.Obs;
                    ObjClienteOrig.Habilitado = cliente.Habilitado;
                    ObjClienteOrig.Link = cliente.Link;
                    ObjClienteOrig.PassAfip = cliente.PassAfip;
                    ObjClienteOrig.PassAnses = cliente.PassAnses;
                    ObjClienteOrig.FechaUltModificacion = DateTime.Now;

                    //Guardamos el objeto...
                    _context.Update(ObjClienteOrig);
                    await _context.SaveChangesAsync();

                    //llamamos al metodo Serialize_Mod_Cliente
                    //----------------------------------------------------------
                    Logs obLog = new Logs(_context);
                    Boolean serializo = await obLog.Serialize_Mod_Cliente(id, "Usuario...");
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            catch(Exception Ex)
            {
                ModelState.AddModelError("", "Por favor ingresar bien los datos");
                ViewData["Tramites"] = _context.TableSubTramites.ToList();

                //llamamos al metodo Serialize_Error_Mod_Cliente
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                obLog.Serialize_Error_Mod_Cliente(id, cliente, "Usuario...", Ex.Message.ToString());
                return View(cliente);
            }
        }

        [Authorize(Roles = "Master, Admin, Lectura")]
        public async Task<IActionResult> Detalles(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var cliente = await _context.TableClientes.FirstOrDefaultAsync(j => j.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        //Codigo para crear la relacion entre el cliente y el subTramite
        public void AgregarRelacion(int IdCliente, int SubTramiteID)
        {
            if (IdCliente != 0 && SubTramiteID != 0)
            {
                //creamos el obj
                TableRelacionClienteTramite TablaRelacion = new TableRelacionClienteTramite();

                //pasamos las propiedades
                TablaRelacion.Descripcion = "-";
                TablaRelacion.IdsubTramite = SubTramiteID;
                TablaRelacion.IdCliente = IdCliente;
                TablaRelacion.Estado = "Iniciado";
                TablaRelacion.Habilitado = true;
                TablaRelacion.FechaAlta = DateTime.Now;
                TablaRelacion.FechaUltModificacion = TablaRelacion.FechaAlta;

                //Guardamos el objeto...
                _context.Add(TablaRelacion);
                _context.SaveChanges();
            }
        }

        [Authorize(Roles = "Master, Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, bool bandera = true)
        {
            if (id == 0)
                return NotFound();

            var cliente = await _context.TableClientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            if (bandera == false)
                ModelState.AddModelError("", "Primero hay que eliminar todas las relacioens asociadas");

            return View(cliente);
        }
        [Authorize(Roles = "Master, Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                var cliente = await _context.TableClientes.FindAsync(id);

                _context.TableClientes.Remove(cliente);
                //tenemos que hacer que se eliminen las referencias - relaciones del cliente antes de borrar al cliente...

                //Eliminar referencia de TableRelacionClienteTramite

                //Eliminar referencia de TableCorreo

                //Eliminar referencia de TableTelefono

                //llamamos al metodo Serialize_Delet_Cliente
                //----------------------------------------------------------
                Logs obLog = new Logs(_context);
                Boolean serializo = await obLog.Serialize_Delet_Cliente(cliente.IdCliente, "Usuario...");
                //verificamos si se serializo...
                if (serializo)
                {
                    int cambios = await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception Error)
            {
                ModelState.AddModelError("", "Primero hay que eliminar todas las relacioens asociadas");
                return RedirectToAction("Delete", new { id = id, bandera = false });
            }
        }
    }
}
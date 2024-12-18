using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
using tl2_tp6_2024_MauroOrlando2000.ViewModels;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("Presupuesto")]
public class PresupuestoController : Controller
{
    private IPresupuestoRepository repositorioPresupuestos;
    private IUserRepository repositorioUsuarios;
    private readonly ILogger<PresupuestoController> _logger;
    public PresupuestoController(IPresupuestoRepository repositorioPres, IUserRepository repositorioUser, ILogger<PresupuestoController> logger)
    {
        repositorioPresupuestos = repositorioPres;
        repositorioUsuarios = repositorioUser;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        try{
            var username = Request.Cookies["username"];
            if(username == null)
            {
                return RedirectToAction("IniciarSesion", "Presupuesto");
            }
            ViewData["username"] = username;
            ViewData["Rol"] = HttpContext.Session.GetInt32("role");
            return View(repositorioPresupuestos.ObtenerPresupuestos());
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto()
    {
        try{
            var username = Request.Cookies["username"];
            var Rol = HttpContext.Session.GetInt32("role");
            if(username == null)
            {
                return RedirectToAction("IniciarSesion", "Presupuesto");
            }
            else if(Rol == 1)
            {
                return RedirectToAction("Index", "Presupuesto");
            }
            ViewData["username"] = username;
            return View();
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto([FromForm]Presupuesto budget)
    {
        try{
            if(!repositorioPresupuestos.CrearPresupuesto(budget))
            {
                return View();
            }
            return RedirectToAction("Confirmar", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/EliminarPresupuesto/{id}")]
    public IActionResult EliminarPresupuesto([FromRoute]int id)
    {
        try{
            if(!repositorioPresupuestos.EliminarPresupuesto(id))
            {
                return RedirectToAction($"VistaDetallada/{id}", "Presupuesto");
            }
            return RedirectToAction("Confirmar", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromRoute]int id)
    {
        try{
            var username = Request.Cookies["username"];
            if(username == null)
            {
                return RedirectToAction("IniciarSesion", "Presupuesto");
            }
            ViewData["username"] = username;
            ViewData["Rol"] = HttpContext.Session.GetInt32("role");
            var ViewProductos = new AgregarProductoViewModel();
            ViewProductos.IdPresupuesto = id;
            return View(ViewProductos);
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromForm]AgregarProductoViewModel detalle)
    {
        try{
            if(!repositorioPresupuestos.AgregarProducto(detalle))
            {
                var ViewProductos = new AgregarProductoViewModel();
                ViewProductos.IdPresupuesto = detalle.IdPresupuesto;
                return View(ViewProductos);
            }
            return RedirectToAction("Confirmar", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Presupuesto/VistaDetallada/{id}")]
    public IActionResult VistaDetallada([FromRoute]int id)
    {
        try{
            var username = Request.Cookies["username"];
            if(username == null)
            {
                return RedirectToAction("IniciarSesion", "Presupuesto");
            }
            ViewData["username"] = username;
            ViewData["Rol"] = HttpContext.Session.GetInt32("role");
            return View(repositorioPresupuestos.Buscar(id));
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/VistaDetallada/EliminarProducto")]
    public IActionResult EliminarProducto(int idPres, int idProd)
    {
        try{
            if(!repositorioPresupuestos.EliminarProducto(idPres, idProd))
            {
                return RedirectToAction($"VistaDetallada/{idPres}", "Presupuesto");
            }
            return RedirectToAction("Confirmar", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Presupuesto/Confirmar")]
    public IActionResult Confirmar()
    {
        return View();
    }

    [HttpGet("/Presupuesto/IniciarSesion")]
    public IActionResult IniciarSesion()
    {
        try{
            var username = Request.Cookies["username"];
            if(username != null)
            {
                return RedirectToAction("Index", "Presupuesto");
            }
            return View();
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/IniciarSesion")]
    public IActionResult IniciarSesion([FromForm]UserViewModel log)
    {
        try{
            var user = repositorioUsuarios.Login(log.Usuario, log.Password);
            if(user == null)
            {
                _logger.LogWarning($"Intento de acceso invalido - Usuario:{log.Usuario} - Clave ingresada:{log.Password}");
                ViewData["Fallo"] = "Usuario no encontrado";
                return View();
            }
            _logger.LogInformation($"el usuario {user.Usuario} ingreso correctamente");
            HttpContext.Session.SetInt32("role", user.Rol);
            var options = new CookieOptions{
                Expires = DateTime.Now.AddSeconds(60),
                Secure = true,
                HttpOnly = true
            };
            Response.Cookies.Append("username", user.Usuario, options);
            return RedirectToAction("Index", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Presupuesto/Logout")]
    public IActionResult Logout()
    {
        try{
            Response.Cookies.Delete("username");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Presupuesto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
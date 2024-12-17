using System.Diagnostics;
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
    public PresupuestoController(IPresupuestoRepository repositorioPres, IUserRepository repositorioUser)
    {
        repositorioPresupuestos = repositorioPres;
        repositorioUsuarios = repositorioUser;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var username = Request.Cookies["username"];
        if(username == null)
        {
            return RedirectToAction("IniciarSesion", "Presupuesto");
        }
        ViewData["username"] = username;
        ViewData["Rol"] = HttpContext.Session.GetInt32("role");
        return View(repositorioPresupuestos.ObtenerPresupuestos());
    }

    [HttpGet("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto()
    {
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

    [HttpPost("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto([FromForm]Presupuesto budget)
    {
        if(!repositorioPresupuestos.CrearPresupuesto(budget))
        {
            return View();
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/ModificarPresupuesto/{id}")]
    public IActionResult ModificarPresupuesto([FromRoute]int id)
    {
        var username = Request.Cookies["username"];
        var Rol = HttpContext.Session.GetInt32("Rol");
        if(username == null)
        {
            return RedirectToAction("IniciarSesion", "Presupuesto");
        }
        else if(Rol == 1)
        {
            return RedirectToAction("Index", "Presupuesto");
        }
        ViewData["username"] = username;
        ViewData["Rol"] = HttpContext.Session.GetInt32("role");
        return View(repositorioPresupuestos.Buscar(id));
    }

    [HttpPost("/Presupuesto/ModificarPresupuesto/{id}")]
    public IActionResult ModificarPresupuesto([FromRoute]int id, [FromForm]Presupuesto budget)
    {
        if(!repositorioPresupuestos.ModificarPresupuesto(id, budget))
        {
            return View(repositorioPresupuestos.Buscar(id));
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpPost("/Presupuesto/EliminarPresupuesto/{id}")]
    public IActionResult EliminarPresupuesto([FromRoute]int id)
    {
        if(!repositorioPresupuestos.EliminarPresupuesto(id))
        {
            return RedirectToAction($"VistaDetallada/{id}", "Presupuesto");
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromRoute]int id)
    {
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

    [HttpPost("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromForm]AgregarProductoViewModel detalle)
    {
        if(!repositorioPresupuestos.AgregarProducto(detalle))
        {
            var ViewProductos = new AgregarProductoViewModel();
            ViewProductos.IdPresupuesto = detalle.IdPresupuesto;
            return View(ViewProductos);
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/VistaDetallada/{id}")]
    public IActionResult VistaDetallada([FromRoute]int id)
    {
        var username = Request.Cookies["username"];
        if(username == null)
        {
            return RedirectToAction("IniciarSesion", "Presupuesto");
        }
        ViewData["username"] = username;
        ViewData["Rol"] = HttpContext.Session.GetInt32("role");
        return View(repositorioPresupuestos.Buscar(id));
    }

    [HttpGet("/Presupuesto/Confirmar")]
    public IActionResult Confirmar()
    {
        return View();
    }

    [HttpGet("/Presupuesto/IniciarSesion")]
    public IActionResult IniciarSesion()
    {
        var username = Request.Cookies["username"];
        if(username != null)
        {
            return RedirectToAction("Index", "Presupuesto");
        }
        return View();
    }

    [HttpPost("/Presupuesto/IniciarSesion")]
    public IActionResult IniciarSesion([FromForm]UserViewModel log)
    {
        var user = repositorioUsuarios.Login(log.Usuario, log.Password);
        if(user == null)
        {
            ViewData["Fallo"] = "Usuario no encontrado";
            return View();
        }
        HttpContext.Session.SetInt32("role", user.Rol);
        Response.Cookies.Append("username", user.Usuario, new CookieOptions{
            Expires = DateTime.Now.AddSeconds(60)
        });
        return RedirectToAction("Index", "Presupuesto");
    }

    [HttpPost("/Presupuesto/Logout")]
    public IActionResult Logout()
    {
        /* Console.WriteLine("Variable de sesion antes: " + HttpContext.Session.GetInt32("role"));
        Console.WriteLine("Cookie antes: " + Request.Cookies["username"]); */
        Response.Cookies.Delete("username");
        HttpContext.Session.Clear();
        /* Console.WriteLine("Variable de sesion despues: " + HttpContext.Session.GetInt32("role"));
        Console.WriteLine("Cookie despues: " + Request.Cookies["username"]); */
        //Solo por propósito de debug
        return RedirectToAction("Index", "Presupuesto");
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
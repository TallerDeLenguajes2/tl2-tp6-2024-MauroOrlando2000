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
        return View(repositorioPresupuestos.ObtenerPresupuestos());
    }

    [HttpGet("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto()
    {
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
        return View();
    }

    [HttpPost("/Presupuesto/Login")]
    public IActionResult Login(User usuario)
    {
        
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
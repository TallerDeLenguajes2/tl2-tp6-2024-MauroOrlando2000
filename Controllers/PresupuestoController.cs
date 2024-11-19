using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("Presupuesto")]
public class PresupuestoController : Controller
{
    private IPresupuestoRepository repositorioPresupuestos;

    public PresupuestoController()
    {
        repositorioPresupuestos = new PresupuestoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(repositorioPresupuestos.ObtenerPresupuestos());
    }

    [HttpGet("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto()
    {
        ViewData["Clientes"] = new ClienteRepository().ObtenerClientes();
        return View();
    }

    [HttpPost("/Presupuesto/CrearPresupuesto")]
    public IActionResult CrearPresupuesto([FromForm]Presupuesto budget)
    {
        if(!repositorioPresupuestos.CrearPresupuesto(budget))
        {
            return RedirectToAction("Rechazo", "Presupuesto");
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpPost("/Presupuesto/EliminarPresupuesto/{id}")]
    public IActionResult EliminarPresupuesto([FromRoute]int id)
    {
        if(!repositorioPresupuestos.EliminarPresupuesto(id))
        {
            return RedirectToAction("Rechazo", "Presupuesto");
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromRoute]int id)
    {
        ViewData["idPres"] = id;
        ViewData["Productos"] = new ProductoRepository().ObtenerProductos();
        return View();
    }

    [HttpPost("/Presupuesto/AgregarProducto/{id}")]
    public IActionResult AgregarProducto([FromRoute]int id, [FromForm]PresupuestoDetalle detalle)
    {
        if(!repositorioPresupuestos.AgregarProducto(id, detalle))
        {
            return RedirectToAction("Rechazo", "Presupuesto");
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/VistaDetallada/{id}")]
    public IActionResult VistaDetallada([FromRoute]int id)
    {
        return View(repositorioPresupuestos.Buscar(id));
    }

    [HttpPost("/Presupuesto/EliminarProductoDetalle/{id}")]
    public IActionResult EliminarProductoDetalle([FromRoute]int id, [FromForm]int IdProducto)
    {
        if(!repositorioPresupuestos.EliminarProductoDetalle(id, IdProducto))
        {
            return RedirectToAction("Rechazo", "Presupuesto");
        }
        return RedirectToAction("Confirmar", "Presupuesto");
    }

    [HttpGet("/Presupuesto/Confirmar")]
    public IActionResult Confirmar()
    {
        return View();
    }

    [HttpGet("/Presupuesto/Rechazo")]
    public IActionResult Rechazo()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("controller")]
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

    [HttpGet("/CrearPresupuesto")]
    public IActionResult CrearPresupuesto()
    {
        return View();
    }

    [HttpPost("/CrearPresupuesto")]
    public IActionResult CrearPresupuesto([FromForm]Presupuesto budget)
    {
        if(!repositorioPresupuestos.CrearPresupuesto(budget))
        {
            return RedirectToAction("Error", "Presupuesto");
        }
        return RedirectToAction("ConfirmarPres", "Presupuesto");
    }

    [HttpGet("/ModificarPresupuesto/{id}")]
    public IActionResult ModificarPresupuesto([FromRoute]int id)
    {
        return View(repositorioPresupuestos.Buscar(id));
    }

    /* [HttpPost("/ModificarPresupuesto/{id}")]
    public IActionResult ModificarPresupuesto([FromRoute]int id, [FromForm]Presupuesto budget)
    {
        if(!repositorioPresupuestos.)
    } */

    [HttpPost("EliminarPresupuesto/{id}")]
    public IActionResult EliminarPresupuesto([FromRoute]int id)
    {
        if(!repositorioPresupuestos.EliminarPresupuesto(id))
        {
            return RedirectToAction("Error", "Presupuesto");
        }
        return RedirectToAction("ConfirmarPres", "Presupuesto");
    }

    [HttpGet("/ConfirmarPres")]
    public IActionResult ConfirmarPres()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
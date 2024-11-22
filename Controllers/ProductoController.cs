using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("Producto")]
public class ProductoController : Controller
{
    private ILogger<ProductoController> _logger;
    private IProductoRepository repositorioProductos;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        repositorioProductos = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(repositorioProductos.ObtenerProductos());
    }

    [HttpGet("/Producto/CrearProducto")]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost("/Producto/CrearProducto")]
    public IActionResult CrearProducto([FromForm]Producto producto)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }
        else if (!repositorioProductos.CrearProducto(producto))
        {
            return RedirectToAction("Rechazo", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpGet("/Producto/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id)
    {
        return View(repositorioProductos.Buscar(id));
    }

    [HttpPost("/Producto/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id, [FromForm]Producto producto)
    {
        if(!repositorioProductos.ModificarProducto(id, producto))
        {
            return RedirectToAction("Rechazo", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpPost("/Producto/EliminarProducto/{id}")]
    public IActionResult EliminarProducto([FromRoute]int id)
    {
        if(!repositorioProductos.EliminarProducto(id))
        {
            return RedirectToAction("Rechazo", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpGet("/Producto/Confirmar")]
    public IActionResult Confirmar()
    {
        return View();
    }

    [HttpGet("/Producto/Rechazo")]
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
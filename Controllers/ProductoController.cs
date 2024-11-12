using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("controller")]
public class ProductoController : Controller
{
    private ILogger<ProductoController> _logger;
    private IProductoRepository repositorioProductos;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        repositorioProductos = new ProductoRepository();
    }

    [HttpGet("Productos")]
    public IActionResult Index()
    {
        return View(repositorioProductos.ObtenerProductos());
    }

    [HttpGet("/CrearProducto")]
    public IActionResult CrearProducto()
    {
        return View();
    }

    [HttpPost("/CrearProducto")]
    public IActionResult CrearProducto([FromForm]Producto producto)
    {
        if(!repositorioProductos.CrearProducto(producto))
        {
            return RedirectToAction("Error", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpGet("/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id)
    {
        return View(repositorioProductos.Buscar(id));
    }

    [HttpPost("/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id, [FromForm]Producto producto)
    {
        if(!repositorioProductos.ModificarProducto(id, producto))
        {
            return RedirectToAction("Error", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpPost("/EliminarProducto/{id}")]
    public IActionResult EliminarProducto([FromRoute]int id)
    {
        if(!repositorioProductos.EliminarProducto(id))
        {
            return RedirectToAction("Error", "Producto");
        }
        return RedirectToAction("Confirmar", "Producto");
    }

    [HttpGet("/Confirmar")]
    public IActionResult Confirmar()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
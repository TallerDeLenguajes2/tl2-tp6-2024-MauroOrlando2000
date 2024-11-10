using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("controller")]
public class ProductoController : Controller
{
    private IProductoRepository repositorioProductos;

    public ProductoController()
    {
        repositorioProductos = new ProductoRepository();
    }

    [HttpGet]
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
        repositorioProductos.CrearProducto(producto);
        return View();
    }

    [HttpPut("/ModificarProducto")]
    public IActionResult ModificarProducto([FromRoute]int id, [FromForm]Producto producto)
    {
        return View(repositorioProductos.ModificarProducto(id, producto));
    }

    [HttpDelete]
    public IActionResult EliminarProducto([FromRoute]int id)
    {
        return View(repositorioProductos.EliminarProducto(id));
    }
}
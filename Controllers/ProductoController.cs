using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

public class ProductoController : Controller
{
    private IProductoRepository repositorioProductos;

    public ProductoController()
    {
        repositorioProductos = new ProductoRepository();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        return View(repositorioProductos.CrearProducto(producto));
    }

    [HttpGet]
    public IActionResult ObtenerProductos()
    {
        return View(repositorioProductos.ObtenerProductos());
    }

    [HttpGet]
    public IActionResult Buscar(int id)
    {
        return View(repositorioProductos.Buscar(id));
    }

    [HttpPut]
    public IActionResult ModificarProducto([FromRoute]int id, [FromBody]Producto producto)
    {
        return View(repositorioProductos.ModificarProducto(id, producto));
    }

    [HttpDelete]
    public IActionResult EliminarProducto(int id)
    {
        return View(repositorioProductos.EliminarProducto(id));
    }
}
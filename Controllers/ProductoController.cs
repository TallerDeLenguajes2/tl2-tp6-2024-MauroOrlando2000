using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
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
        if(!repositorioProductos.CrearProducto(producto))
        {
            return Error();
        }
        return Confirmar(producto.IdProducto);
    }

    [HttpGet("/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id)
    {
        return View(repositorioProductos.Buscar(id));
    }

    [HttpPut("/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id, [FromForm]Producto producto)
    {
        bool anda = repositorioProductos.ModificarProducto(id, producto);
        if(!anda)
        {
            return Error();
        }
        return Index();
    }

    [HttpDelete]
    public IActionResult EliminarProducto([FromRoute]int id)
    {
        return View(repositorioProductos.EliminarProducto(id));
    }

    [HttpGet]
    public IActionResult Confirmar([FromRoute]int id)
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
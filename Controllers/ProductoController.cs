using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;
namespace tl2_tp6_2024_MauroOrlando2000.Controllers;

[Route("Producto")]
public class ProductoController : Controller
{
    private IProductoRepository repositorioProductos;
    private IUserRepository repositorioUsuarios;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(IProductoRepository productos, IUserRepository usuarios, ILogger<ProductoController> logger)
    {
        repositorioProductos = productos;
        repositorioUsuarios = usuarios;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        try{
            var username = Request.Cookies["username"];
            if(username != null)
            {
                ViewData["username"] = username;
                ViewBag.rol = HttpContext.Session.GetInt32("role");
            }
            return View(repositorioProductos.ObtenerProductos());
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Producto/CrearProducto")]
    public IActionResult CrearProducto()
    {
        try{
            var username = Request.Cookies["username"];
            var rol = HttpContext.Session.GetInt32("role");
            if(username == null || rol != 2)
            {
                return RedirectToAction("Index", "Producto");
            }
            return View();   
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Producto/CrearProducto")]
    public IActionResult CrearProducto([FromForm]Producto producto)
    {
        try{
            if(!repositorioProductos.CrearProducto(producto))
            {
                return View();
            }
            return RedirectToAction("Confirmar", "Producto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Producto/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id)
    {
        try{
            var username = Request.Cookies["username"];
            var rol = HttpContext.Session.GetInt32("role");
            if(username == null || rol != 2)
            {
                return RedirectToAction("Index", "Producto");
            }
            return View(repositorioProductos.Buscar(id));
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Producto/ModificarProducto/{id}")]
    public IActionResult ModificarProducto([FromRoute]int id, [FromForm]Producto producto)
    {
        try{
            if(!repositorioProductos.ModificarProducto(id, producto))
            {
                return View(repositorioProductos.Buscar(id));
            }
            return RedirectToAction("Confirmar", "Producto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/Producto/EliminarProducto/{id}")]
    public IActionResult EliminarProducto([FromRoute]int id)
    {
        try{
            var username = Request.Cookies["username"];
            var rol = HttpContext.Session.GetInt32("role");
            if(username == null || rol != 2 || !repositorioProductos.EliminarProducto(id))
            {
                return RedirectToAction("Index", "Producto");
            }
            return RedirectToAction("Confirmar", "Producto");
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/Producto/Confirmar")]
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
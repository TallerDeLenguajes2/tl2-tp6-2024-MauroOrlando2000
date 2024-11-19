using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.Repositories;

namespace tl2_tp6_2024_MauroOrlando2000.Controllers
{
    [Route("Cliente")]
    public class ClienteController : Controller
    {
        private IClienteRepository repositorioClientes;

        public ClienteController()
        {
            repositorioClientes = new ClienteRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(repositorioClientes.ObtenerClientes());
        }

        [HttpGet("/Cliente/CrearCliente")]
        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost("/Cliente/CrearCliente")]
        public IActionResult CrearCliente(Cliente client)
        {
            if(!repositorioClientes.CrearCliente(client))
            {
                return RedirectToAction("Rechazo", "Cliente");
            }
            return RedirectToAction("Confirmar", "Cliente");
        }

        [HttpGet("/Cliente/ModificarCliente/{id}")]
        public IActionResult ModificarCliente(int id)
        {
            return View(repositorioClientes.Buscar(id));
        }

        [HttpPost("/Cliente/ModificarCliente/{id}")]
        public IActionResult ModificarCliente([FromRoute]int id, [FromForm]Cliente client)
        {
            if(!repositorioClientes.ModificarCliente(id, client))
            {
                return RedirectToAction("Rechazo", "Cliente");
            }
            return RedirectToAction("Confirmar", "Cliente");
        }

        [HttpPost("/Cliente/EliminarCliente/{id}")]
        public IActionResult EliminarCliente(int id)
        {
            if(!repositorioClientes.EliminarCliente(id))
            {
                return RedirectToAction("Rechazo", "Cliente");
            }
            return RedirectToAction("Confirmar", "Cliente");
        }

        [HttpGet("/Cliente/Confirmar")]
        public IActionResult Confirmar()
        {
            return View();
        }

        [HttpGet("/Cliente/Rechazo")]
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
}
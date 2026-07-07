using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuarioController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public IActionResult Index()
        {
            var usuarios = _usuarioRepo.ObtenerTodos();
            return View(usuarios);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            var existente = _usuarioRepo.ObtenerPorLogin(usuario.UsuarioLogin);
            if (existente != null)
            {
                ModelState.AddModelError("", "Ya existe un usuario con ese login.");
                return View(usuario);
            }
            _usuarioRepo.Registrar(usuario);
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var usuario = _usuarioRepo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            _usuarioRepo.Actualizar(usuario);
            return RedirectToAction("Index");
        }

        public IActionResult CambiarEstado(int id)
        {
            _usuarioRepo.CambiarEstado(id);
            return RedirectToAction("Index");
        }
    }
}
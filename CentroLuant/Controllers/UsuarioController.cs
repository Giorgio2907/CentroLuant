using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CentroLuant.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuarioController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        // Solo usuarios logueados con rol Recepcionista (administrador)
        // pueden entrar a este módulo. Ajusta "Recepcionista" si tu rol
        // de administrador se llama distinto.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rol = HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(rol))
            {
                context.Result = RedirectToAction("Login", "Account");
                return;
            }

            if (rol != "Recepcionista")
            {
                context.Result = RedirectToAction("AccesoDenegado", "Account");
                return;
            }

            base.OnActionExecuting(context);
        }

        // GET: Usuario
        public IActionResult Index()
        {
            var usuarios = _usuarioRepo.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuario/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Usuario/Crear
        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto) ||
                string.IsNullOrWhiteSpace(usuario.UsuarioLogin) ||
                string.IsNullOrWhiteSpace(usuario.ContrasenaHash))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View(usuario);
            }

            if (_usuarioRepo.ExisteLogin(usuario.UsuarioLogin))
            {
                ViewBag.Error = "Ese nombre de usuario ya existe.";
                return View(usuario);
            }

            usuario.Activo = true;
            _usuarioRepo.Registrar(usuario);
            TempData["Mensaje"] = "Usuario creado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Usuario/Editar/5
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioRepo.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuario/Editar/5
        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreCompleto) ||
                string.IsNullOrWhiteSpace(usuario.UsuarioLogin))
            {
                ViewBag.Error = "Nombre y usuario son obligatorios.";
                return View(usuario);
            }

            if (_usuarioRepo.ExisteLogin(usuario.UsuarioLogin, usuario.ID_Usuario))
            {
                ViewBag.Error = "Ese nombre de usuario ya está en uso por otro usuario.";
                return View(usuario);
            }

            _usuarioRepo.Actualizar(usuario);
            TempData["Mensaje"] = "Usuario actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // POST: Usuario/CambiarEstado/5
        // Recibe el estado ACTUAL del usuario y lo invierte
        [HttpPost]
        public IActionResult CambiarEstado(int id, bool activo)
        {
            _usuarioRepo.CambiarEstado(id, !activo);
            TempData["Mensaje"] = !activo
                ? "Usuario activado correctamente."
                : "Usuario desactivado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
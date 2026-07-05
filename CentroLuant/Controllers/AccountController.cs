using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AccountController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string contrasena)
        {
            var user = _usuarioRepo.ObtenerPorLogin(usuario);
            if (user == null || user.ContrasenaHash != contrasena)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
            HttpContext.Session.SetString("Usuario", user.NombreCompleto);
            HttpContext.Session.SetString("Rol", user.Rol);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
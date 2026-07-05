using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class PacienteController : Controller
    {
        private readonly PacienteRepository _pacienteRepo;

        public PacienteController(PacienteRepository pacienteRepo)
        {
            _pacienteRepo = pacienteRepo;
        }

        public IActionResult Index()
        {
            var pacientes = _pacienteRepo.ObtenerTodos();
            return View(pacientes);
        }

        public IActionResult Detalle(string dni)
        {
            var paciente = _pacienteRepo.ObtenerPorDNI(dni);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(Paciente paciente)
        {
            var existente = _pacienteRepo.ObtenerPorDNI(paciente.DNI);
            if (existente != null)
            {
                ModelState.AddModelError("", "Ya existe un paciente con ese DNI.");
                return View(paciente);
            }
            _pacienteRepo.Registrar(paciente);
            return RedirectToAction("Index");
        }

        public IActionResult Editar(string dni)
        {
            var paciente = _pacienteRepo.ObtenerPorDNI(dni);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        [HttpPost]
        public IActionResult Editar(Paciente paciente)
        {
            _pacienteRepo.Actualizar(paciente);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(string dni)
        {
            _pacienteRepo.Eliminar(dni);
            return RedirectToAction("Index");
        }
    }
}
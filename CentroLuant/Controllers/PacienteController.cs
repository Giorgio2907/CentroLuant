using CentroLuant.Models;
using CentroLuant.Repositories;
using CentroLuant.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class PacienteController : Controller
    {
        private readonly PacienteRepository _pacienteRepo;
        private readonly DniService _dniService;

        public PacienteController(PacienteRepository pacienteRepo, DniService dniService)
        {
            _pacienteRepo = pacienteRepo;
            _dniService = dniService;
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

        [HttpGet]
        public async Task<IActionResult> ConsultarDni(string dni)
        {
            if (string.IsNullOrEmpty(dni) || dni.Length != 8)
                return Json(new { success = false, message = "DNI inválido" });

            var resultado = await _dniService.ConsultarDni(dni);

            if (resultado == null)
                return Json(new { success = false, message = "No se encontró el DNI" });

            return Json(new
            {
                success = true,
                nombres = resultado.Nombres,
                apellidos = $"{resultado.ApellidoPaterno} {resultado.ApellidoMaterno}"
            });
        }
    }
}
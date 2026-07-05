using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class HistorialController : Controller
    {
        private readonly HistorialRepository _historialRepo;
        private readonly PacienteRepository _pacienteRepo;

        public HistorialController(HistorialRepository historialRepo, PacienteRepository pacienteRepo)
        {
            _historialRepo = historialRepo;
            _pacienteRepo = pacienteRepo;
        }

        public IActionResult Index(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                return View(null as HistorialMedico);

            var paciente = _pacienteRepo.ObtenerPorDNI(dni);
            if (paciente == null)
            {
                ViewBag.Error = "No se encontró el paciente.";
                return View(null as HistorialMedico);
            }

            var historial = _historialRepo.ObtenerPorPaciente(dni);
            if (historial != null)
            {
                historial.NombrePaciente = paciente.NombreCompleto;
                var tratamientos = _historialRepo.ObtenerTratamientos(historial.ID_Historial);
                ViewBag.Tratamientos = tratamientos;
            }

            ViewBag.Paciente = paciente;
            return View(historial);
        }

        public IActionResult Crear(string dni)
        {
            var paciente = _pacienteRepo.ObtenerPorDNI(dni);
            if (paciente == null) return NotFound();
            ViewBag.Paciente = paciente;
            return View();
        }

        [HttpPost]
        public IActionResult Crear(HistorialMedico historial)
        {
            var existente = _historialRepo.ObtenerPorPaciente(historial.DNI_Paciente);
            if (existente != null)
            {
                ModelState.AddModelError("", "El paciente ya tiene un historial clínico.");
                return View(historial);
            }
            historial.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            _historialRepo.Crear(historial);
            return RedirectToAction("Index", new { dni = historial.DNI_Paciente });
        }

        public IActionResult Editar(int id)
        {
            var tratamiento = new Tratamiento { ID_Historial = id };
            return View(tratamiento);
        }

        [HttpPost]
        public IActionResult AgregarTratamiento(Tratamiento tratamiento)
        {
            tratamiento.FechaTratamiento = DateOnly.FromDateTime(DateTime.Now);
            _historialRepo.RegistrarTratamiento(tratamiento);
            var historial = _historialRepo.ObtenerPorPaciente(
                _historialRepo.ObtenerTratamientos(tratamiento.ID_Historial).First().ID_Historial.ToString());
            return RedirectToAction("Index");
        }
    }
}
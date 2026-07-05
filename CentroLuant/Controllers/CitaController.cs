using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class CitaController : Controller
    {
        private readonly CitaRepository _citaRepo;
        private readonly PacienteRepository _pacienteRepo;
        private readonly EspecialistaRepository _especialistaRepo;

        public CitaController(CitaRepository citaRepo, PacienteRepository pacienteRepo, EspecialistaRepository especialistaRepo)
        {
            _citaRepo = citaRepo;
            _pacienteRepo = pacienteRepo;
            _especialistaRepo = especialistaRepo;
        }

        public IActionResult Index()
        {
            var citas = _citaRepo.ObtenerTodas();
            return View(citas);
        }

        public IActionResult Consultar(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                return View(new List<Cita>());
            var citas = _citaRepo.ObtenerPorPaciente(dni);
            ViewBag.DNI = dni;
            return View(citas);
        }

        public IActionResult Registrar()
        {
            ViewBag.Especialistas = _especialistaRepo.ObtenerTodos();
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(Cita cita)
        {
            bool disponible = _citaRepo.VerificarDisponibilidad(cita.ID_Especialista, cita.Fecha, cita.Hora);
            if (!disponible)
            {
                ModelState.AddModelError("", "El especialista no está disponible en ese horario.");
                ViewBag.Especialistas = _especialistaRepo.ObtenerTodos();
                return View(cita);
            }
            _citaRepo.Registrar(cita);
            return RedirectToAction("Index");
        }

        public IActionResult Cancelar(int id)
        {
            _citaRepo.Cancelar(id);
            return RedirectToAction("Index");
        }

        public IActionResult Reprogramar(int id)
        {
            var cita = _citaRepo.ObtenerPorId(id);
            if (cita == null) return NotFound();
            return View(cita);
        }

        [HttpPost]
        public IActionResult Reprogramar(int id, DateOnly nuevaFecha, TimeOnly nuevaHora)
        {
            _citaRepo.Reprogramar(id, nuevaFecha, nuevaHora);
            return RedirectToAction("Index");
        }
    }
}
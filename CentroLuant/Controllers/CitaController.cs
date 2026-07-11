using CentroLuant.Models;
using CentroLuant.Repositories;
using CentroLuant.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class CitaController : Controller
    {
        private readonly CitaRepository _citaRepo;
        private readonly PacienteRepository _pacienteRepo;
        private readonly EspecialistaRepository _especialistaRepo;
        private readonly CorreoService _correoService;

        public CitaController(CitaRepository citaRepo, PacienteRepository pacienteRepo,
            EspecialistaRepository especialistaRepo, CorreoService correoService)
        {
            _citaRepo = citaRepo;
            _pacienteRepo = pacienteRepo;
            _especialistaRepo = especialistaRepo;
            _correoService = correoService;
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
        public async Task<IActionResult> Registrar(Cita cita)
        {
            var paciente = _pacienteRepo.ObtenerPorDNI(cita.DNI_Paciente);
            if (paciente == null)
            {
                ModelState.AddModelError("", "No existe ningún paciente registrado con ese DNI.");
                ViewBag.Especialistas = _especialistaRepo.ObtenerTodos();
                return View(cita);
            }

            bool disponible = _citaRepo.VerificarDisponibilidad(cita.ID_Especialista, cita.Fecha, cita.Hora);
            if (!disponible)
            {
                ModelState.AddModelError("", "El especialista no está disponible en ese horario. Debe haber al menos 30 minutos de diferencia entre citas.");
                ViewBag.Especialistas = _especialistaRepo.ObtenerTodos();
                return View(cita);
            }

            _citaRepo.Registrar(cita);

            var especialista = _especialistaRepo.ObtenerPorId(cita.ID_Especialista);

            if (!string.IsNullOrEmpty(paciente.CorreoElectronico) && especialista != null)
            {
                try
                {
                    await _correoService.EnviarRecordatorioCita(
                        paciente.CorreoElectronico,
                        paciente.NombreCompleto,
                        cita.Fecha.ToString("dd/MM/yyyy"),
                        cita.Hora.ToString("HH:mm"),
                        especialista.NombreCompleto
                    );
                }
                catch { }
            }

            TempData["Exito"] = "Cita registrada correctamente.";
            return RedirectToAction("Index");
        }

        public IActionResult Cancelar(int id)
        {
            _citaRepo.Cancelar(id);
            TempData["Exito"] = "Cita cancelada correctamente.";
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
            TempData["Exito"] = "Cita reprogramada correctamente.";
            return RedirectToAction("Index");
        }
        public IActionResult Eliminar(int id)
        {
            _citaRepo.Eliminar(id);
            TempData["Exito"] = "Cita eliminada correctamente.";
            return RedirectToAction("Index");
        }
    }
}
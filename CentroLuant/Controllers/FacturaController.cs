using CentroLuant.Models;
using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class FacturaController : Controller
    {
        private readonly FacturaRepository _facturaRepo;
        private readonly PacienteRepository _pacienteRepo;
        private readonly HistorialRepository _historialRepo;

        public FacturaController(FacturaRepository facturaRepo, PacienteRepository pacienteRepo, HistorialRepository historialRepo)
        {
            _facturaRepo = facturaRepo;
            _pacienteRepo = pacienteRepo;
            _historialRepo = historialRepo;
        }

        public IActionResult Index()
        {
            var facturas = _facturaRepo.ObtenerTodas();
            return View(facturas);
        }

        public IActionResult Detalle(int id)
        {
            var factura = _facturaRepo.ObtenerPorId(id);
            if (factura == null) return NotFound();
            return View(factura);
        }

        public IActionResult Generar(string dni)
        {
            var paciente = _pacienteRepo.ObtenerPorDNI(dni);
            if (paciente == null) return NotFound();
            var historial = _historialRepo.ObtenerPorPaciente(dni);
            if (historial != null)
            {
                var tratamientos = _historialRepo.ObtenerTratamientos(historial.ID_Historial);
                ViewBag.Tratamientos = tratamientos;
            }
            ViewBag.Paciente = paciente;
            return View();
        }

        [HttpPost]
        public IActionResult Generar(Factura factura)
        {
            factura.FechaEmision = DateOnly.FromDateTime(DateTime.Now);
            _facturaRepo.Registrar(factura);
            return RedirectToAction("Detalle", new { id = _facturaRepo.ObtenerPorPaciente(factura.DNI_Paciente).Last().ID_Factura });
        }

        public IActionResult ActualizarEstado(int id, string estado)
        {
            _facturaRepo.ActualizarEstado(id, estado);
            return RedirectToAction("Index");
        }
    }
}

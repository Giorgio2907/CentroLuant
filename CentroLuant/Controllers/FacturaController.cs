using CentroLuant.Models;
using CentroLuant.Repositories;
using CentroLuant.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class FacturaController : Controller
    {
        private readonly FacturaRepository _facturaRepo;
        private readonly PacienteRepository _pacienteRepo;
        private readonly HistorialRepository _historialRepo;
        private readonly TipoCambioService _tipoCambioService;
        private readonly FacturaPdfService _pdfService;

        public FacturaController(FacturaRepository facturaRepo, PacienteRepository pacienteRepo,
            HistorialRepository historialRepo, TipoCambioService tipoCambioService, FacturaPdfService pdfService)
        {
            _facturaRepo = facturaRepo;
            _pacienteRepo = pacienteRepo;
            _historialRepo = historialRepo;
            _tipoCambioService = tipoCambioService;
            _pdfService = pdfService;
        }

        public IActionResult Index()
        {
            var facturas = _facturaRepo.ObtenerTodas();
            return View(facturas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var factura = _facturaRepo.ObtenerPorId(id);
            if (factura == null) return NotFound();
            var tipoCambio = await _tipoCambioService.ObtenerTipoCambio();
            ViewBag.TipoCambio = tipoCambio;
            ViewBag.MontoUSD = Math.Round(factura.MontoTotal * tipoCambio, 2);
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
            TempData["Exito"] = "Factura generada correctamente.";
            var facturas = _facturaRepo.ObtenerPorPaciente(factura.DNI_Paciente);
            return RedirectToAction("Detalle", new { id = facturas.Last().ID_Factura });
        }

        public IActionResult ActualizarEstado(int id, string estado)
        {
            _facturaRepo.ActualizarEstado(id, estado);
            TempData["Exito"] = "Estado de factura actualizado correctamente.";
            return RedirectToAction("Index");
        }

        public IActionResult DescargarPdf(int id)
        {
            var factura = _facturaRepo.ObtenerPorId(id);
            if (factura == null) return NotFound();
            var pdf = _pdfService.GenerarPdf(factura);
            return File(pdf, "application/pdf", $"Factura_{id}.pdf");
        }
    }
}
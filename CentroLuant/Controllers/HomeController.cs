using CentroLuant.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CentroLuant.Controllers
{
    public class HomeController : Controller
    {
        private readonly PacienteRepository _pacienteRepo;
        private readonly CitaRepository _citaRepo;
        private readonly FacturaRepository _facturaRepo;

        public HomeController(PacienteRepository pacienteRepo, CitaRepository citaRepo, FacturaRepository facturaRepo)
        {
            _pacienteRepo = pacienteRepo;
            _citaRepo = citaRepo;
            _facturaRepo = facturaRepo;
        }

        public IActionResult Index()
        {
            ViewBag.TotalPacientes = _pacienteRepo.ContarPacientes();
            ViewBag.TotalCitas = _citaRepo.ContarCitas();
            ViewBag.CitasHoy = _citaRepo.ContarCitasHoy();
            ViewBag.FacturasPendientes = _facturaRepo.ContarFacturasPendientes();
            return View();
        }
    }
}
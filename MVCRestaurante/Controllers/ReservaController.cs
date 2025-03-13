﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCRestaurante.Filters;
using MVCRestaurante.Models;
using MVCRestaurante.Repositories;

namespace MVCRestaurante.Controllers
{
    public class ReservaController : Controller
    {
        private readonly RepositoryReservas _repo;

        public ReservaController(RepositoryReservas repo)
        {
            _repo = repo;
        }

        // Vista para los usuarios normales:
        public IActionResult Index()
        {
            var fechaHoy = DateTime.Today;
            List<string> horariosDisponibles = _repo.ObtenerHorariosDisponibles(fechaHoy);

            ViewBag.HorariosDisponibles = horariosDisponibles;
            ViewBag.Reservas = _repo.GetReservas();

            return View();
        }


        [HttpPost]
        public IActionResult ObtenerHorariosDisponibles(DateTime fecha)
        {
            var horarios = _repo.ObtenerHorariosDisponibles(fecha);
            return Json(horarios);
        }

        // Registrar una nueva reserva
        [HttpPost]
        public IActionResult CrearReserva(Reserva reserva)
        {
            if (!_repo.EsHorarioDisponible(reserva.FechaReserva, reserva.HoraReserva))
            {
                ModelState.AddModelError("", "Este horario ya está lleno. Elige otra hora.");
                return View("Index", reserva);
            }

            _repo.CrearReserva(reserva);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarReserva(int id)
        {
            _repo.EliminarReserva(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult ConfirmarReserva(int id)
        {
            _repo.ConfirmarReserva(id);
            return Json(new { success = true });
        }
    }
}

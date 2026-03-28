using Microsoft.AspNetCore.Mvc;
using Practica01.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Practica01.Controllers
{
    public class CampaignsController : Controller
    {
        // Lista en memoria de campañas
        private static List<Campaign> campaigns = new List<Campaign>
        {
            new Campaign { Id = 1, Nombre = "Campaña A", Categoria = "Marketing", Estado = "Activa", Canal = "Email", Descuento = 10, FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(10) },
            new Campaign { Id = 2, Nombre = "Campaña B", Categoria = "Promoción", Estado = "Inactiva", Canal = "Redes", Descuento = 15, FechaInicio = DateTime.Today.AddDays(-5), FechaFin = DateTime.Today.AddDays(5) }
        };

        // Acción Index con filtros opcionales
        public IActionResult Index(string categoria = null, string estado = null)
        {
            var lista = campaigns.AsEnumerable();

            if (!string.IsNullOrEmpty(categoria))
                lista = lista.Where(c => c.Categoria == categoria);

            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(c => c.Estado == estado);

            return View(lista.ToList());
        }

        // Acción Details
        public IActionResult Details(int id)
        {
            var campaign = campaigns.FirstOrDefault(c => c.Id == id);
            if (campaign == null)
                return NotFound();

            return View(campaign);
        }

        // Acción Summary
        public IActionResult Summary()
        {
            var total = campaigns.Count;
            var vigentes = campaigns.Count(c => c.Estado == "Activa");
            var proximas = campaigns.Count(c => c.FechaInicio > DateTime.Today);
            var promedioDescuento = campaigns.Any() ? campaigns.Average(c => c.Descuento) : 0;
            var porCanal = campaigns
                .GroupBy(c => c.Canal)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewBag.Total = total;
            ViewBag.Vigentes = vigentes;
            ViewBag.Proximas = proximas;
            ViewBag.PromedioDescuento = promedioDescuento;
            ViewBag.PorCanal = porCanal;

            return View();
        }
    }
}
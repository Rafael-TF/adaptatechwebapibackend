﻿using System;
using adaptatechwebapibackend.Models;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Services
{
    public class OperacionesService
    {
        private readonly AdaptatechContext _context;
        private readonly IHttpContextAccessor _accessor;

        public OperacionesService(AdaptatechContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task AddOperacion(string operacion, string controller)
        {
            Operacione nuevaOperacion = new Operacione()
            {
                FechaAccion = DateTime.Now,
                Operacion = operacion,
                Controller = controller,
                Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            await _context.Operaciones.AddAsync(nuevaOperacion);
            await _context.SaveChangesAsync();

            Task.FromResult(0);
        }

        public async Task<bool> PuedeRealizarOperacion()
        {
            var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var minimumTimeSpan = TimeSpan.FromSeconds(10);
            var ultimaOperacion = await _context.Operaciones
                .Where(o => o.Ip == ip)
                .OrderByDescending(o => o.FechaAccion)
                .FirstOrDefaultAsync();

            if (ultimaOperacion == null)
            {
                return true;
            }

            var tiempoTranscurrido = DateTime.Now - ultimaOperacion.FechaAccion;
            return tiempoTranscurrido >= minimumTimeSpan;
        }
    }
}


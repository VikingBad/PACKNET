using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PACKNET.Models;
using System;

namespace PACKNET.Controllers
{
    public class VehiculoController : ControllerBase
    {
        #region BBDD
        private readonly PackNetDbContext _packNetDB;

        public VehiculoController(PackNetDbContext packNetDB)
        {
            _packNetDB = packNetDB;
        }
        #endregion

        #region Obtener Vehiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> Obtener()
        {
            return await _packNetDB.Vehiculo.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> ObtenerPorId(int id)
        {
            var vehiculo = await _packNetDB.Vehiculo.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return vehiculo;
        }
        #endregion

        #region Insertar Vehiculo
        [HttpPost]
        public async Task<ActionResult<Vehiculo>> Insertar(Vehiculo vehiculo)
        {
            _packNetDB.Vehiculo.Add(vehiculo);
            await _packNetDB.SaveChangesAsync();

            return CreatedAtAction("ObtenerPorId", new { id = vehiculo.Id }, vehiculo);
        }
        #endregion

        #region Modificar Vehiculo
        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return BadRequest();
            }

            _packNetDB.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                await _packNetDB.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteVehiculo(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        #endregion

        #region Eliminar Vehiculo
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            var vehiculo = await _packNetDB.Vehiculo.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _packNetDB.Vehiculo.Remove(vehiculo);
            await _packNetDB.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Localizador
        public IActionResult LocalizarVehiculo(int id_paquete, double longitud, double latitud)
        {
            using (var packNetDB = new PackNetDbContext())
            {
                // Obténemos el paquete que queremos localizar
                var paquete = packNetDB.Paquete.FirstOrDefault(p => p.Id == id_paquete);

                var vehiculo = packNetDB.Vehiculo.FirstOrDefault(v => v.Id == paquete.Id_Vehiculo);

                

                // Si se encuentra un vehículo cercano, le informamos de las coordenadas que tiene tras su ultima actualizacion
                if (vehiculo != null)
                {
                    return Ok("Su paquete se encuentra en el vehiculo " + vehiculo.Modelo + ", con matricula " + vehiculo.Matricula + ". Sus coordenadas son: Latitud -- " + vehiculo.Latitud + " Longitud -- " + vehiculo.Longitud);
                }
                else
                {
                    return NotFound("No se encontró ningún vehículo que transportara su paquete");
                }
            }
        }
        #endregion

        #region General
        private bool ExisteVehiculo(int id)
        {
            return _packNetDB.Vehiculo.Any(e => e.Id == id);
        }
        #endregion
    }
}

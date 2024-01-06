using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PACKNET.Models;
using System.Net;

namespace PACKNET.Controllers
{
    public class PaqueteController : ControllerBase
    {
        #region BBDD
        private readonly PackNetDbContext _packNetDB;

        public PaqueteController(PackNetDbContext packNetDB)
        {
            _packNetDB = packNetDB;
        }
        #endregion

        #region Obtener Paquetes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paquete>>> Obtener()
        {
            return await _packNetDB.Paquete.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paquete>> ObtenerPorId(int id)
        {
            var paquete = await _packNetDB.Paquete.FindAsync(id);

            if (paquete == null)
            {
                return NotFound();
            }

            return paquete;
        }
        #endregion

        #region Crear Paquete nuevo
        [HttpPost]
        public async Task<ActionResult<Paquete>> Insertar(Paquete paquete)
        {
            _packNetDB.Paquete.Add(paquete);
            await _packNetDB.SaveChangesAsync();

            return CreatedAtAction("ObtenerPorId", new { id = paquete.Id }, paquete);
        }
        #endregion

        #region Eliminar Paquete
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaquete(int id)
        {
            var paquete = await _packNetDB.Paquete.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }

            _packNetDB.Paquete.Remove(paquete);
            await _packNetDB.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Asignar Paquetes a Vehiculos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(int Id_Paquete, int Id_Vehiculo)
        {
            var paquete = _packNetDB.Paquete.Find(Id_Paquete);
            var vehiculo = _packNetDB.Vehiculo.Find(Id_Vehiculo);

            if (paquete == null || vehiculo == null)
            {
                return NotFound("No se pudo asignar el paquete al vehiculo");
            }

            paquete.Vehiculo = vehiculo;
            _packNetDB.SaveChanges();

            return RedirectToAction("Index");
        }

        #endregion

        #region Desasignar Paquetes a Vehiculos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Desasignar(int Id_Paquete)
        {
            var paquete = _packNetDB.Paquete.Find(Id_Paquete);

            if (paquete == null)
            {
                return NotFound("No se pudo desasignar el paquete");
            }
            else
            {
                paquete.Vehiculo = null;
                _packNetDB.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}

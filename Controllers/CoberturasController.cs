using AdminPolizasAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPolizasAPI.Controllers
{
    [ApiController]
    [Route("api/coberturas")]
    public class CoberturasController : ControllerBase
    {
        private readonly ApplicationDbContext contexto;

        public CoberturasController(ApplicationDbContext contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cobertura>>> ObtenerTodasLasCoberturas()
        {
            return await contexto.Coberturas.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cobertura>> ObtenerCoberturaPorId(int id)
        {
            var cobertura = await contexto.Coberturas.FirstOrDefaultAsync(c => c.Id == id);

            if (cobertura == null)
            {
                return NotFound();
            }

            return cobertura;
        }

        [HttpPost]
        public async Task<ActionResult> CrerCobertura (Cobertura cobertura)
        {
            var existeCobertura = await contexto.Coberturas.AnyAsync(c => c.Nombre == cobertura.Nombre);
            if (existeCobertura)
            {
                return BadRequest($"Ya existe cobertura con nombre {cobertura.Nombre}");
            }
            contexto.Add(cobertura);
            await contexto.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ActualizarCobertura (Cobertura cobertura, int id)
        {
            if (id != cobertura.Id)
            {
                return BadRequest("El id de la cobertura no coincide con el id {id}");
            }

            if (!await ExisteCobertura(id))
            {
                return NotFound();
            }

            contexto.Update(cobertura);
            await contexto.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarCobertura (int id)
        {
            if (!await ExisteCobertura(id))
            {
                return NotFound();
            }
            contexto.Remove(new Cobertura { Id = id }); 
            await contexto.SaveChangesAsync();
            return Ok();
        }

        private async Task<bool> ExisteCobertura(int id)
        {
            return await contexto.Coberturas.AnyAsync(e => e.Id == id);
        }
    }
}

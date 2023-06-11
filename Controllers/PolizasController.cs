using AdminPolizasAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using AdminPolizasAPI.DTO;

namespace AdminPolizasAPI.Controllers
{
    [ApiController]
    [Route("api/polizas")]
    public class PolizasController : ControllerBase
    {
        private readonly ApplicationDbContext contexto;

        public PolizasController(ApplicationDbContext contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<List<Poliza>>> ObtenerTodasLasPolizas()
        {
            var polizas = await contexto.Polizas
                .Include(p => p.PolizasCoberturas)
                .ThenInclude(pc => pc.Cobertura)
                .ToListAsync();

            var polizasDTO = polizas.Select(p => new PolizaDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                PolizasCoberturas = p.PolizasCoberturas.Select(pc => new PolizasCoberturasDTO
                {
                    Id = pc.Id,
                    CoberturaId = pc.CoberturaId,
                    CoberturaNombre = pc.Cobertura.Nombre,
                    MontoAsegurado = pc.MontoAsegurado
                }).ToList(),
                MontoTotal = ObtenerMontoTotalPoliza(p)
            }).ToList();

            return Ok(polizasDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Poliza>> ObtenerPolizaPorId(int id)
        {
            var poliza = await contexto.Polizas
                .Include(p => p.PolizasCoberturas)
                .ThenInclude(pc => pc.Cobertura)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poliza == null)
            {
                return NotFound();
            }

            var polizaDTO = new PolizaDTO
            {
                Id = poliza.Id,
                Nombre = poliza.Nombre,
                PolizasCoberturas = poliza.PolizasCoberturas.Select(pc => new PolizasCoberturasDTO
                {
                    Id = pc.Id,
                    CoberturaId = pc.CoberturaId,
                    CoberturaNombre = pc.Cobertura.Nombre,
                    MontoAsegurado = pc.MontoAsegurado
                }).ToList(),
                MontoTotal = ObtenerMontoTotalPoliza(poliza)
            };

            return Ok(polizaDTO);
        }

        [HttpPost]
        public async Task<ActionResult> CrearPoliza(Poliza poliza)
        {
            var existePoliza = await contexto.Polizas.AnyAsync(p => p.Nombre == poliza.Nombre);
            if (existePoliza)
            {
                return BadRequest($"Ya existe poliza con nombre {poliza.Nombre}" );
            }
            contexto.Add(poliza);
            await contexto.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> ActualizarPoliza(Poliza poliza, int id)
        {
            if (id != poliza.Id)
            {
                return BadRequest("El id de la poliza no coincide con el id {id}");
            }

            var polizaAModificar = await contexto.Polizas
                .Include(p => p.PolizasCoberturas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (polizaAModificar == null)
            {
                return NotFound();
            }

            polizaAModificar.Nombre = poliza.Nombre;

            contexto.PolizasCoberturas.RemoveRange(polizaAModificar.PolizasCoberturas);

            foreach (var cobertura in poliza.PolizasCoberturas)
            {
                polizaAModificar.PolizasCoberturas.Add(cobertura);
            }

            await contexto.SaveChangesAsync();

            /*try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistePoliza(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarPoliza(int id)
        {
            if (!await ExistePoliza(id))
            {
                return NotFound();
            }
            contexto.Remove(new Poliza { Id = id });
            await contexto.SaveChangesAsync();
            return Ok();
        }

        private async Task<bool> ExistePoliza(int id)
        {
            return await contexto.Polizas.AnyAsync(e => e.Id == id);
        }

        static private decimal ObtenerMontoTotalPoliza(Poliza poliza)
        {
            decimal total = 0;
            foreach (PolizasCoberturas pc in poliza.PolizasCoberturas)
            {
                total += pc.MontoAsegurado;
            }
            return total;   
        }
    }
}

using AdminPolizasAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPolizasAPI.Controllers
{
    [ApiController]
    [Route("api/polizascoberturas")]
    public class PolizasCoberturasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PolizasCoberturasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /*[HttpGet]
        public async Task<ActionResult<List<Poliza>>> Get()
        {
            return await context.Polizas.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Poliza>> Get(int id)
        {
            var poliza = await context.Polizas.FirstOrDefaultAsync(x => x.Id == id);

            if (poliza == null)
            {
                return NotFound();
            }

            return poliza;
        }*/

        [HttpPost]
        public async Task<ActionResult> Post(PolizasCoberturas polizasCoberturas)
        {
            var existePoliza = await context.Polizas.AnyAsync(x => x.Id == polizasCoberturas.PolizaId);
            if (!existePoliza)
            {
                return BadRequest($"No existe poliza con ese id");
            }

            var existeCobertura = await context.Coberturas.AnyAsync(x => x.Id == polizasCoberturas.CoberturaId);
            if (!existeCobertura)
            {
                return BadRequest($"No existe cobertura con ese id");
            }
            using (var transaction = context.Database.BeginTransaction()) 
            {
                try
                {
                    context.Add(polizasCoberturas);

                    polizasCoberturas.Poliza.PolizasCoberturas.Add(polizasCoberturas);

                    polizasCoberturas.Cobertura.PolizasCoberturas.Add(polizasCoberturas);

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message );
                }
            }
            
            
        }

        /*[HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Poliza poliza, int id)
        {
            if (poliza.Id != id)
            {
                return BadRequest("El id de la poliza no coincide con el id enviado");
            }
            var existePoliza = await context.Polizas.AnyAsync(x => x.Id == id);

            if (!existePoliza)
            {
                return NotFound();
            }

            context.Update(poliza);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existePoliza = await context.Polizas.AnyAsync(x => x.Id == id);

            if (!existePoliza)
            {
                return NotFound();
            }
            context.Remove(new Poliza { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }*/
    }
}

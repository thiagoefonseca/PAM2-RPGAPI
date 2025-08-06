using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArmasController : ControllerBase
    {
        private readonly DataContext _context;

        public ArmasController(DataContext context)
        {
            _context = context;
        }

         [HttpGet("{id}")] //Buscar pelo id
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                Arma a = await _context.TB_ARMAS.FirstOrDefaultAsync(aBusca => aBusca.Id == id);
                //using Microsoft.EntityFrameworkCore;

                return Ok(a);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //using System.Collections.Generic;
                List<Arma> lista = await _context.TB_ARMAS.ToListAsync();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Arma novaArma)
        {
            try
            {               
                if(novaArma.Dano == 0)
                  throw new Exception("O Dano da arma não pode ser 0");

                Personagem? p = await _context.TB_PERSONAGENS.FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);
                
                if(p == null)
                    throw new Exception("Não existe personagem com o Id informado.");

                Arma buscaArma = await _context.TB_ARMAS
                    .FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

                if(buscaArma != null)
                    throw new Exception("O Personagem selecionado já contém uma arma atribuída a ele.");

                await _context.TB_ARMAS.AddAsync(novaArma);
                await _context.SaveChangesAsync();

                return Ok(novaArma.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Arma novaArma)
        {
            try
            {
                _context.TB_ARMAS.Update(novaArma);
                int linhaAfetadas = await _context.SaveChangesAsync();

                return Ok(linhaAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Arma aRemover = await _context.TB_ARMAS.FirstOrDefaultAsync(p => p.Id == id);

                _context.TB_ARMAS.Remove(aRemover);
                int linhaAfetadas = await _context.SaveChangesAsync();

                return Ok(linhaAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }
    }
}
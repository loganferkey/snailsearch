using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTO;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/relic")]
    [ApiController]
    public class RelicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RelicController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult GetRelics() 
        {
            // Grab all of the relics in the db and return them only if it works and is non zero
            var relics = _context.Relics.ToList().Select(r => r.ToRelicDto());
            if (relics == null)
                return NotFound();
            return Ok(relics);
        }

        [HttpGet("{keyword}")]
        public IActionResult GetRelicsByKeyword([FromRoute] string? keyword)
        {
            // Query relics for matching keywords
            // TODO: see if I can make something more performance optimized
            IQueryable<Relic> query = _context.Relics.Where(r => EF.Functions.Like(r.Name, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Special, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Description, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Skill1, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Skill2, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Skill3, $"%{keyword}%") ||
                                                   EF.Functions.Like(r.Source, $"%{keyword}%"));
            IEnumerable<Relic?> result = query.AsNoTracking().ToList();
            if (!result.Any() || result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
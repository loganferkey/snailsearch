using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll() 
        {
            // Grab all of the relics in the db and return them only if it works and is non zero
            List<Relic>? relics = _context.Relics.ToList();
            if (relics == null || relics.Count == 0)
                return NotFound();
            return Ok(relics);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int? id)
        {
            // Find a relic in the db by matching id and return it if it's not null
            Relic? relic = _context.Relics.Find(id);
            if (relic == null || id == null)
                return NotFound();
            return Ok(relic);
        }

        // [HttpGet("{keyword}")]
        // public IActionResult GetByKeyword([FromRoute] string keyword) 
        // {
        //     return Ok(1);
        // }
    }
}
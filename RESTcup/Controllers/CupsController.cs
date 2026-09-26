using Microsoft.AspNetCore.Mvc;
using RESTcup.Models;

namespace RESTcup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CupsController : ControllerBase
    {
        private ICupsRepository repo;

        public CupsController(ICupsRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/<CupsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Cup>> Get()
        {
            IEnumerable<Cup> cups = repo.GetCups();
            return Ok(cups);
        }

        // GET api/<CupsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cup> Get(int id)
        {
            var cup = repo.GetCupById(id);
            if (cup == null)
            {
                return NotFound();
            }
            return Ok(cup);
        }

        // POST api/<CupsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Cup> Post([FromBody] Cup cup)
        {
            Cup addedCup = repo.AddCup(cup);
            return CreatedAtAction(nameof(Get), new { id = addedCup.Id }, addedCup);
        }

        // PUT api/<CupsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cup> Put(int id, [FromBody] Cup updatedCup)
        {
            var cup = repo.UpdateCup(updatedCup);
            if (cup == null)
            {
                return NotFound();
            }
            return Ok(cup);
        }

        // DELETE api/<CupsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cup> Delete(int id)
        {
            var cup = repo.RemoveCup(id);
            if (cup == null)
            {
                return NotFound();
            }
            return Ok(cup);
        }
    }
}

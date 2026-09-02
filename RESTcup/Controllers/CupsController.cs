using Microsoft.AspNetCore.Mvc;
using RESTcup.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTcup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CupsController : ControllerBase
    {
        private CupsRepository repository;

        public CupsController(CupsRepository repo)
        {
            repository = repo;
        }

        // GET: api/<CupsController>
        [HttpGet]
        public IEnumerable<Cup> Get()
        {
            return repository.Get();
        }

        // GET api/<CupsController>/5
        [HttpGet("{id}")]
        public Cup Get(int id)
        {
            return repository.GetById(id);
        }

        // POST api/<CupsController>
        [HttpPost]
        public Cup Post([FromBody] Cup cup)
        {
            return repository.Add(cup);
        }

        // PUT api/<CupsController>/5
        [HttpPut("{id}")]
        public Cup Put(int id, [FromBody] Cup updatedCup)
        {
            return repository.Update(id, updatedCup);
        }

        // DELETE api/<CupsController>/5
        [HttpDelete("{id}")]
        public Cup Delete(int id)
        {
            return repository.Delete(id);
        }
    }
}

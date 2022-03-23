using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Service;

namespace ProjetoFinal.Controllers
{
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService service;

        public PublicationController(IPublicationService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<Publication> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            Publication? pub = service.GetById(id);
            if(pub == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pub);
            }
        }


        [HttpPost("/create")]
        public IActionResult Create(Publication publication)
        {
            if (publication != null)
            {
                Publication newPub = service.Create(publication);
                return CreatedAtRoute(new { IdPub = newPub.IdPub }, newPub);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut ("{id}", Name = "Edit")]
        public IActionResult Edit(int id, Publication publication)
        {
            var pubToUpdate = service.GetById(id);
            if (pubToUpdate is not null && publication is not null)
            {
                service.Edit(id, publication);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete ("{id}", Name = "Delete")]
        public IActionResult Delete(int id)
        {
            Publication pubToDelete = service.GetById(id);
            if(pubToDelete != null)
            {
                service.Delete(pubToDelete);
                return Ok("Foi eliminado");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        
        public async Task<IActionResult> SignUp(User user)
        {

        }
    }
}

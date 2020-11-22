using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data.Entities;
using WebShop.Data.Interfaces;
using WebShop.Data.Repos;

namespace WebShop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepo;

        public ClientsController(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(Client client)
        {
            var affectedRows = await _clientRepo.Add(client);
            return affectedRows;
        }

        [HttpGet("{id}")]
        public async Task<Client> Get(string id)
        {
            var client = await _clientRepo.Get(id);
            return client;
        }

        [HttpGet]
        public async Task<IEnumerable<Client>> GetAll()
        {
            var clients = await _clientRepo.GetAll();
            return clients;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Client client)
        {
            var affectedRows = await _clientRepo.Update(client);
            return affectedRows;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Remove(string id)
        {
            var affectedRows = await _clientRepo.Remove(id);
            return affectedRows;
        }
    }
}

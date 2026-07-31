using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiMatheusProjetoFinal.Models;

namespace ApiMatheusProjetoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "matheus", Password = "12345" },
            new User { Id = 2, Username = "admin", Password = "admin123" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { Message = "Utilizador não encontrado" });

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User newUser)
        {
            if (_users.Any(u => u.Username == newUser.Username))
                return Conflict(new { Message = "Já existe um utilizador com esse username" });

            newUser.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(newUser);

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { Message = "Utilizador não encontrado" });

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { Message = "Utilizador não encontrado" });

            _users.Remove(user);
            return NoContent();
        }
    }
}
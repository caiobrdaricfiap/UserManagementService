using Microsoft.AspNetCore.Mvc;
using UserManagementService.Models.User;
using System.Collections.Generic;
using System.Linq;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<UserModel> _users = new List<UserModel>();

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"Usuário com ID {id} não encontrado.");

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("Usuário não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("Nome e Email são obrigatórios.");

            if (string.IsNullOrWhiteSpace(user.RawPassword))
                return BadRequest("Senha é obrigatória.");

            try
            {
                user.SetPassword(user.RawPassword);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            var brazilTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            user.CreatedAt = brazilTime;

            user.Id = _users.Count + 1;
            _users.Add(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"Usuário com ID {id} não encontrado.");

            user.Name = updatedUser.Name ?? user.Name;
            user.Email = updatedUser.Email ?? user.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.RawPassword))
            {
                try
                {
                    user.SetPassword(updatedUser.RawPassword);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var brazilTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            user.UpdatedAt = brazilTime;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"Usuário com ID {id} não encontrado.");

            _users.Remove(user);
            return NoContent();
        }

        [HttpPost("activate/{id}")]
        public IActionResult ActivateUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"Usuário com ID {id} não encontrado.");

            user.IsActive = true;
            return Ok(user);
        }

        [HttpPost("deactivate/{id}")]
        public IActionResult DeactivateUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"Usuário com ID {id} não encontrado.");

            user.IsActive = false;
            return Ok(user);
        }
    }
}

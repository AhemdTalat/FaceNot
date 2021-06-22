using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _Dbcontext;
        public UsersController(DataContext Dbcontext)
        {
            _Dbcontext = Dbcontext;
        }

        [HttpGet]
        [Authorize]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() => await _Dbcontext.Users.ToListAsync();

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id) => await _Dbcontext.Users.FindAsync(id);


    }
}
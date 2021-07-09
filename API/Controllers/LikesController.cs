using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var user = await _unitOfWork.LikesRepository.GetUserWithLikes(User.GetUserId());
            var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (likedUser == null) return NotFound();

            if (likedUser.UserName == user.UserName) return BadRequest("You cannot like yourself");


            var like = await _unitOfWork.LikesRepository.GetUserLike(user.Id, likedUser.Id);

            if (like != null)
            {
                user.LikedUsers.Remove(like);
            }
            else
            {
                like = new UserLike
                {
                    SourceUserId = user.Id,
                    LikedUserId = likedUser.Id
                };

                user.LikedUsers.Add(like);
            }


            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikedUsers([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}
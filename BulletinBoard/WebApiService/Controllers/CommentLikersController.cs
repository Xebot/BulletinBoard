using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.DTO;
using AppServices.Interfaces;

namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikersController : ControllerBase
    {
        public CommentLikersController(ICommentLikerService _commentLikerService)
        {
            commentLikerService = _commentLikerService;
        }
        protected readonly ICommentLikerService commentLikerService;

        // GET api/values
        [HttpGet("add-like/{commentId}/{userId}")]
        public ActionResult<CommentDto> AddLike(int commentId, Guid userId)
        {        
            return Ok(commentLikerService.AddLike(commentId, userId));
        }

        // GET api/values
        [HttpGet("delete-like/{commentId}/{userId}")]
        public ActionResult<CommentDto> DeleteLike(int commentId, Guid userId)
        {
            return Ok(commentLikerService.DeleteLike(commentId, userId));
        }
    }
}

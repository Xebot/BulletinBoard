using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.DTO;
using AppServices.Interfaces;

namespace WebApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        public CommentsController(ICommentService _commentService)
        {
            commentService = _commentService;
        }
        protected readonly ICommentService commentService;

        // GET api/values
        [HttpGet("all")]
        public ActionResult<List<CommentDto>> GetAll()
        {
            IList<CommentDto> categories = commentService.GetAll();
            return Ok(categories);
        }

        // GET api/values
        [HttpGet("get/{id}")]
        public ActionResult<CommentDto> Get(int id)
        {
            CommentDto comment = commentService.Get(id);
            return Ok(comment);
        }

        // GET api/values
        [HttpGet("get-advert-comments/{id}/{showedAdvertsNumber}/{addingAdvertsNumber}")]
        public ActionResult<IList<CommentDto>> GetAdvertComments(Guid id, int showedAdvertsNumber, int addingAdvertsNumber)
        {
            IList<CommentDto> comment = commentService.GetAdvertComments(id, showedAdvertsNumber, addingAdvertsNumber);
            return Ok(comment);
        }

        // GET api/values
        [HttpGet("get-advert-comments-number/{id}")]
        public ActionResult<int> GetAdvertCommentsNumber(Guid id)
        {
            int commentsNumber = commentService.GetAdvertCommentsNumber(id);
            return Ok(commentsNumber);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<int> CreateComment([FromBody] CommentDto newComment)
        {
            commentService.CreateComment(newComment);
            return Ok(newComment.Id);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public ActionResult DeleteComment(int id)
        {
            commentService.DeleteComment(id);
            return Ok();
        }

        [HttpGet]
        [Route("get-new-comments-information/{userId}/{lastActionPublicationTime}")]
        public ActionResult<List<CommentInformMassege>> GetNewCommentsInformation(Guid userId, DateTime lastActionPublicationTime)
        {
            List<CommentInformMassege> commentInformMasseges = commentService.GetNewCommentsInformation(userId, lastActionPublicationTime);
            return Ok(commentInformMasseges);
        }

    }
}

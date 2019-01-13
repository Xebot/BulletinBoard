using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using System.Net.Http;
using WebApi.Contracts.DTO;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using AppServices.Interfaces;
using Authentication.AppServices.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;

namespace WebMVC.Controllers
{
    public class AdvertController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAdvertService _advertService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public AdvertController(IHostingEnvironment hostingEnvironment, IAdvertService advertService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _advertService = advertService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
            _client = new HttpClient();
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetNewCommentsInformation(string lastPublicationTime)
        {
            var a = lastPublicationTime;
            List<CommentInformMassege> commentInformMasseges = null;
            var context = _contextAccessor.HttpContext;
            Guid userId = new Guid();
            if (context.User.GetUserId() != null)
            {
                userId = (Guid)context.User.GetUserId();
            }
            using (var httpClient = new HttpClient())
            {
                var url = GetAbsolutePath("comments", "get-new-comments-information");
                HttpResponseMessage response = httpClient.GetAsync(url + userId + "/" + lastPublicationTime).Result;
                if (response.IsSuccessStatusCode)
                {
                    commentInformMasseges = response.Content.ReadAsAsync<List<CommentInformMassege>>().Result;
                }                
                return new JsonResult(commentInformMasseges);
            }
        }

        public IActionResult AdvertPage(Guid? id)
        {
            AdvertDto advert = null;
            int showedCommentsNumber = 0;
            int addingCommentsNumber = 5;
            int commentsNumber = 0;
            var url = GetAbsolutePath("adverts","adverts");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    advert = response.Content.ReadAsAsync<AdvertDto>().Result;
                }
                url = GetAbsolutePath("comments", "get-advert-comments");
                response = httpClient.GetAsync(url + id.ToString() + "/" + showedCommentsNumber.ToString() + "/" + addingCommentsNumber.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    advert.Comments = response.Content.ReadAsAsync<List<CommentDto>>().Result;
                }
                url = GetAbsolutePath("comments", "get-advert-comments-number");
                response = httpClient.GetAsync(url + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    commentsNumber = response.Content.ReadAsAsync<int>().Result;
                }
            }
            var context = _contextAccessor.HttpContext;          
            AdvertViewModel viewModel = new AdvertViewModel { advert = advert, commentsNumber = commentsNumber, addingCommentsNumber = addingCommentsNumber };                        
            if (context.User.GetUserId()!= null)
            {
                viewModel.currentUserId = (Guid)context.User.GetUserId();
            }            
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddCommentAction(CommentViewModel addedComment)
        {
            var context = _contextAccessor.HttpContext;            
            addedComment.UserId = (Guid)context.User.GetUserId();
            addedComment.UserName = context.User.GetUserFIO();
            addedComment.PublicationDate = DateTime.UtcNow;
            Guid advertId = addedComment.AdvertId;
            var url = GetAbsolutePath("comments","add");
            //Передача объекта
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PostAsJsonAsync<CommentViewModel>(url, addedComment).Result;
            }
            return Redirect("/Advert/AdvertPage/" + advertId);
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteCommentAction(int id)
        {
            var url = GetAbsolutePath("comments","get");
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url + id.ToString()).Result;
                Guid advertId = new Guid();
                CommentDto comment = new CommentDto();
                if (response.IsSuccessStatusCode)
                {
                    comment = response.Content.ReadAsAsync<CommentDto>().Result;
                }
                if (comment.UserId == (Guid)_contextAccessor.HttpContext.User.GetUserId())
                {                    
                    url = GetAbsolutePath("comments", "delete");
                    response = httpClient.GetAsync(url + id.ToString()).Result;
                }
                advertId = comment.AdvertId;
                return Redirect("/Advert/AdvertPage/" + advertId.ToString());
            }
        }

        [HttpGet]
        public JsonResult ShowMoreComments(string advertId, int showedAdvertsNumber = 0, int addingAdvertsNumber = 5)
        {
            List<CommentDto> comments = null;
            using (var httpClient = new HttpClient())
            {
                int commentsNumber = 0;
                var url = GetAbsolutePath("comments", "get-advert-comments-number");
                // получение количества комментариев к объявлению
                HttpResponseMessage response = httpClient.GetAsync(url + advertId).Result;
                if (response.IsSuccessStatusCode)
                {
                    commentsNumber = response.Content.ReadAsAsync<int>().Result;
                }
                // получение списка комментариев к объявлению
                url = GetAbsolutePath("comments", "get-advert-comments");
                response = httpClient.GetAsync(url + advertId + "/" + showedAdvertsNumber + "/" + addingAdvertsNumber).Result;
                if (response.IsSuccessStatusCode)
                {
                    comments = response.Content.ReadAsAsync<List<CommentDto>>().Result;
                }
                // приведение списка комментариев к типу CommentViewModel
                List<CommentViewModel> parsedComments = new List<CommentViewModel>();
                int i = 0;
                foreach (var comment in comments)
                {
                    CommentViewModel parsedComment = new CommentViewModel();
                    i++;
                    parsedComment.Id = comment.Id;
                    parsedComment.UserName = comment.UserName;
                    parsedComment.ImageUrl = "/images/avatar.jpg";
                    parsedComment.AdvertId = comment.AdvertId;
                    parsedComment.CommentText = comment.CommentText;
                    parsedComment.LocalDate = comment.PublicationDate.ToLocalTime().ToLongDateString();
                    parsedComment.LocalTime = comment.PublicationDate.ToLocalTime().ToShortTimeString();
                    parsedComment.IsLastComment = (showedAdvertsNumber + i >= commentsNumber) ? true : false;
                    parsedComment.CommentLikersNumber = comment.CommentLikersNumber;
                    if (_contextAccessor.HttpContext.User.GetUserId() != null)
                    {
                        parsedComment.IsCurrentUserComment = comment.UserId == (Guid)_contextAccessor.HttpContext.User.GetUserId() ? true : false;
                        foreach (var commentLiker in comment.CommentLikers)
                        {
                            if (commentLiker.UserId == (Guid)_contextAccessor.HttpContext.User.GetUserId())
                            {
                                parsedComment.IsLikedByUser = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        parsedComment.IsCurrentUserComment = false;
                        parsedComment.IsLikedByUser = false;
                    }
                    parsedComment.IsAuthorizedCurrentUser = User.Identity.IsAuthenticated;

                    parsedComments.Add(parsedComment);
                    parsedComment.CommentReplies = new List<CommentViewModel>();
                    if (comment.CommentReplies != null)
                    {
                        foreach (var commentReply in comment.CommentReplies)
                        {
                            CommentViewModel reply = new CommentViewModel();
                            reply.Id = commentReply.Id;
                            reply.UserName = commentReply.UserName;
                            reply.ImageUrl = "/images/avatar.jpg";
                            reply.AdvertId = commentReply.AdvertId;
                            reply.CommentText = commentReply.CommentText;
                            reply.LocalDate = commentReply.PublicationDate.ToLocalTime().ToLongDateString();
                            reply.LocalTime = commentReply.PublicationDate.ToLocalTime().ToShortTimeString();
                            reply.CommentLikersNumber = commentReply.CommentLikersNumber;
                            if (_contextAccessor.HttpContext.User.GetUserId() != null)
                            {
                                reply.IsCurrentUserComment = commentReply.UserId == (Guid)_contextAccessor.HttpContext.User.GetUserId() ? true : false;
                                foreach (var commentLiker in commentReply.CommentLikers)
                                {
                                    if (commentLiker.UserId == (Guid)_contextAccessor.HttpContext.User.GetUserId())
                                    {
                                        reply.IsLikedByUser = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                reply.IsCurrentUserComment = false;
                                parsedComment.IsLikedByUser = false;
                            }
                            reply.IsAuthorizedCurrentUser = User.Identity.IsAuthenticated;
                            parsedComment.CommentReplies.Add(reply);
                        }
                    }
                }
                return new JsonResult(parsedComments);
            }
        }

        [HttpGet]
        [Authorize]
        public JsonResult AddLike(int commentId)
        {
            using (var httpClient = new HttpClient())
            {
                Guid userId = (Guid)_contextAccessor.HttpContext.User.GetUserId();
                bool result = false;
                // получение количества комментариев к объявлению
                var url = string.Format(GetAbsolutePath("commentLikers", "add-like"), commentId, userId);
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }                
                return new JsonResult(result);
            }
        }

        [HttpGet]
        [Authorize]
        public JsonResult DeleteLike(int commentId)
        {
            using (var httpClient = new HttpClient())
            {
                Guid userId = (Guid)_contextAccessor.HttpContext.User.GetUserId();
                bool result = false;
                var url = string.Format(GetAbsolutePath("commentLikers","delete-like"),commentId,userId);
                // получение количества комментариев к объявлению
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<bool>().Result;
                }
                return new JsonResult(result);
            }
        }

        [Authorize]
        public IActionResult AddAdvert()
        {
            using (var httpClient = new HttpClient())
            {
                AddAdvertViewModel viewModel = new AddAdvertViewModel();
                viewModel.isNotValidAdvert = false;
                HttpResponseMessage response;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                //Получение списка категорий 
                var url = string.Format(GetAbsolutePath("Categories", "categories-dictionary"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                }
                //Получение словаря регионов (Id, Name) 
                url = string.Format(GetAbsolutePath("Regions", "GetRegions"),cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
                
                var context = _contextAccessor.HttpContext;
                var user = new UserDto();
                viewModel.UserId = (Guid)context.User.GetUserId();
                url = string.Format(GetAbsolutePath("user", "GetUser"), viewModel.UserId);
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetToken()}");
                response = httpClient.GetAsync(url).Result;                    
                if (response.IsSuccessStatusCode)
                {
                    var answer = response.Content.ReadAsAsync<ApiResult<UserDto>>().Result;
                    user = answer.Result;
                }       
                viewModel.UserName = user.FIO;
                viewModel.Phone = Convert.ToInt64(user.UserTel).ToString("+#(###)###-##-##");
                viewModel.Email = context.User.GetUserEmail();                
                return View(viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult UnvalidAddAdvert(AddAdvertModel incorrectAdvertInformation)
        {
            using (var httpClient = new HttpClient())
            {
                AddAdvertViewModel viewModel = new AddAdvertViewModel();
                viewModel.RegionId = incorrectAdvertInformation.RegionId;
                viewModel.CategoryId = incorrectAdvertInformation.CategoryId;
                viewModel.Title = incorrectAdvertInformation.Title;
                viewModel.Address = incorrectAdvertInformation.Address;
                viewModel.AdvertPrimaryImage = incorrectAdvertInformation.AdvertPrimaryImage;                                
                viewModel.Price = incorrectAdvertInformation.Price;
                viewModel.ShortDescription = incorrectAdvertInformation.ShortDescription;
                viewModel.Description = incorrectAdvertInformation.Description;
                viewModel.isNotValidAdvert = true;
                HttpResponseMessage response;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                //Получение списка категорий 
                var url = string.Format(GetAbsolutePath("Categories", "categories-dictionary"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                }
                //Получение словаря регионов (Id, Name)     
                url = string.Format(GetAbsolutePath("Regions", "GetRegions"),cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
                var context = _contextAccessor.HttpContext;
                viewModel.UserName = context.User.GetUserFIO();
                viewModel.Phone = context.User.GetUserPhone();
                viewModel.Email = context.User.GetUserEmail();
                viewModel.UserId = (Guid)context.User.GetUserId();
                return View("AddAdvert", viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAdvert(AddAdvertModel addedAdvertInformation, IFormFile primaryFile, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // Передача данных в объект типа AdvertDto
                AdvertDto addingAdvert = new AdvertDto { };

                Guid id = Guid.NewGuid();
                addingAdvert.Id = id;
                addingAdvert.PublicationDate = DateTime.UtcNow;
                addingAdvert.Status = "Published";
                addingAdvert.Title = addedAdvertInformation.Title;
                addingAdvert.ShortDescription = addedAdvertInformation.ShortDescription;
                addingAdvert.Description = addedAdvertInformation.Description;
                addingAdvert.UserName = addedAdvertInformation.UserName;
                addingAdvert.Address = addedAdvertInformation.Address;
                addingAdvert.Phone = addedAdvertInformation.Phone;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                if (cultureName == "ru")
                {                    
                    addingAdvert.Price = decimal.Parse(addedAdvertInformation.Price.Replace('.', ','));
                }
                if (cultureName == "en")
                {
                    addingAdvert.Price = decimal.Parse(addedAdvertInformation.Price.Replace(',', '.'));
                }
                addingAdvert.CategoryId = addedAdvertInformation.CategoryId;
                addingAdvert.RegionId = addedAdvertInformation.RegionId;

                var context = _contextAccessor.HttpContext;
                addingAdvert.UserId = (Guid)context.User.GetUserId();

                //addingAdvert.UserId = addedAdvertInformation.UserId;
                addingAdvert.Email = addedAdvertInformation.Email;
                // Получение пути к папке объявления
                string webRootPath = _hostingEnvironment.WebRootPath;
                string imagesDirectoryName = Translit(addedAdvertInformation.Title);
                addingAdvert.FolderName = imagesDirectoryName + "-" + id;
                string fullPath = webRootPath + "/images/adverts-images/" + imagesDirectoryName + "-" + id;
                // Создать каталог в случае его отсутствия
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                // Добавление главной картинки
                IFormFile primaryImage = addedAdvertInformation.AdvertPrimaryImage;
                if (primaryImage != null)
                {
                    using (var fileStream = new FileStream(fullPath + "/main-" + primaryImage.FileName, FileMode.Create))
                    {
                        primaryImage.CopyTo(fileStream);
                    }
                    string primaryImageUrl = imagesDirectoryName + "-" + id + "/main-" + primaryImage.FileName.ToString();
                    addingAdvert.PrimaryImageUrl = primaryImageUrl;
                }
                // Добавление дополнительных изображений
                List<IFormFile> images = addedAdvertInformation.AdvertImages;
                string imagesUrl = "";
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        string imageGuid = Guid.NewGuid().ToString();
                        string path = fullPath + "/" + imageGuid + "-" + image.FileName;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                        imagesUrl += imagesDirectoryName + "-" + id + "/" + imageGuid + "-" + image.FileName.ToString() + "||";
                    }
                }
                addingAdvert.ImagesUrl = imagesUrl;
                // Передача объекта
                var url = GetAbsolutePath("adverts", "add");
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.PostAsJsonAsync<AdvertDto>(url, addingAdvert).Result;
                }
                return Redirect("/advert/" + id.ToString());
            }
            else {
                return UnvalidAddAdvert(addedAdvertInformation);
            }
        }

        [Authorize]
        public IActionResult EditAdvert(Guid id)
        {
            var context = _contextAccessor.HttpContext;
            
            using (var httpClient = new HttpClient())
            {
                EditAdvertViewModel viewModel = new EditAdvertViewModel();                
                // Получение данных объявления    
                AdvertDto Advert = new AdvertDto { };
                var url = GetAbsolutePath("adverts","adverts");
                HttpResponseMessage response = httpClient.GetAsync(url + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    Advert = response.Content.ReadAsAsync<AdvertDto>().Result;
                }            
                viewModel.Id = id;
                viewModel.Phone = Advert.Phone;
                viewModel.UserName = Advert.UserName;
                viewModel.Email = context.User.GetUserEmail();
                viewModel.Status = Advert.Status;
                viewModel.RegionId = Advert.RegionId;
                viewModel.CategoryId = Advert.CategoryId;
                viewModel.Title = Advert.Title;
                viewModel.Address = Advert.Address;
                viewModel.Price = Advert.Price.ToString();
                viewModel.ShortDescription = Advert.ShortDescription;
                viewModel.Description = Advert.Description;
                viewModel.PrimaryImageUrl = Advert.PrimaryImageUrl;
                viewModel.ImagesUrl = Advert.ImagesUrl;
                if (context.User.GetUserId() != Advert.UserId)
                {
                    return RedirectToAction("SignIn");
                }
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                //Получение списка категорий 
                url = string.Format(GetAbsolutePath("Categories", "categories-dictionary"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                }
                //Получение словаря регионов (Id, Name)        
                url = string.Format(GetAbsolutePath("Regions", "GetRegions"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
                return View(viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditAdvertError(EditAdvertModel incorrectAdvertInformation)
        {
            using (var httpClient = new HttpClient())
            {
                EditAdvertViewModel viewModel = new EditAdvertViewModel();
                Guid id = new Guid(incorrectAdvertInformation.Id);
                string a = incorrectAdvertInformation.Title;
                // Получение данных объявления    
                AdvertDto Advert = new AdvertDto { };
                var url = GetAbsolutePath("adverts","adverts");
                HttpResponseMessage response = httpClient.GetAsync(url + id.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    Advert = response.Content.ReadAsAsync<AdvertDto>().Result;
                }
                viewModel.Id = id;
                viewModel.Phone = incorrectAdvertInformation.Phone;
                viewModel.UserName = incorrectAdvertInformation.UserName;
                viewModel.Email = incorrectAdvertInformation.Email;
                viewModel.Status = incorrectAdvertInformation.Status;
                viewModel.RegionId = incorrectAdvertInformation.RegionId;
                viewModel.CategoryId = incorrectAdvertInformation.CategoryId;
                viewModel.Title = incorrectAdvertInformation.Title;
                viewModel.Address = incorrectAdvertInformation.Address;
                viewModel.Price = incorrectAdvertInformation.Price;
                viewModel.ShortDescription = incorrectAdvertInformation.ShortDescription;
                viewModel.Description = incorrectAdvertInformation.Description;
                viewModel.PrimaryImageUrl = Advert.PrimaryImageUrl;
                viewModel.ImagesUrl = Advert.ImagesUrl;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                //Получение списка категорий 
                url = string.Format(GetAbsolutePath("Categories", "categories-dictionary"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.CategoriesListWithSubcategories = response.Content.ReadAsAsync<List<CategorySubcategories>>().Result;
                }
                //Получение словаря регионов (Id, Name) 
                url = string.Format(GetAbsolutePath("Regions", "GetRegions"), cultureName);
                response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    viewModel.Regions = response.Content.ReadAsAsync<Dictionary<int, string>>().Result;
                }
                return View("EditAdvert", viewModel);
            }            
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> EditAdvert(EditAdvertModel editedAdvertInformation, IFormFile primaryFile, List<IFormFile> files)
        {             
            // проверка на принадлежность данного объявления пользователю
            List<AdvertDto> adverts = null;
            UserAdvertsDto ads = new UserAdvertsDto();
            var context = _contextAccessor.HttpContext;
            var token = context.User.GetAuthToken();
            Guid id = (Guid)context.User.GetUserId();
            var url = string.Format(GetAbsolutePath("adverts", "UserAdverts"), id,0);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (var response = await _client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    ads = response.Content.ReadAsAsync<UserAdvertsDto>().Result;
                    adverts = ads.ads;
                }
            };                                 
            var c = adverts;
            Guid editedAdvertId = new Guid(editedAdvertInformation.Id);
            if (!adverts.Select(x => x.Id).Contains(editedAdvertId)) {                
                return View("AdvertError");
            }
            //паредача данных в БД в случае валидность данных
            if (ModelState.IsValid)
            {
                // Получение данных объявления из БД
                AdvertDto advert = null;
                url = GetAbsolutePath("adverts", "Adverts");
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.GetAsync(url + editedAdvertInformation.Id.ToString()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        advert = response.Content.ReadAsAsync<AdvertDto>().Result;
                    }
                }
                // Обновление данных
                advert.Status = editedAdvertInformation.Status;
                advert.Title = editedAdvertInformation.Title;
                advert.ShortDescription = editedAdvertInformation.ShortDescription;
                advert.Description = editedAdvertInformation.Description;
                advert.Address = editedAdvertInformation.Address;
                advert.Phone = editedAdvertInformation.Phone;
                string cultureName = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
                if (cultureName == "ru")
                {
                    advert.Price = decimal.Parse(editedAdvertInformation.Price.Replace('.', ','));
                }
                if (cultureName == "en")
                {
                    advert.Price = decimal.Parse(editedAdvertInformation.Price.Replace(',', '.'));
                }
                advert.CategoryId = editedAdvertInformation.CategoryId;
                advert.RegionId = editedAdvertInformation.RegionId;

                // Получение пути к папке объявления
                string imagesFolderName = advert.FolderName;
                string webRootPath = _hostingEnvironment.WebRootPath;
                string fullPath = webRootPath + "/images/adverts-images/" + imagesFolderName + "-" + editedAdvertInformation.Id;
                // Создать каталог в случае его отсутствия
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                // Изменение главной картинки
                if (editedAdvertInformation.AdvertPrimaryImage != null)
                {
                    IFormFile primaryImage = editedAdvertInformation.AdvertPrimaryImage;
                    using (var fileStream = new FileStream(fullPath + "/main-" + primaryImage.FileName, FileMode.Create))
                    {
                        primaryImage.CopyTo(fileStream);
                    }
                    string primaryImageUrl = imagesFolderName + "-" + editedAdvertInformation.Id + "/main-" + primaryImage.FileName.ToString();
                    advert.PrimaryImageUrl = primaryImageUrl;
                }
                // Удаление дополнительных картинок
                string imagesUrl = advert.ImagesUrl;
                if (editedAdvertInformation.DeletedAdvertImages != null)
                {
                    string[] deletedAdvertImagesUrl = editedAdvertInformation.DeletedAdvertImages.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var deletedAdvertImageUrl in deletedAdvertImagesUrl)
                    {
                        int index = imagesUrl.IndexOf(deletedAdvertImageUrl);
                        int separatorLength = 2;//length of ||
                        imagesUrl = (index < 0) ? imagesUrl : imagesUrl.Remove(index, deletedAdvertImageUrl.Length + separatorLength);
                    }
                }
                // Добавление новых дополнительных изображений
                if (editedAdvertInformation.AdvertImages != null)
                {
                    List<IFormFile> images = editedAdvertInformation.AdvertImages;
                    if (images != null)
                    {
                        foreach (var image in images)
                        {
                            string imageGuid = Guid.NewGuid().ToString();
                            string path = fullPath + "/" + imageGuid + "-" + image.FileName;
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                image.CopyTo(fileStream);
                            }
                            imagesUrl += imagesFolderName + "-" + advert.Id + "/" + imageGuid + "-" + image.FileName.ToString() + "||";
                        }
                    }
                }
                advert.ImagesUrl = imagesUrl;
                // Передача объекта
                url = GetAbsolutePath("adverts","save");
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = httpClient.PostAsJsonAsync<AdvertDto>(url, advert).Result;
                }
                return Redirect("/advert/" + advert.Id);
            }
            else
            {
                return EditAdvertError(editedAdvertInformation);
            }
        }

        public string Translit(string str)
        {
            string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "'", "Y", "", "E", "Yu", "Ya", "-" };
            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "'", "y", "", "e", "yu", "ya", "-" };
            string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", " " };
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", " " };
            for (int i = 0; i <= 33; i++)
            {
                str = str.Replace(rus_up[i], lat_up[i]);
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private string GetAbsolutePath(string controller, string methodName)
        {
            return $"{_configuration["ServiceApi:BaseUrl"]}{_configuration[$"ServiceApi:Areas:{controller}:{methodName}"]}";
        }
        private string GetToken()
        {
            var context = _contextAccessor.HttpContext;
            return context.User.GetAuthToken();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using NewsPortal.Data.Entities;
using NewsPortal.Domain.Interfaces;
using NewsPortal.Domain.Responses;
using NewsPortal.Web.Models.ArticleViewModels;
using NewsPortal.Web.Models.NewArticleViewModel;

namespace NewsPortal.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleManager _articleManager;

        public ArticleController(IArticleManager articleManager)
        {
            _articleManager = articleManager;
        }
        // GET: Article
        public ActionResult Index(int id)
        {
            var article = Mapper.Map<ArticleDetailViewModel>(_articleManager.GetArticle(id));
            return View(article);
        }
        // Post: Article/Like
        [HttpPost]
        public ActionResult Like(int id)
        {
            var user = (User)Session["LoggedInUser"];
            var article = new Article() { Id = id };
            ArticlePublishResponse response = _articleManager.LikeArticle(user, article);
            return Json(response.Success);
        }

        public ActionResult Manage()
        {
            var model = new ArticleViewModel();
            model.Authors = Mapper.Map<IList<Author>, IList<Models.NewArticleViewModel.AuthorViewModel>>(_articleManager.GetAllAuthors());
            return View(model);
        }

        public ActionResult Add(ArticleViewModel model)
        {
            var article = Mapper.Map<ArticleViewModel, Article>(model);
            article.PublishDate = DateTime.Now;
            var response = _articleManager.Publish((User)Session["LoggedInUser"], article);
            if (response.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View("Manage", model);
        }

        [HttpPost]
        public JsonResult ChartData()
        {
            var articleViewModel = Mapper.Map<IList<Article>, IList<ArticleStatsViewMode>>(_articleManager.GetArticleStats());
            return Json(articleViewModel);
        }
    }
}
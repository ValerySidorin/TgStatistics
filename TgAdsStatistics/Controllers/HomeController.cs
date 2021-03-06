﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TgAdsStatistics.Models;
using TgAdsStatistics.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using TgAdsStatistics.Logger;

namespace TgAdsStatistics.Controllers
{
    [Authorize]
    [ResponseCache(CacheProfileName = "Caching")]
    public class HomeController : Controller
    {
        private readonly CustomLoggerManager customLoggerManager;
        private readonly ApplicationContext db;
        private IMemoryCache cache;

        public HomeController(CustomLoggerManager customLoggerManager, ApplicationContext db, IMemoryCache memoryCache)
        {
            this.customLoggerManager = customLoggerManager;
            this.db = db;
            cache = memoryCache;
        }

        [HttpGet]
        public IActionResult Posts()
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var posts = db.Posts.Include(p => p.Channel);
            Post p = null;
            foreach (var post in posts)
            {
                p = post;
                if (cache.TryGetValue(post.Id, out p))
                    cache.Set(p.Id, post, TimeSpan.FromMinutes(5));
            }
            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            PostViewModel model = new PostViewModel
            {
                Channels = new SelectList(db.Channels, "Id", "ChannelName")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                Channel channel = await db.Channels.FirstOrDefaultAsync(c => c.Id == model.ChannelId);
                Post post = new Post { Date = DateTime.Now.ToString(), Views = model.Views, Subscribers = model.Subscribers, Cost = model.Cost, ChannelId = model.ChannelId, Channel = channel };
                post.Convercy = (post.Subscribers == 0) ? 0 : (post.Views / post.Subscribers);
                post.SingleSubscriberCost = (post.Subscribers == 0) ? 0 : (post.Cost / post.Subscribers);
                db.Posts.Add(post);
                int operations = await db.SaveChangesAsync();
                if (operations > 0)
                {
                    cache.Set(post.Id, post, TimeSpan.FromMinutes(5));
                }

                channel.Form(post);
                db.Channels.Update(channel);
                await db.SaveChangesAsync();

                return RedirectToAction("Posts");
            }
            else
            {
                return View(model);
            }
        }



        [HttpDelete]
        public async Task<IActionResult> DeletePost(int? id)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (id != null)
            {
                Post post = new Post { Id = id.Value };
                db.Entry(post).State = EntityState.Deleted;
                int operations = await db.SaveChangesAsync();
                if (operations > 0)
                {
                    if (cache.TryGetValue(post.Id, out post))
                    {
                        cache.Remove(post.Id);
                    }
                }
                return RedirectToAction("Posts");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreateChannel()
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(ChannelViewModel model)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                Channel channel = new Channel(model.ChannelName, 0, 0, 0, 0);
                db.Channels.Add(channel);
                await db.SaveChangesAsync();
                return RedirectToAction("Posts");
            }
            else
            {
                return View(model);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChannel(int? id)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (id != null)
            {
                Channel channel = new Channel(id.Value);

                db.Entry(channel).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Posts");
            }
            return NotFound();
        }
    }
}

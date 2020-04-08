using System;
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

namespace TgAdsStatistics.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationContext db;
        ILoggerFactory loggerFactory = LoggerFactory.Create(options =>
        {
            options.AddConsole();
        });
        ILogger logger;

        public HomeController(ApplicationContext context)
        {
            db = context;
            loggerFactory.AddFile("logger.txt");
            logger = loggerFactory.CreateLogger<HomeController>();
        }

        [HttpGet]
        public IActionResult Posts()
        {
            Log();
            var posts = db.Posts.Include(p => p.Channel);
            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            Log();
            PostViewModel model = new PostViewModel
            {
                Channels = new SelectList(db.Channels, "Id", "ChannelName")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
            Log();
            if (ModelState.IsValid)
            {
                Channel channel = await db.Channels.FirstOrDefaultAsync(c => c.Id == model.ChannelId);
                Post post = new Post { Date = DateTime.Now.ToString(), Views = model.Views, Subscribers = model.Subscribers, Cost = model.Cost, ChannelId = model.ChannelId, Channel = channel };
                post.Convercy = (post.Subscribers == 0) ? 0 : (post.Views / post.Subscribers);
                post.SingleSubscriberCost = (post.Subscribers == 0) ? 0 : (post.Cost / post.Subscribers);
                db.Posts.Add(post);
                await db.SaveChangesAsync();

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
            Log();
            if (id != null)
            {
                Post post = new Post { Id = id.Value };
                db.Entry(post).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Posts");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreateChannel()
        {
            Log();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(ChannelViewModel model)
        {
            Log();
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
            Log();
            if (id != null)
            {
                Channel channel = new Channel(id.Value);

                db.Entry(channel).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Posts");
            }
            return NotFound();
        }

        public void Log()
        {
            
            string requestbody;
            using (Stream stream = Request.Body)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    requestbody = sr.ReadToEnd();
                }
            }
            Logs log = new Logs { DateTime = DateTime.Now, Body = requestbody, Host = Request.Host.ToString(), Method = Request.Method, Path = Request.Path, Protocol = Request.Protocol };
            db.Logs.Add(log);
            db.SaveChanges();
            logger.LogInformation($"{log.DateTime}, {log.Method}, {log.Path}, {log.Host}, {log.Protocol}, {log.Body}");
        }
    }
}

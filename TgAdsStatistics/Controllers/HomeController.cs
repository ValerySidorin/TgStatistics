using System;
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

        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Posts()
        {
            var posts = db.Posts.Include(p => p.Channel);
            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            PostViewModel model = new PostViewModel
            {
                Channels = new SelectList(db.Channels, "Id", "ChannelName")
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostViewModel model)
        {
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(ChannelViewModel model)
        {
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
            if (id != null)
            {
                Channel channel = new Channel(id.Value);

                db.Entry(channel).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Posts");
            }
            return NotFound();
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
    }
}

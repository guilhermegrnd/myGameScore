using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myGameScore.Database;
using myGameScore.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace myGameScore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContextLibrary _context;

        public HomeController(ContextLibrary context)
        {
            _context = context;
        }
        
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Play()
        {
            return View();
        }

        public IActionResult Result()
        {
            var scores = _context.scores
                .OrderBy(item => item.Date)
                .ToList();

            dynamic jsonResult = Score.GetResults(scores);
            ViewBag.gamesResult = jsonResult;

            return View();
        }

        [HttpPost]
        public JsonResult SaveScorePoints(Score data)
        {
            try
            { 
                _context.scores.Add(data);
                int num = _context.SaveChanges();
                string status = "err";
                string msg = "Falha ao lançar os pontos";
                if(num == 1)
                {
                    status = "suc";
                    msg = "Pontos lançados com sucesso";
                }

                string jsonStr = "{\"status\":\""+ status + "\",\"msg\":\"" + msg + "\"}";
                var json = JObject.Parse(jsonStr);
                return Json(json);
            }
            catch (Exception e)
            {
                string jsonStr = "{\"status\":\"err\",\"msg\":\"" + e.Message + "\"}";
                var json = JObject.Parse(jsonStr);
                return Json(json);
            }
        }

        [HttpPost]
        public JsonResult DeletePoints()
        {
            try 
            { 
                var scores = _context.scores
                    .OrderBy(item => item.Date)
                    .ToList();
                scores.ForEach(x =>
                {
                    _context.scores.Remove(x);
                });
                int num = _context.SaveChanges();
                string status = "err";
                string msg = "Falha ao excluir pontos";
                if (num > -1)
                {
                    status = "suc";
                    msg = "Pontos excluídos com sucesso";
                }

                string jsonStr = "{\"status\":\"" + status + "\",\"msg\":\"" + msg + "\"}";
                var json = JObject.Parse(jsonStr);
                return Json(json);
            }
            catch (Exception e)
            {
                string jsonStr = "{\"status\":\"err\",\"msg\":\"" + e.Message + "\"}";
                var json = JObject.Parse(jsonStr);
                return Json(json);
            }
        }
    }
}

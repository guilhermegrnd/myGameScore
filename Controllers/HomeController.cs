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
            int totalGames = 0;
            int totalPoints = 0;
            double averagePoints = 0;
            int highestPoints = 0;
            int lowestPoints = 0;
            int recordsBeated = 0;
            int recordPoints = 0;
            string date1 = "";
            string date2 = "";
            string periodo = "";
            int i = 0;
            try
            {
                var scores = _context.scores
                    .OrderBy(item => item.Date)
                    .ToList();

                if (scores.Count() > 0)
                {
                    int j = scores.Count() - 1;
                    scores.ForEach(x =>
                    {
                        Console.WriteLine("ID: " + x.Id + "DATE: " + x.Date + "POINTS: " + x.Points);

                        if (i == 0)
                        {
                            date1 = scores[i].Date;
                            recordPoints = scores[i].Points;
                            lowestPoints = scores[i].Points;
                            highestPoints = scores[i].Points;
                        }

                        totalPoints += scores[i].Points;
                        if (scores[i].Points > highestPoints)
                        {
                            highestPoints = scores[i].Points;
                        }

                        if (scores[i].Points < lowestPoints)
                        {
                            lowestPoints = scores[i].Points;
                        }

                        Console.WriteLine("PONTO ATUAL: " + scores[i].Points.ToString() + "RECORD: " + recordPoints);
                        if (scores[i].Points > recordPoints)
                        {
                            recordsBeated++;
                            recordPoints = scores[i].Points;
                        }

                        i++;
                    });

                    totalGames = i;
                    date2 = scores[j].Date;
                    averagePoints = totalPoints / totalGames;
                    string[] dateSplit = date1.Split("-");
                    date1 = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
                    dateSplit = date2.Split("-");
                    date2 = dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0];
                    periodo = date1 + " até " + date2;
                }

                dynamic jsonResult = new JObject();
                jsonResult.periodo = periodo;
                jsonResult.totalGames = totalGames;
                jsonResult.totalPoints = totalPoints;
                jsonResult.averagePoints = averagePoints;
                jsonResult.highestPoints = highestPoints;
                jsonResult.lowestPoints = lowestPoints;
                jsonResult.recordsBeated = recordsBeated;

                ViewBag.gamesResult = jsonResult;

                return View();
            }
            catch(Exception e)
            {
                string jsonStr = "{\"status\":\"err\",\"msg\":\"" + e.Message + "\"}";
                var json = JObject.Parse(jsonStr);
                return Json(json);
            }
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

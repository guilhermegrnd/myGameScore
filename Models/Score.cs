using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myGameScore.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Points { get; set; }

        public static Object GetResults(List<Score> scores)
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
            if (scores.Count() > 0)
            {
                int j = scores.Count() - 1;
                scores.ForEach(x =>
                {
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

                    if (scores[i].Points > recordPoints)
                    {
                        recordsBeated++;
                        recordPoints = scores[i].Points;
                    }

                    i++;
                });

                totalGames = i;
                date2 = scores[j].Date;
                averagePoints = (double)totalPoints / (double)totalGames;
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

            return jsonResult;
        }
    }
}

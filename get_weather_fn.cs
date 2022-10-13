using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace weather_daily_github_fn
{
    public class get_weather_fn
    {
        [FunctionName("get_weather_fn")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var weatherApiKey = Environment.GetEnvironmentVariable("weatherApiKey");
            var emailAuthPassword = Environment.GetEnvironmentVariable("gmailpasswd");
            if(weatherApiKey == null || emailAuthPassword == null)
            {
                log.LogError("Could not retrieve any of the keys.");
                return;
            }

            try
            {
                var openWeatherAPI = new OpenWeatherAPI.OpenWeatherApiClient(weatherApiKey);
                var query = await openWeatherAPI.QueryAsync("New York,us");
                var message = "The weather is " + query.Main.Temperature.CelsiusCurrent + " Celcius \n At: " + DateTime.Now + "\n For: " +
                    query.Sys.Country;
                log.LogInformation("Weather API called succesfully");

            }
            catch (Exception e)
            {
                log.LogError(e.Message);
            }
        }
    }
}

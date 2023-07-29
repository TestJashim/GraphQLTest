using Microsoft.AspNetCore.Mvc;

namespace GraphQLTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly List<WeatherForecast> _forecasts;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _forecasts = GenerateWeatherForecasts().ToList();
        }

        private IEnumerable<WeatherForecast> GenerateWeatherForecasts()
        {
            return Enumerable.Range(1, 30).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)), // Use DateOnly instead of DateTime
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 30).Select(index => new WeatherForecast
            {
                Id = new Random().Next(1, 100), // Generate a random integer as Id
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] WeatherForecast newForecast)
        //{
        //    // Add logic to handle the new forecast data and save it.
        //    // For demonstration purposes, let's return a 201 Created response with the new forecast.
        //    return CreatedAtRoute("GetWeatherForecast", new { id = newForecast.Id }, newForecast);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] WeatherForecast updatedForecast)
        //{
        //    // Add logic to handle the updated forecast data and update the existing forecast.
        //    // For demonstration purposes, let's return a 204 No Content response.
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    // Add logic to handle the deletion of the forecast.
        //    // For demonstration purposes, let's return a 204 No Content response.
        //    return NoContent();
        //}


        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast newForecast)
        {
            // Assign a unique Id to the new forecast.
            newForecast.Id = _forecasts.Count + 1;

            // Add the new forecast to the data store.
            _forecasts.Add(newForecast);

            // Return a 201 Created response with the new forecast.
            return CreatedAtRoute("GetWeatherForecast", new { id = newForecast.Id }, newForecast);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WeatherForecast updatedForecast)
        {
            // Find the existing forecast by its Id.
            WeatherForecast existingForecast = _forecasts.FirstOrDefault(f => f.Id == id);

            if (existingForecast == null)
            {
                return NotFound("Forecast not found.");
            }

            // Update the existing forecast with the new data.
            existingForecast.Date = updatedForecast.Date;
            existingForecast.TemperatureC = updatedForecast.TemperatureC;
            existingForecast.Summary = updatedForecast.Summary;

            // Return a 204 No Content response.
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Find the forecast to be deleted by its Id.
            WeatherForecast forecastToDelete = _forecasts.FirstOrDefault(f => f.Id == id);

            if (forecastToDelete == null)
            {
                return NotFound("Forecast not found.");
            }

            // Remove the forecast from the data store.
            _forecasts.Remove(forecastToDelete);

            // Return a 204 No Content response.
            return NoContent();
        }

        [HttpGet("GetAllSummaries")]
        public IActionResult GetAllSummaries()
        {
            return Ok(Summaries);
        }

        [HttpGet("GetRandomSummary")]
        public IActionResult GetRandomSummary()
        {
            Random random = new Random();
            string randomSummary = Summaries[random.Next(Summaries.Length)];
            return Ok(randomSummary);
        }

        [HttpGet("GetSummaryByIndex/{index}")]
        public IActionResult GetSummaryByIndex(int index)
        {
            if (index >= 0 && index < Summaries.Length)
            {
                string summary = Summaries[index];
                return Ok(summary);
            }
            return NotFound("Summary not found.");
        }

        [HttpGet("SearchSummaries/{keyword}")]
        public IActionResult SearchSummaries(string keyword)
        {
            var matchingSummaries = Summaries.Where(s => s.ToLower().Contains(keyword.ToLower()));
            if (matchingSummaries.Any())
            {
                return Ok(matchingSummaries);
            }
            return NotFound("No matching summaries found.");
        }

        [HttpGet("CountSummaries")]
        public IActionResult CountSummaries()
        {
            int count = Summaries.Length;
            return Ok(count);
        }
    }
}
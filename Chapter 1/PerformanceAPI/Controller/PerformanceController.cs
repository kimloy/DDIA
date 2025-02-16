using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

public class PerformanceController : Controller 
{
    private readonly Meter _meter;
    private readonly Histogram<double> _requestDurationHistogram;

    public PerformanceController(Meter meter){
        _meter = meter;
        _requestDurationHistogram = _meter.CreateHistogram<double>(
            "http_request_duration_ms", 
            unit: "ms",
            description: "Measures the duration of HTTP requests in milliseconds"
        );
    }
    [HttpGet("simulate")]
    public async Task<IActionResult>SimulateProcessing()
    {

        var startTime = DateTime.UtcNow;

        // Simulate some processing
        await Task.Delay(Random.Shared.Next(100, 500)); // Simulate request time

        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;

        // Record the duration in the histogram
        _requestDurationHistogram.Record(duration);

        return Ok(new { message = "Test complete", duration });
    }
}
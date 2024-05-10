using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebCalc;

[Route("api/[controller]")]
[ApiController]
public class CalculatorController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly ICalculator _calculator;

    public CalculatorController(IMemoryCache cache, ICalculator calculator)
    {
        _cache = cache;
        _calculator = calculator;
    }

    [HttpPost]
    public IActionResult Calculate(string expression)
    {
        if (_cache.TryGetValue(expression, out double cachedResult))
        {
            return Ok(cachedResult);
        }

        double result;
        try
        {
            result = _calculator.Calculate(expression);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        _cache.Set(expression, result, TimeSpan.FromMinutes(10));

        return Ok(result);
    }
}
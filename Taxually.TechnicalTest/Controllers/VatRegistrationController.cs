using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VatRegistrationController(IVatRegistrationService vatRegistrationService) : ControllerBase
{
    /// <summary>
    /// Registers a company for a VAT number in a given country
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
    {
        try
        {
            await vatRegistrationService.RegisterCompany(request);
            return Ok($"Company with ID '{request.CompanyId}' was successfully processed!");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error occurred during processing Company:{request.CompanyId}. Details:{ ex.Message}");
        }
    }
}

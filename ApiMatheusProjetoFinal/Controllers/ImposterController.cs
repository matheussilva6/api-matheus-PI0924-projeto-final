using Microsoft.AspNetCore.Mvc;
using ApiMatheusProjetoFinal.Services;

namespace ApiMatheusProjetoFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImposterController : ControllerBase
    {
        private readonly ImposterService _imposterService;

        public ImposterController(ImposterService imposterService)
        {
            _imposterService = imposterService;
        }

        [HttpGet("inventory/{sku}")]
        public async Task<IActionResult> GetInventory(string sku)
        {
            try
            {
                var result = await _imposterService.GetInventoryAsync(sku);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { erro = "Serviço de inventário indisponível", detalhe = ex.Message });
            }
        }

        [HttpPost("payments")]
        public async Task<IActionResult> CreatePayment([FromBody] object paymentData)
        {
            try
            {
                var result = await _imposterService.CreatePaymentAsync(paymentData);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { erro = "Serviço de pagamentos indisponível", detalhe = ex.Message });
            }
        }
    }
}
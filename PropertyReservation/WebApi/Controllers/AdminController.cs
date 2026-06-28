using Application.DTOs.Admin;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Panel del administrador. Expone las estadísticas generales del sistema.
    /// Todos los endpoints requieren el rol <c>Admin</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly AdminStatsService _adminStatsService;

        public AdminController(AdminStatsService adminStatsService)
        {
            _adminStatsService = adminStatsService;
        }

        /// <summary>Obtiene las estadísticas generales del sistema.</summary>
        /// <remarks>Totales de propiedades, reservas, usuarios, ingresos, etc. para el dashboard.</remarks>
        /// <response code="200">Estadísticas del sistema.</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(AdminStatsDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AdminStatsDTO>> GetStats()
        {
            var stats = await _adminStatsService.GetStatsAsync();
            return Ok(stats);
        }
    }
}

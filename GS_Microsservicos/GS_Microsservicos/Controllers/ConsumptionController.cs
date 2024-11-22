using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GS_Microsservicos.Models;
using GS_Microsservicos.Repositories;

namespace GS_Microsservicos.Controllers
{
    [ApiController]
    [Route("consumo")]
    public class ConsumptionController : ControllerBase
    {
        private readonly IConsumptionRepository _consumption;

        public ConsumptionController(IConsumptionRepository consumption)
        {
            _consumption = consumption;
        }

        [HttpPost]
        public async Task<IActionResult> AddConsumption([FromBody] Consumptiondomain record)
        {
            try
            {
                if (record == null || record.Consumption < 0)
                    return BadRequest(new { message = "Invalid data" });

                await _consumption.Save(record);
                return CreatedAtAction(nameof(GetConsumptionById), new { id = record.Id.ToString() }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the record", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsumptionById(string id)
        {
            try
            {
                var record = await _consumption.GetById(id);
                if (record == null)
                    return NotFound(new { message = "Record not found" });

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the record", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConsumptions()
        {
            try
            {
                var records = await _consumption.ListAll();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the records", error = ex.Message });
            }
        }
    }
}



using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using trips.Models;
using trips.Models.DTO;
using trips.Services;

namespace trips.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TripsController : ControllerBase
	{
		private readonly IDatabaseService _databaseService;

		public TripsController(IDatabaseService databaseService)
		{
			_databaseService = databaseService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTripsAsync()
		{
			return Ok(await _databaseService.GetTripsAsync());
		}

		[HttpDelete("/api/clients/{idClient}")]
		public async Task<IActionResult> DeleteClientAsync([FromRoute] int idClient)
		{
			if (await _databaseService.IsClientAssigned(idClient))
				return BadRequest("Couldn't delete client");

			await _databaseService.DeleteClientAsync(idClient);

			return Ok();
		}

		[HttpPost("{idTrip}/clients")]
		public async Task<IActionResult> AssignClientToTripAsync([FromRoute] int idTrip, [FromBody] ClientTripDto payload)
		{
			int idClient = await _databaseService.AddClientAsync(new()
			{
				FirstName = payload.FirstName,
				LastName = payload.LastName,
				Email = payload.Email,
				Telephone = payload.Telephone,
				Pesel = payload.Pesel
			});

			if (await _databaseService.IsClientAssignedToTripAsync(idClient, idTrip))
				return BadRequest("Client is already assigned to the trip");

			if (idTrip != payload.IdTrip || !await _databaseService.TripExistsAsync(payload.IdTrip, payload.TripName))
				return BadRequest("Invalid trip");

			await _databaseService.AssignClientToTripAsync(payload);

			return Ok();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trips.Models;
using trips.Models.DTO;

namespace trips.Services
{
	public interface IDatabaseService
	{
		Task<IEnumerable<TripDto>> GetTripsAsync();
		Task<bool> IsClientAssigned(int idClient);
		Task DeleteClientAsync(int idClient);
		Task<int> AddClientAsync(Client client);
		Task<bool> IsClientAssignedToTripAsync(int idClient, int idTrip);
		Task<bool> TripExistsAsync(int idTrip, string name);
		Task AssignClientToTripAsync(ClientTripDto payload);
	}
}

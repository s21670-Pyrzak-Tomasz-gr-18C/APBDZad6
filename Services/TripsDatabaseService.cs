using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using trips.Models;
using trips.Models.DTO;

namespace trips.Services
{
	public class TripsDatabaseService : IDatabaseService
	{
		private readonly DatabaseContext _databaseContext;

		public TripsDatabaseService(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public async Task<IEnumerable<TripDto>> GetTripsAsync()
		{
			return await _databaseContext.Trips
				.Select(trip => new TripDto
				{
					Name = trip.Name,
					Description = trip.Name,
					DateFrom = trip.DateFrom,
					DateTo = trip.DateTo,
					MaxPeople = trip.MaxPeople,
					Countries = trip.CountryTrips
						.Select(countryTrip => new CountryDto
						{
							Name = countryTrip.IdCountryNavigation.Name
						}).ToList(),
					Clients = trip.ClientTrips
						.Select(clientTrip => new ClientDto
						{
							FirstName = clientTrip.IdClientNavigation.FirstName,
							LastName = clientTrip.IdClientNavigation.LastName
						}).ToList()
				})
				.OrderByDescending(trip => trip.DateFrom)
				.ToListAsync();
		}

		public async Task<bool> IsClientAssigned(int idClient)
		{
			return await _databaseContext.Clients
				.Where(client => client.IdClient == idClient)
				.AnyAsync(client => client.ClientTrips.Any());
		}

		public async Task DeleteClientAsync(int idClient)
		{
			var client = new Client() { IdClient = idClient };

			_databaseContext.Clients.Attach(client);
			_databaseContext.Clients.Remove(client);

			await _databaseContext.SaveChangesAsync();
		}

		private async Task<Client> GetClientAsync(string pesel)
		{
			return await _databaseContext.Clients
				.Where(client => client.Pesel.Equals(pesel))
				.FirstOrDefaultAsync();
		}

		public async Task<int> AddClientAsync(Client client)
		{
			var existingClient = await GetClientAsync(client.Pesel);

			if (existingClient != null)
				return existingClient.IdClient;

			_databaseContext.Clients.Add(client);

			await _databaseContext.SaveChangesAsync();

			return client.IdClient;
		}

		public async Task<bool> IsClientAssignedToTripAsync(int idClient, int idTrip)
		{
			return await _databaseContext.Clients
				.Where(client => client.IdClient == idClient)
				.AnyAsync(client => client.ClientTrips
					.Any(clientTrip => clientTrip.IdTrip == idTrip)
				);
		}

		public async Task<bool> TripExistsAsync(int idTrip, string name)
		{
			return await _databaseContext.Trips
				.AnyAsync(trip => trip.IdTrip == idTrip && trip.Name.Equals(name));
		}

		public async Task AssignClientToTripAsync(ClientTripDto payload)
		{
			var client = await GetClientAsync(payload.Pesel);

			var clientTrip = new Client_Trip()
			{
				IdClientNavigation = client,
				IdTrip = payload.IdTrip,
				RegisteredAt = DateTime.Now,
				PaymentDate = payload.PaymentDate
			};

			_databaseContext.ClientTrips.Add(clientTrip);

			await _databaseContext.SaveChangesAsync();
		}
	}
}

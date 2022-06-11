using System;
using System.Collections.Generic;

#nullable disable

namespace trips.Models
{
	public partial class Trip
	{
		public Trip()
		{
			ClientTrips = new HashSet<Client_Trip>();
			CountryTrips = new HashSet<Country_Trip>();
		}

		public int IdTrip { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public int MaxPeople { get; set; }

		public virtual ICollection<Client_Trip> ClientTrips { get; set; }
		public virtual ICollection<Country_Trip> CountryTrips { get; set; }
	}
}

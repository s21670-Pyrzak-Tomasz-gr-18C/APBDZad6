using System.Collections.Generic;

#nullable disable

namespace trips.Models
{
	public partial class Country
	{
		public Country()
		{
			CountryTrips = new HashSet<Country_Trip>();
		}

		public int IdCountry { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Country_Trip> CountryTrips { get; set; }
	}
}

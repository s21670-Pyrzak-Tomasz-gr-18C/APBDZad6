using System.Collections.Generic;

#nullable disable

namespace trips.Models
{
	public partial class Client
	{
		public Client()
		{
			ClientTrips = new HashSet<Client_Trip>();
		}

		public int IdClient { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Telephone { get; set; }
		public string Pesel { get; set; }

		public virtual ICollection<Client_Trip> ClientTrips { get; set; }
	}
}

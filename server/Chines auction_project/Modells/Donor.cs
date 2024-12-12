using System.ComponentModel.DataAnnotations;

namespace Chines_auction_project.Modells
{
    public class Donor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<Present> ?Presents { get; set; }

    }
}

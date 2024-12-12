using System.ComponentModel.DataAnnotations;

namespace Chines_auction_project.Modells
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Address { get; set; }
        public IEnumerable<Purchase> ?Purchase { get; set; }
    }
}

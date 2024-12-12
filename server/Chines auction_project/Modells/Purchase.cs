namespace Chines_auction_project.Modells
{
    public class Purchase
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public DateTime dateOfPurchase { get; set; }
        public string? TypeOfPayment { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
        public User? User { get; set; }
    }
}

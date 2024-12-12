using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Chines_auction_project.Modells.Dto
{
    public class PresentDonorDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public float Price { get; set; }
        public int DonorId { get; set; }
        public int? WinnerId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Category? Category { get; set; }
        public Donor? Donor { get; set; }
        public User? Winner { get; set; }
    }
}

namespace Chines_auction_project.Modells.Dto
{
    public class PresentDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public float Price { get; set; }
        public int DonorId { get; set; }
        public string ImagePath { get; set; }

        public string Description { get; set; }
    }
}

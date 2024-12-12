using AutoMapper;
using Chines_auction_project.Modells.Dto;

namespace Chines_auction_project.Modells
{
    public class DIProfile:Profile
    {
        public DIProfile()
        {
            CreateMap<DonorDto, Donor>().ForMember(d => d.FullName, i => i.MapFrom(o => o.FirstName + " " + o.LastName));
            CreateMap<PresentDto, Present>();
            CreateMap<PresentDonorDto, Present>();
            CreateMap<CategoryDto, Category>();
            CreateMap<ManagerDto, Manager>().ForMember(d => d.FullName, i => i.MapFrom(o => o.FirstName + " " + o.LastName));
            CreateMap<PurchaseDto, Purchase>().ForMember(d => d.dateOfPurchase, i => i.MapFrom(o => DateTime.Now)).ForMember(d => d.Status, i => i.MapFrom(o => false));
            CreateMap<UserDto, User>().ForMember(d => d.FullName, i => i.MapFrom(o => o.FirstName + " " + o.LastName));
            CreateMap<TicketDto, Ticket>().ForMember(d => d.Quantity, i => i.MapFrom(o => 1));

        }
    }
}

using AutoMapper;
using StockManagement.Models.Dto;


namespace StockManagement.Models.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ActionDto, ActionResponseModel>();
            CreateMap<ActionEditModel, ActionDto>();

            CreateMap<ActivityDto, ActivityResponseModel>();
            CreateMap<ActivityEditModel, ActivityDto>();

            CreateMap<DeliveryNoteDto, DeliveryNoteResponseModel>();
            CreateMap<DeliveryNoteEditModel, DeliveryNoteDto>();

            CreateMap<ProductDto, ProductResponseModel>();
            CreateMap<ProductEditModel, ProductDto>();

            CreateMap<ProductProductTypeDto, ProductProductTypeResponseModel>();
            CreateMap<ProductProductTypeEditModel, ProductProductTypeDto>();

            CreateMap<ProductTypeDto, ProductTypeResponseModel>();
            CreateMap<ProductTypeEditModel, ProductTypeDto>();

            CreateMap<VenueDto, VenueResponseModel>();
            CreateMap<VenueEditModel, VenueDto>();
        }
    }
}

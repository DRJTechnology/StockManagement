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

            CreateMap<DeliveryNoteDetailDto, DeliveryNoteDetailResponseModel>();
            CreateMap<DeliveryNoteDetailEditModel, DeliveryNoteDetailDto>();

            CreateMap<DeliveryNoteDto, DeliveryNoteResponseModel>();
            CreateMap<DeliveryNoteEditModel, DeliveryNoteDto>();

            CreateMap<ProductDto, ProductResponseModel>();
            CreateMap<ProductEditModel, ProductDto>();

            CreateMap<ProductTypeDto, ProductTypeResponseModel>();
            CreateMap<ProductTypeEditModel, ProductTypeDto>();

            CreateMap<SupplierDto, SupplierResponseModel>();
            CreateMap<SupplierEditModel, SupplierDto>();

            CreateMap<VenueDto, VenueResponseModel>();
            CreateMap<VenueEditModel, VenueDto>();
        }
    }
}

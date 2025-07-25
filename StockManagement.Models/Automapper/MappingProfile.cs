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

            CreateMap<ContactDto, ContactResponseModel>();
            CreateMap<ContactEditModel, ContactDto>();

            CreateMap<DeliveryNoteDetailDto, DeliveryNoteDetailResponseModel>();
            CreateMap<DeliveryNoteDetailEditModel, DeliveryNoteDetailDto>();

            CreateMap<DeliveryNoteDto, DeliveryNoteResponseModel>();
            CreateMap<DeliveryNoteEditModel, DeliveryNoteDto>();

            CreateMap<LocationDto, LocationResponseModel>();
            CreateMap<LocationEditModel, LocationDto>();

            CreateMap<ProductDto, ProductResponseModel>();
            CreateMap<ProductEditModel, ProductDto>();

            CreateMap<ProductTypeDto, ProductTypeResponseModel>();
            CreateMap<ProductTypeEditModel, ProductTypeDto>();

            CreateMap<SettingDto, SettingResponseModel>();
            CreateMap<SettingEditModel, SettingDto>();

            CreateMap<StockReceiptDetailDto, StockReceiptDetailResponseModel>();
            CreateMap<StockReceiptDetailEditModel, StockReceiptDetailDto>();

            CreateMap<StockReceiptDto, StockReceiptResponseModel>();
            CreateMap<StockReceiptEditModel, StockReceiptDto>();
        }
    }
}

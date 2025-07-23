using AutoMapper;
using StockManagement.Models.Dto;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;


namespace StockManagement.Models.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
       {
            CreateMap<AccountTypeDto, AccountTypeResponseModel>();
            CreateMap<AccountTypeEditModel, AccountTypeDto>();

            CreateMap<AccountDto, AccountResponseModel>();
            CreateMap<AccountEditModel, AccountDto>();

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

            CreateMap<SettingDto, SettingResponseModel>();
            CreateMap<SettingEditModel, SettingDto>();

            CreateMap<StockReceiptDetailDto, StockReceiptDetailResponseModel>();
            CreateMap<StockReceiptDetailEditModel, StockReceiptDetailDto>();

            CreateMap<StockReceiptDto, StockReceiptResponseModel>();
            CreateMap<StockReceiptEditModel, StockReceiptDto>();

            CreateMap<SupplierDto, SupplierResponseModel>();
            CreateMap<SupplierEditModel, SupplierDto>();

            CreateMap<VenueDto, VenueResponseModel>();
            CreateMap<VenueEditModel, VenueDto>();

            CreateMap<TransactionDetailDto, TransactionDetailResponseModel>();
            CreateMap<TransactionDetailEditModel, TransactionDetailDto>();
        }
    }
}

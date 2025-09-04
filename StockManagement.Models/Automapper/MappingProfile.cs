using AutoMapper;
using StockManagement.Models.Dto;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Enums;
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

            CreateMap<ContactDto, ContactResponseModel>();
            CreateMap<ContactEditModel, ContactDto>();

            CreateMap<StockSaleDetailDto, StockSaleDetailResponseModel>();
            CreateMap<StockSaleDetailEditModel, StockSaleDetailDto>();

            CreateMap<StockSaleDto, StockSaleResponseModel>();
            CreateMap<StockSaleEditModel, StockSaleDto>();

            CreateMap<LocationDto, LocationResponseModel>();
            CreateMap<LocationEditModel, LocationDto>();

            CreateMap<InventoryBatchDto, InventoryBatchResponseModel>()
                .ForMember(dest => dest.InventoryBatchStatus, opt => opt.MapFrom(src => (InventoryBatchStatusEnum)src.InventoryBatchStatusId));

            CreateMap<ProductDto, ProductResponseModel>();
            CreateMap<ProductEditModel, ProductDto>();

            CreateMap<ProductTypeDto, ProductTypeResponseModel>();
            CreateMap<ProductTypeEditModel, ProductTypeDto>();

            CreateMap<SettingDto, SettingResponseModel>();
            CreateMap<SettingEditModel, SettingDto>();

            CreateMap<StockOrderDetailDto, StockOrderDetailResponseModel>();
            CreateMap<StockOrderDetailEditModel, StockOrderDetailDto>();

            CreateMap<StockOrderDto, StockOrderResponseModel>();
            CreateMap<StockOrderEditModel, StockOrderDto>();

            CreateMap<StockOrderDetailPaymentResponseModel, StockOrderResponseModel>();
            CreateMap<StockOrderResponseModel, StockOrderDetailPaymentResponseModel>();

            CreateMap<TransactionDetailDto, TransactionDetailResponseModel>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => (TransactionTypeEnum)src.TransactionTypeId));
            CreateMap<TransactionDetailEditModel, TransactionDetailDto>()
                .ForMember(dest => dest.TransactionTypeId, opt => opt.MapFrom(src => (int)src.TransactionType));
        }
    }
}

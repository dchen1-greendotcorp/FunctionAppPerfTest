using AutoMapper;
using FunctionAppPerfTest.Models;
using System;
using System.Linq;

namespace FunctionAppPerfTest
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            MapCreateDdaAccountRequestToCreateCardAccountRequest();
            MapCreateCardAccountResponseToCreateAccountResponse();
            MapCreateCardAccountResponseToAddCardAccountActivityData();
            MapRegisterCardActivityDataToRegisterCardRequest();
        }
        void MapCreateDdaAccountRequestToCreateCardAccountRequest()
        {
            CreateMap<CreateAccountRequest, CreateCardAccountRequest>()
                .ForMember(d => d.RequestId, opt => opt.MapFrom(s => s.RequestId))
                .ForMember(d => d.OtherRequestData, opt => opt.MapFrom(s => s.OtherRequestData))
                .ForMember(d => d.OtherRequestData, opt => opt.MapFrom(s => s.OtherRequestData));
        }

        void MapCreateCardAccountResponseToCreateAccountResponse()
        {
            CreateMap<CreateCardAccountResponse, CreateAccountResponse>()
                .ForMember(d => d.Data, opt => opt.MapFrom(s => new CreateAccountResponseData
                {
                    CardID = s.Data.CardAccountId,
                    OtherData = s.Data.OtherData
                }));
        }
        void MapCreateCardAccountResponseToAddCardAccountActivityData()
        {
            CreateMap<CreateCardAccountResponse, AddCardAccountActivityData>()

                .ForMember(d => d.CardAccountId, opt => opt.MapFrom(s => s.Data.CardAccountId))
                .ForMember(d => d.OtherData,
                    opt => opt.MapFrom(s => s.Data.OtherData));
        }

        void MapRegisterCardActivityDataToRegisterCardRequest()
        {
            CreateMap<RegisterCardActivityData, RegisterCardRequest>()
                .ForMember(d => d.RequestId, opt => opt.MapFrom(s => s.CreateAccountRequest.RequestId))
                .ForMember(d => d.CreateAccountRequest, opt => opt.MapFrom(s => s.CreateAccountRequest));
        }
        
    }
}
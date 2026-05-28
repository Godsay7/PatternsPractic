using AutoMapper;
using BLL.DTO;
using Domain.Entities;

namespace BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(d => d.AccountName, opt => opt.MapFrom(s => s.Account.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));

            CreateMap<TransactionDTO, Transaction>();

            CreateMap<CreateAccountDTO, Account>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<CreateTransactionDTO, Transaction>();
        }
    }
}

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

            // Складний мапінг для транзакцій (витягуємо імена з вкладених об'єктів)
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(d => d.AccountName, opt => opt.MapFrom(s => s.Account.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));

            // Мапінг назад з DTO в Entity (для збереження)
            CreateMap<TransactionDTO, Transaction>();
        }
    }
}

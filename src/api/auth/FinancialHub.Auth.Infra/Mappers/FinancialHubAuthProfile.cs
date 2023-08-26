namespace FinancialHub.Auth.Infra.Mappers
{
    public class FinancialHubAuthProviderProfile : Profile
    {
        public FinancialHubAuthProviderProfile() : base()
        {
            this.CreateMap<UserEntity,UserModel>().ReverseMap();
        }
    }
}

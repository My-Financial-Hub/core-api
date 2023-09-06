namespace FinancialHub.Core.Domain.Interfaces.Mappers
{
    /// <summary>
    /// Wrapper interface for Mapper Classes
    /// </summary>
    public interface IMapperWrapper
    {
        Y Map<T,Y>(T ent);
        Y Map<Y>(object ent);
    }
}

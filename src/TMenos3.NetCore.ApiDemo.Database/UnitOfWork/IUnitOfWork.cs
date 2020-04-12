using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Repositories;

namespace TMenos3.NetCore.ApiDemo.Database.UnitOfWork
{
    /// <summary>
    /// I would not use this implementation in a production environment
    /// since it is redundant. Entity Framework Core DbContext is already an 
    /// implementation of UnitOfWork pattern.
    /// Prior versions of EF would not allow to mock the DbContext, that why
    /// this pattern became handy.
    /// However, newer versions of EF, such as EF Core 2.0 and higher, allow
    /// to easily mock DbContexts. Take a look at the Integration Tests for 
    /// further examples
    /// </summary>
    public interface IUnitOfWork
    {
        IJobRepository JobRepository { get; }

        ILeagueRepository LeagueRepository { get; }

        ITeamRepository TeamRepository { get; }

        Task SaveChangesAsync();
    }
}

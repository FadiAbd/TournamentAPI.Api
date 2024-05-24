using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITournamentRepository Tournaments { get; }
        IGameRepository Games { get; }
        Task<int> CompleteAsync();
    }
}

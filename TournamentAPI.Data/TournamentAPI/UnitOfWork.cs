using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.TournamentAPI
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentApiContext _context;
        public IGameRepository Games { get; private set; }
        public ITournamentRepository Tournaments { get; private set; }

        public UnitOfWork(TournamentApiContext context)
        {
            _context = context;
            Games = new GameRepository(_context);
            Tournaments = new TournamentRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

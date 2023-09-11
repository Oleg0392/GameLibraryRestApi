using GameLibraryRestApi.Repositories.Interfaces;
using GameLibraryRestApi.Data.Entities;
using GameLibraryRestApi.Data.DbContexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryRestApi.Repositories
{
    public class GenreRefRepository : EFCoreRepository<GenreRef>, IGenreRefRepository
    {
        private readonly GameLibraryContext _context;
        
        public GenreRefRepository(GameLibraryContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IList<GenreRef>> InsertRangeAsync(IList<GenreRef> genreRefs, bool saveChanges = true)
        {
            string SqlQuery = "INSERT INTO GenreRefs (GameId, GenreId) VALUES ";
            foreach(var genreRef in genreRefs)
            {
                SqlQuery += String.Format("( {0},{1} ),", genreRef.GameId, genreRef.GenreId);
            }
            SqlQuery = SqlQuery.Remove(SqlQuery.Length-1);

            _context.Set<GenreRef>().AsNoTracking();
            int result = await _context.Database.ExecuteSqlRawAsync(SqlQuery);
            if (genreRefs.Count == result) return genreRefs;
            else
            {
                int genreRefsCount = genreRefs.Count;
                while (genreRefsCount != result)
                {
                    genreRefs.RemoveAt(result);
                    genreRefsCount--;
                }
            }


            return genreRefs;    
        }

        public async Task<int> DeleteDependenсies(Game gameToDelete)
        {           
            if (gameToDelete == null) return 0;
            string SqlQuery = String.Format("DELETE FROM GenreRefs WHERE GameId = {0}",gameToDelete.Id);
            _context.Set<GenreRef>().AsNoTracking();
            return await _context.Database.ExecuteSqlRawAsync(SqlQuery);
        }
    }
}

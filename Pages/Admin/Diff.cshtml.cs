using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Admin
{
    public class DiffModel : PageModel
    {
        private readonly DoltContext _context;

        public DiffModel(DoltContext context)
        {
            _context = context;
        }

        public MovieDiff MovieDiff {get;set; } = default!;

        public async Task OnGetAsync(string? id)
        {
            // The "id" parameter contains the Dolt commit ID where a movie was changed. To calculate what changed in that
            // Dolt commit, we use the dolt_diff() table function and provide the from commit, to commit, and the table name.
            // The same ancestry syntax from Git also works when specifying Dolt commits, so an easy way to specify the 
            // from commit parameter below is to append "~" to the end, meaning the first parent of the specified commit ID.
            var previousCommit = id + "~";
            var commit = id;

            // Because we know our application only updates a single row in our Movies table for each Dolt commit it creates,
            // we know that calling dolt_diff() to view the diff between two adjacent commits will only return one row, so
            // we use the .First<MovieDiff>() method to grab the first (and only) result. 
            this.MovieDiff = _context.Database.SqlQuery<MovieDiff>($"SELECT * from dolt_diff({previousCommit}, {commit}, 'Movie')").First<MovieDiff>();
        }
    }
}

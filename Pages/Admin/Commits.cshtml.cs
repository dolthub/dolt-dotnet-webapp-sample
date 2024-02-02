using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Admin
{
    public class CommitsModel : PageModel
    {
        private readonly DoltContext _context;

        public CommitsModel(DoltContext context)
        {
            _context = context;
        }

        public IList<Commit> Commits { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Commits = await _context.Commits.ToListAsync();
        }
    }
}

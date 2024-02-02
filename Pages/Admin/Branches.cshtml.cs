using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Admin
{
    public class BranchesModel : PageModel
    {
        private readonly DoltContext _context;

        public BranchesModel(DoltContext context)
        {
            _context = context;
        }

        public IList<Branch> Branches { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Branches = await _context.Branches.ToListAsync();
        }

        public string CurrentBranch() {
            return _context.GetActiveBranch();
        }
    }
}

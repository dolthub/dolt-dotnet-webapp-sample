using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using RazorPagesMovie.Data;

namespace RazorPagesMovie.Pages.Admin
{
    public class SetBranchModel : PageModel
    {
        private readonly DoltContext _context;

        public SetBranchModel(DoltContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id)
        {
            _context.SetActiveBranch(id);
            return RedirectToPage("/Admin/Branches");
        }
    }
}

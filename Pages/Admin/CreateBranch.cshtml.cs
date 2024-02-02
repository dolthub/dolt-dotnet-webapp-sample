using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;

namespace RazorPagesMovie.Pages.Movies
{
    public class CreateBranchModel : PageModel
    {
        private readonly DoltContext _context;

        [BindProperty]
        public string? NewBranchName { get; set; }

        public CreateBranchModel(DoltContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public string GetCurrentBranch() {
            return _context.GetActiveBranch();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CALL dolt_branch(@BranchName);";
                command.Parameters.Add(new MySqlConnector.MySqlParameter("@BranchName", NewBranchName));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();
                    var status = reader.GetInt32(0);
                    if (status != 0) {
                        throw new Exception("Unable to create branch; status: " + status);
                    }
                }
            }

            await connection.CloseAsync();
            return RedirectToPage("./Branches");
        }
    }
}

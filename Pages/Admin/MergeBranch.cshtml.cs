using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using RazorPagesMovie.Data;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesMovie.Pages.Admin
{
    public class MergeBranchModel : PageModel
    {
        private readonly DoltContext _context;

        public MergeBranchModel(DoltContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CALL dolt_checkout('main');";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();
                    var status = reader.GetInt32(0);
                    var message = reader.GetString(1);

                    if (status != 0) {
                        throw new Exception("Unable to checkout main branch: " + message);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CALL dolt_merge(@BranchName);";
                command.Parameters.Add(new MySqlConnector.MySqlParameter("@BranchName", id));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();
                    var hash = reader.GetString(0);
                    var conflicts = reader.GetInt32(2);
                    var message = reader.GetString(3);

                    if (conflicts != 0) {
                        throw new Exception("Unable to merge branch due to conflicts: " + message);
                    }
                }
            }

            await connection.CloseAsync();
            return RedirectToPage("/Admin/Branches");
        }
    }
}

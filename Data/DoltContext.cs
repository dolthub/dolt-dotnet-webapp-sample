using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using RazorPagesMovie.Pages.Admin;

namespace RazorPagesMovie.Data;

public partial class DoltContext : DbContext
{
    /// The activeBranch static property records the branch the user 
    /// currently has selected. This is modeled here as an application-wide 
    /// setting, so when a user changes it, the entire app switches to use 
    /// the new branch as the default. Since this is just tracked in memory,
    /// the active branch will reset to the default ("main") whenever the app
    /// is restarted.
    static string activeBranch = "main";

    private readonly IConfiguration _configuration;

    public DoltContext(IConfiguration configuration) {
        _configuration = configuration;
    }

    /// <summary>
    /// The OnConfiguring method gets invoked whenever a new DoltContext instances is
    /// configured for use before being injected into a page. This implementation 
    /// demonstrates a technique for working with Dolt branches where the application
    /// uses a global setting for which branch is active. Callers can use the SetActiveBranch
    /// method on this class to change the active branch, and each time a new DoltContext
    /// is configured, it will override the default connection string to specify the 
    /// specific branch that should be connected to. 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // There are many ways to control the active branch with Dolt. One of the most common
        // is to use the dolt_checkout() stored procedure from within a SQL session to checkout
        // a branch. This approach is flexible, but requires running a SQL statement on the connection
        // before the connection can be used. In the code below, we show how to use a database revision
        // specifier (e.g. <database_name>/<branch_name>) in a connection string to automatically select
        // a checked out branch as soon as the connection is established. 
        // https://docs.dolthub.com/sql-reference/version-control/branches#specify-a-database-revision-in-the-connection-string
        var connectionString = _configuration.GetConnectionString("RazorPagesMovieContext");
        connectionString = connectionString.Replace("database=dolt", "database=dolt/" + activeBranch);
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb4_0900_bin")
            .HasCharSet("utf8mb4");

        // The dolt_log system table provides a commit log for the currently checked out branch, similar to git log.
        // Because this is a system table that Dolt creates automatically, we exclude it from migrations. 
        // https://docs.dolthub.com/sql-reference/version-control/dolt-system-tables#dolt_log
        modelBuilder.Entity<Commit>()
            .ToTable("dolt_log")
            .HasAnnotation("Relational:IsTableExcludedFromMigrations", true);

        // The dolt_branches system table shows what branches exist in the Dolt database. This is also a system table
        // that Dolt provides automatically, so we also exclude it from migrations. 
        // https://docs.dolthub.com/sql-reference/version-control/dolt-system-tables#dolt_branches
        modelBuilder.Entity<Branch>()
            .ToTable("dolt_branches")
            .HasAnnotation("Relational:IsTableExcludedFromMigrations", true);

        // The MovieDiff model class is used to load diff information for the Movies table, using the dolt_diff()
        // table function. The result set from this table function depends on the schema of the table being queried,
        // and includes the values of each row in the table before and after a commit. Because this data comes from
        // a Dolt table function, we also want to exclude the MovieDiff class from migrations. 
        // https://docs.dolthub.com/sql-reference/version-control/dolt-sql-functions#dolt_diff
        modelBuilder.Entity<MovieDiff>()
            .HasNoKey()
            .HasAnnotation("Relational:IsTableExcludedFromMigrations", true);

        OnModelCreatingPartial(modelBuilder);
    }

    /// <summary>
    /// Method <c>SaveChangesAndCommitAsync</c> is similar to the SaveChangesAsync() method, but in addition to 
    /// committing the SQL transaction, it also creates a Dolt commit in the Dolt commit graph for this database.
    /// </summary>
    public async Task SaveChangesAndCommitAsync(string commitMessage) {
        using (var transaction = this.Database.BeginTransaction()) {
            try {
                await this.SaveChangesAsync();
                await this.Database.ExecuteSqlInterpolatedAsync($"call dolt_commit('-Am', {commitMessage});");

                // No need to explicitly commit the SQL transaction, since dolt_commit() will implicitly 
                // commit the in-progress transaction as part of creating the Dolt commit. 
            } catch (Exception) {
                transaction.Rollback();
                throw;
            }
        }
    }

    public string GetActiveBranch() {
        return activeBranch;
    }

    public void SetActiveBranch(string branch) {
        activeBranch = branch;
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<RazorPagesMovie.Models.Movie> Movie { get; set; } = default!;

    public DbSet<RazorPagesMovie.Models.Commit> Commits {get; set;} = default!;

    public DbSet<RazorPagesMovie.Models.Branch> Branches {get; set;} = default!;

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace RazorPagesMovie.Models;

/// <summary>
/// The Branch class models a Dolt branch, as returned by the dolt_branches system table. 
/// </summary>
/// https://docs.dolthub.com/sql-reference/version-control/dolt-system-tables#dolt_branches
public class Branch
{
    [Key]
    [Column("name")]
    public string? Name { get; private set; }

    [Column("hash")]
    public string? Hash { get; private set; }

    [Column("latest_committer")]
    public string? LatestCommitter { get; private set; }

    [Column("latest_committer_email")]
    public string? LatestCommitterEmail { get; private set; }

    [Column("latest_commit_date")]
    public DateTime? LatestCommitDate { get; private set; }

    [Column("latest_commit_message")]
    public string? LatestCommitMessage { get; private set; }
}
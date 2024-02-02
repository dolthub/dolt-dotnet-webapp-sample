using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models;

/// <summary>
/// The Commit class models a Dolt commit, as returned by the dolt_log system table. 
/// </summary>
/// https://docs.dolthub.com/sql-reference/version-control/dolt-system-tables#dolt_log
public class Commit
{
    [Key]
    [Column("commit_hash")]
    public string? CommitHash { get; private set; }

    [Column("committer")]
    public string? Committer { get; private set; }

    [Column("email")]
    public string? CommitterEmail { get; private set; }

    [Column("date")]
    public DateTime Date { get; private set; }

    [Column("message")]
    public string? Message { get; private set; }
}
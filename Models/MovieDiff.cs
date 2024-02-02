using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models;

/// <summary>
/// The MovieDiff class models the changes to a Movie between two Dolt commits. This information is loaded from the
/// dolt_diff() table function provided by Dolt. The returned schema from dolt_diff() shows the values for a single
/// table record before and after a Dolt commit. The fields prepended with "from_" are the values before the Dolt
/// commit, and the fields prepended with "to_" are the values after the Dolt commit. 
/// </summary>
/// https://docs.dolthub.com/sql-reference/version-control/dolt-sql-functions#dolt_diff
public class MovieDiff
{
    [Column("from_id")]
    public Guid? FromId { get; private set; }

    [Column("to_id")]
    public Guid? ToId { get; private set; }

    [Column("from_title")]
    public string? FromTitle { get; private set; }

    [Column("to_title")]
    public string? ToTitle { get; private set; }

    [Column("from_releasedate")]
    [DataType(DataType.Date)]
    public DateTime? FromReleaseDate { get; private set; }

    [Column("to_releasedate")]
    [DataType(DataType.Date)]
    public DateTime? ToReleaseDate { get; private set; }

    [Column("from_genre")]
    public string? FromGenre { get; private set; }

    [Column("to_genre")]
    public string? ToGenre { get; private set; }

    [Column("from_price")]
    public decimal? FromPrice { get; private set; }

    [Column("to_price")]
    public decimal? ToPrice { get; private set; }

    [Column("diff_type")]
    public string? DiffType { get; private set; }
}
namespace dpa.Library.Models;

public class Record
{
    [SQLite.Column("date")] public string date { get; set; }
    [SQLite.Column("right")] public int right { get; set; }
    [SQLite.Column("right")] public int wrong { get; set; }
}
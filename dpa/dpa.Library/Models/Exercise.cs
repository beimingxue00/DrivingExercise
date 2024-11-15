using System.Runtime.InteropServices.JavaScript;

namespace dpa.Library.Models;

[SQLite.Table("exercises")]

public class Exercise{
    [SQLite.Column("id")] public int Id { get; set; }//问题id
    [SQLite.Column("question")] public string question { get; set; } = string.Empty;//问题

    [SQLite.Column("answer")] public string answer { get; set; } = string.Empty;//正确答案
    
    [SQLite.Column("user_answer")] public string user_answer { get; set; } = string.Empty;//用户初次答案

    [SQLite.Column("item1")] public string item1 { get; set; } = string.Empty;//四个选项

    [SQLite.Column("item2")] public string item2 { get; set; } = string.Empty;
    
    [SQLite.Column("item3")] public string item3 { get; set; } = string.Empty;
    
    [SQLite.Column("item4")] public string item4 { get; set; } = string.Empty;
    
    [SQLite.Column("explains")] public string explains { get; set; } = string.Empty;//答案选项
    
    [SQLite.Column("url")] public string url { get; set; } = string.Empty;//图片资源

    private string _snippet;

    [SQLite.Ignore] public string Snippet =>
        _snippet ??= question.Split('。')[0].Replace("\r\n", " ");
}
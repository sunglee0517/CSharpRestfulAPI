using System;

public class Qna
{
    public int Qno { get; set; }
    public int Lev { get; set; }
    public int? Parno { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime ResDate { get; set; }
    public int Hits { get; set; }
}
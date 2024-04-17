namespace Quiz.Models;

internal class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Level Level { get; set; }
    public int SubjectId { get; set; }
}
enum Level
{
    HIGH,
    MEDIUM,
    EASY
}


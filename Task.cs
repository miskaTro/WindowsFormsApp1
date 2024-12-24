using System;

public class Task
{
    public int TaskId { get; set; }
    public string ProjectName { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public string Assignee { get; set; }
    public string Status { get; set; }
    public DateTime CreationDate { get; set; }
}

public class TaskStatistics
{
    public int TaskId { get; set; }
    public int CompletionTime { get; set; } // Время выполнения задачи в днях или часах
    public DateTime TaskDate { get; set; }
}

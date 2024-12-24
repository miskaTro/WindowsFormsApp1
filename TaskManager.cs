using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

public class TaskManager
{
    private Database _db = new Database();

    // Метод для добавления новой задачи
    public void AddTask(Task task)
    {
        string query = "INSERT INTO tasks (project_name, task_name, description, priority, assignee, status, creation_date) " +
                       "VALUES (@project_name, @task_name, @description, @priority, @assignee, @status, @creation_date)";

        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("project_name", task.ProjectName),
            new NpgsqlParameter("task_name", task.TaskName),
            new NpgsqlParameter("description", task.Description),
            new NpgsqlParameter("priority", task.Priority),
            new NpgsqlParameter("assignee", task.Assignee),
            new NpgsqlParameter("status", task.Status),
            new NpgsqlParameter("creation_date", task.CreationDate)
        };

        _db.ExecuteQuery(query, parameters);
    }

    // Метод для обновления задачи
    public void UpdateTask(Task task)
    {
        string query = "UPDATE tasks SET project_name = @project_name, task_name = @task_name, description = @description, " +
                       "priority = @priority, assignee = @assignee, status = @status WHERE task_id = @task_id";

        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("task_id", task.TaskId),
            new NpgsqlParameter("project_name", task.ProjectName),
            new NpgsqlParameter("task_name", task.TaskName),
            new NpgsqlParameter("description", task.Description),
            new NpgsqlParameter("priority", task.Priority),
            new NpgsqlParameter("assignee", task.Assignee),
            new NpgsqlParameter("status", task.Status)
        };

        _db.ExecuteQuery(query, parameters);
    }

    // Метод для удаления задачи
    public void DeleteTask(int taskId)
    {
        string query = "DELETE FROM tasks WHERE task_id = @task_id";

        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("task_id", taskId)
        };

        _db.ExecuteQuery(query, parameters);
    }

    // Метод для получения списка задач
    public List<Task> GetTasks(string filterColumn = "", string filterValue = "")
    {
        string query = "SELECT * FROM tasks";

        if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
        {
            query += $" WHERE {filterColumn} = @filterValue";
        }

        var parameters = new List<NpgsqlParameter>();
        if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterValue))
        {
            parameters.Add(new NpgsqlParameter("filterValue", filterValue));
        }

        var dt = _db.ExecuteSelectQuery(query, parameters);

        List<Task> tasks = new List<Task>();
        foreach (DataRow row in dt.Rows)
        {
            tasks.Add(new Task
            {
                TaskId = row.Field<int>("task_id"), // Используем Field<T> для безопасного извлечения
                ProjectName = row.Field<string>("project_name"),
                TaskName = row.Field<string>("task_name"),
                Description = row.Field<string>("description"),
                Priority = row.Field<string>("priority"),
                Assignee = row.Field<string>("assignee"),
                Status = row.Field<string>("status"),
                CreationDate = row.Field<DateTime>("creation_date")
            });
        }

        return tasks;
    }

    // Метод для получения статистики по задачам
    public List<TaskStatistics> GetTaskStatistics(DateTime startDate, DateTime endDate)
    {
        string query = "SELECT * FROM statistics WHERE task_date BETWEEN @startDate AND @endDate";

        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("startDate", startDate),
            new NpgsqlParameter("endDate", endDate)
        };

        var dt = _db.ExecuteSelectQuery(query, parameters);

        List<TaskStatistics> stats = new List<TaskStatistics>();
        foreach (DataRow row in dt.Rows)
        {
            stats.Add(new TaskStatistics
            {
                TaskId = row.Field<int>("task_id"),
                CompletionTime = row.Field<int>("completion_time"),
                TaskDate = row.Field<DateTime>("task_date")
            });
        }

        return stats;
    }
}

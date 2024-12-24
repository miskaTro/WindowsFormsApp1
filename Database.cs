using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

public class Database
{
    // Строка подключения к базе данных
    private string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=22032005;Database=TaskManagerDB;";

    // Метод для получения соединения с базой данных
    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }

    // Вспомогательный метод для выполнения SQL-запросов (например, INSERT, UPDATE, DELETE)
    public void ExecuteQuery(string query, List<NpgsqlParameter> parameters = null)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Метод для выполнения SELECT-запросов и возврата DataTable
    public DataTable ExecuteSelectQuery(string query, List<NpgsqlParameter> parameters = null)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    // Метод для добавления задачи в таблицу
    public void AddTask(string taskNumber, string taskName, string projectName, string description,
                        string priority, string status, string assignee, DateTime creationDate)
    {
        string query = "INSERT INTO tasks (task_number, task_name, project_name, description, priority, status, assignee, creation_date) " +
                       "VALUES (@task_number, @task_name, @project_name, @description, @priority, @status, @assignee, @creation_date)";

        // Список параметров для запроса
        List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@task_number", taskNumber),
            new NpgsqlParameter("@task_name", taskName),
            new NpgsqlParameter("@project_name", projectName),
            new NpgsqlParameter("@description", description),
            new NpgsqlParameter("@priority", priority),
            new NpgsqlParameter("@status", status),
            new NpgsqlParameter("@assignee", assignee),
            new NpgsqlParameter("@creation_date", creationDate)
        };

        // Выполнение запроса
        ExecuteQuery(query, parameters);
    }

    // Метод для обновления задачи в таблице
    public void UpdateTask(int taskId, string taskNumber, string taskName, string projectName, string description,
                           string priority, string status, string assignee, DateTime creationDate)
    {
        string query = "UPDATE tasks SET task_number = @task_number, task_name = @task_name, project_name = @project_name, " +
                       "description = @description, priority = @priority, status = @status, assignee = @assignee, " +
                       "creation_date = @creation_date WHERE task_id = @task_id";

        // Список параметров для запроса
        List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@task_id", taskId),
            new NpgsqlParameter("@task_number", taskNumber),
            new NpgsqlParameter("@task_name", taskName),
            new NpgsqlParameter("@project_name", projectName),
            new NpgsqlParameter("@description", description),
            new NpgsqlParameter("@priority", priority),
            new NpgsqlParameter("@status", status),
            new NpgsqlParameter("@assignee", assignee),
            new NpgsqlParameter("@creation_date", creationDate)
        };

        // Выполнение запроса
        ExecuteQuery(query, parameters);
    }

    // Метод для удаления задачи из таблицы
    public void DeleteTask(int taskId)
    {
        string query = "DELETE FROM tasks WHERE task_id = @task_id";

        // Список параметров для запроса
        List<NpgsqlParameter> parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@task_id", taskId)
        };

        // Выполнение запроса
        ExecuteQuery(query, parameters);
    }

    // Метод для получения всех задач из базы данных
    public DataTable GetAllTasks()
    {
        string query = "SELECT * FROM tasks";
        return ExecuteSelectQuery(query);
    }
}

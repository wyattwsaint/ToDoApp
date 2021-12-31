using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp
{
    internal class Program
    {
        static List<string> taskList = new List<string>();
        static void Main(string[] args)
        {
            getTasksFromDataBase();
            addTask();
            deleteTask();
            showAllTasks();
        }

        static void addTask()
        {
            wipeDataBase();
            string task = "*";
            while (task != "n")
            {
                Console.WriteLine("Please enter a task or press \"n\" for next: "); 
                task = Console.ReadLine();
                taskList.Add(task);
            }
        }

        static void deleteTask()
        {
            string task = "*";
            while (task != "n")
            {
                Console.WriteLine("Please enter a task to delete or press \"n\" for next: ");
                task = Console.ReadLine();
                taskList.Remove(task);
            }
        }

        static void showAllTasks()
        {
            foreach (string task in taskList)
                Console.WriteLine(task);
            foreach (string task in taskList)
                addTaskToDataBase(task);

            Console.ReadLine();
        }

        static void addTaskToDataBase(string task)
        {
            using (var conn = new MySqlConnection("Server=localhost;User ID=root;Password=root;Database=todolist"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into tasks(tasks) values ('" + task + "')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void wipeDataBase()
        {
            using (var conn = new MySqlConnection("Server=localhost;User ID=root;Password=root;Database=todolist"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "create or replace table tasks (id int auto_increment primary key,tasks varchar(100))";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void getTasksFromDataBase()
        {
            using (var conn = new MySqlConnection("Server=localhost;User ID=root;Password=root;Database=todolist"))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT tasks FROM tasks", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        taskList.Add(reader.GetString("tasks"));

            }
        }
    }
}

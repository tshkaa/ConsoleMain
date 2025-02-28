using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConsoleMain;

class Program
{
    class Task 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string title, string description)
        {
            this.Title = title;
            this.Description = description;
            IsCompleted = false;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[*]" : "[ ]";
            return $"{status} {Title}: {Description}";
        }
    }
    
    static List<Task> tasks = new List<Task>();
    
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Планировщик");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Показать все задачи");
            Console.WriteLine("3. Отметить задачу как выполненную");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("5. Показать выполненные задачи");
            Console.WriteLine("6. Показать невыполненные задачи");
            Console.WriteLine("7. Сохранить данные в файл");
            Console.WriteLine("8. Загрузить данные из файла");
            Console.WriteLine("9. Выйти");
            Console.WriteLine("----------------------------------");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ShowTasks();
                    break;
                case "3":
                    MarkCompleted();
                    break;
                case "4":
                    DeleteTask();
                    break;
                case "5":
                    ShowCompleted();
                    break;
                case "6":
                    ShowNCompleted();
                    break;
                case "7":
                    SaveTasks();
                    break;
                case "8":
                    LoadTasks();
                    break;
                case "9":
                    return;
                default:
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.\n");
                    break;
            }
            
            ExitToMenu();
        }
    }

    static void AddTask()
    {
        Console.Clear();
        
        Console.Write("Введите название: ");
        string title = Console.ReadLine();
        Console.Write("Введите описание: ");
        string description = Console.ReadLine();
        
        tasks.Add(new Task(title, description));
        Console.Clear();
        Console.WriteLine("Задача успешно добавлена.");
    }

    static void ShowTasks()
    {
        Console.Clear();
        
        if (tasks.Count == 0)
        {
            Console.WriteLine("Задач нет.");
            Thread.Sleep(1000);
            Console.Clear();
            return;
        }
        
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
    }

    static void MarkCompleted()
    {
        Console.Clear();
        ShowTasks();
        Console.WriteLine("----------------------------------");
        Console.Write("Введите номер задачи: ");

        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
        {
            tasks[index - 1].IsCompleted = true;
            Console.Clear();
            Console.WriteLine("Задача отмечена как выполненная.");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Некорректный ввод.");
        }
    }

    static void DeleteTask()
    {
        Console.Clear();
        ShowTasks();
        Console.WriteLine("----------------------------------");
        Console.Write("Введите номер задачи: ");
        
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
        {
            tasks.RemoveAt(index - 1);
            
            Console.WriteLine("Задача удалена.");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Некорректный ввод.");
        }
    }

    static void ShowCompleted()
    {
        var completed = tasks.Where(t => t.IsCompleted).ToList();
        
        Console.Clear();
        
        if (completed.Count == 0)
        {
            Console.WriteLine("Выполненных задач нет.");
            return;
        }
        
        foreach (var task in completed)
        {
            Console.WriteLine(task);
        }
    }

    static void ShowNCompleted()
    {
        var ncompleted = tasks.Where(t => !t.IsCompleted).ToList();

        Console.Clear();
        
        if (ncompleted.Count == 0)
        {
            Console.WriteLine("Невыполненных задач нет.");
            return;
        }
        
        foreach (var task in ncompleted)
        {
            Console.WriteLine(task);
        }
    }

    static void SaveTasks()
    {
        Console.Clear();
        
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText("tasks.json", json);
        Console.WriteLine("Файлы сохранены (tasks.json).\n");
    }

    static void LoadTasks()
    {
        Console.Clear();
        
        if (File.Exists("tasks.json"))
        {
            string json = File.ReadAllText("tasks.json");
            tasks = JsonConvert.DeserializeObject<List<Task>>(json);
            Console.WriteLine("Задачи успешно импортированы.\n");
        }
        else
        {
            Console.WriteLine("Файл не найден.\n");
        }
    }

    static void ExitToMenu()
    {
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Нажмите любую клавишу чтобы вернуться в главное меню.");
        Console.ReadKey();
        Console.Clear();
    }
}
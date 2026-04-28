using SL.App.Services;
using SL.Cli;

namespace SL.Cli.Managers;

public class TeacherManager
{
    private readonly TeacherService _teacherService;

    public TeacherManager(TeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            MinorMethods.PrintHeader("TEACHERS MANAGER");
            Console.WriteLine("1. Hire teacher");
            Console.WriteLine("2. Dismiss teacher");
            Console.WriteLine("3. Update teacher");
            Console.WriteLine("4. List all teachers");
            Console.WriteLine("5. Check information about teacher by Id");
            Console.WriteLine("0. Return");
            Console.Write("\n> Action: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "0") break;
            try

            {
                switch (choice)
                {
                    case "1": HireTeacher(); break;
                    case "2": DismissTeacher(); break;
                    case "3": UpdateTeacher(); break;
                    case "4": ListAllTeachers(); break;
                    case "5": CheckByIdTeacher(); break;
                    default:
                        MinorMethods.ShowError("Invalid option. Try again.");
                        break;

                }
            }
            catch (Exception ex)
            {
                MinorMethods.ShowError(ex.Message);
            }
        }
    }

    private Guid? GetTeacherIdFromConsole()
    {
        Console.Write("\nEnter teacher Id: ");

        if (Guid.TryParse(Console.ReadLine()?.Trim(), out Guid id))
        {
            return id;
        }

        MinorMethods.ShowError("Invalid ID format.");
        return null;
    }

    private void HireTeacher()
    {
        Console.Write("\nFirst name: ");
        string firstName = Console.ReadLine() ?? "Unknown";

        Console.Write("\nLast name: ");
        string lastName = Console.ReadLine() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
        {
            MinorMethods.ShowError("Name cannot be empty.");
            return;
        }

        _teacherService.Create(firstName, lastName);
        MinorMethods.ShowSuccess("Teacher hired.");
    }

    private void DismissTeacher()
    {
        Guid? id = GetTeacherIdFromConsole();
        if (id is null) return;

        _teacherService.Delete(id.Value);
        MinorMethods.ShowSuccess("Teacher dismissed.");
    }

    private void UpdateTeacher()
    {
        Guid? id = GetTeacherIdFromConsole();
        if (id is null) return;

        var teacher = _teacherService.GetById(id.Value);

        if (teacher is null)
        {
            MinorMethods.ShowError("Teacher not found.");
            return;
        }

        Console.WriteLine($"\nEditing Teacher: {teacher.FirstName} {teacher.LastName}");
        Console.WriteLine("- Leave field empty to keep current value.\n");

        Console.Write("New first name: ");
        string inputFirstName = Console.ReadLine() ?? string.Empty;
        string newFirstName;

        if (string.IsNullOrWhiteSpace(inputFirstName))
        {
            newFirstName = teacher.FirstName;
        }
        else
        {
            newFirstName = inputFirstName;
        }

        Console.Write("New last name: ");
        string inputLastName = Console.ReadLine() ?? string.Empty;
        string newLastName;

        if (string.IsNullOrWhiteSpace(inputLastName))
        {
            newLastName = teacher.LastName;
        }
        else
        {
            newLastName = inputLastName;
        }

        teacher.UpdateInfo(newFirstName, newLastName);
        _teacherService.Update(teacher);
        MinorMethods.ShowSuccess($"Updated successfully\n- Current value: {teacher.FirstName} {teacher.LastName}.");
    }

    private void ListAllTeachers()
    {
        var allTeachers = _teacherService.GetAll().ToList();
        int count = 0;

        if (!allTeachers.Any())
        {
            MinorMethods.ShowError("List is empty.");
            return;
        }

        Console.Clear();
        MinorMethods.PrintHeader("ALL TEACHERS");
        Console.WriteLine($"{"№",-3} | {"ID",-36} | {"First name",-15} | {"Last name",-15} | {"Status"}");
        Console.WriteLine(new string('-', 90));

        foreach (var teacher in allTeachers)
        {
            string status = teacher.IsBusy ? "Busy" : "Free";
            Console.WriteLine($"{count + 1,-3} | {teacher.Id,-36} | {teacher.FirstName,-15} | {teacher.LastName,-15} | {status}");
            count++;
        }

        MinorMethods.ShowSuccess("List provided.");
    }

    private void CheckByIdTeacher()
    {
        Guid? id = GetTeacherIdFromConsole();
        if (id is null) return;

        var teacher = _teacherService.GetById(id.Value);

        if (teacher != null)
        {
            Console.Clear();
            MinorMethods.PrintHeader($"TEACHER {teacher.FirstName} {teacher.LastName}");
            Console.WriteLine($" Status     : {(teacher.IsBusy ? "Busy" : "Free")}");
            MinorMethods.ShowSuccess("Information provided.");
        }
        else
        {
            MinorMethods.ShowError("Teacher not found.");
            return;
        }


    }
}
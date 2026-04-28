using SL.App.Services;
using SL.Cli;
using SL.Domain.Entities;

namespace SL.Cli.Managers;

public class GroupManager
{
    private readonly GroupService _groupService;

    public GroupManager(GroupService groupService)
    {
        _groupService = groupService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            MinorMethods.PrintHeader(" GROUPS MANAGER ");
            Console.WriteLine("1. Create a group");
            Console.WriteLine("2. Delete existing group");
            Console.WriteLine("3. Update existing group");
            Console.WriteLine("4. List all existing groups");
            Console.WriteLine("5. Check information about group by ID\n");
            MinorMethods.PrintHeader("STUDENTS & GRADES ");
            Console.WriteLine("6. Add student to group");
            Console.WriteLine("7. View group composition");
            Console.WriteLine("8. View gradebook (Journal)");
            Console.WriteLine("0. Return");
            Console.Write("\n> Action: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "0") break;

            try
            {
                switch (choice)
                {
                    case "1": CreateGroup(); break;

                    case "2": DeleteGroup(); break;

                    case "3": UpdateGroup(); break;

                    case "4": ListAllGroups(); break;

                    case "5": CheckByIdGroup(); break;

                    case "6": AddStudentToGroup(); break;

                    case "7": ViewGroupComposition(); break;

                    case "8": ViewGradeBook(); break;

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

    private Guid? GetGroupIdFromConsole()
    {
        Console.Write("\nEnter group ID: ");
        if (Guid.TryParse(Console.ReadLine()?.Trim(), out Guid id))
        {
            return id;
        }
        MinorMethods.ShowError("Invalid ID format.");
        return null;
    }

    private void CreateGroup()
    {
        Console.Write("\nGroup name: ");
        string name = Console.ReadLine() ?? "Unknown";

        if (string.IsNullOrWhiteSpace(name))
        {
            MinorMethods.ShowError("Name cannot be empty");
            return;
        }

        Console.Write("Group study year: ");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            MinorMethods.ShowError("Invalid year format");
            return;
        }

        if (year > 5 || year <= 0)
        {
            MinorMethods.ShowError($"Invalid year {year}. Must be between 1 and 5.");
            return;
        }

        _groupService.Create(name, year);
        MinorMethods.ShowSuccess("Group created");
    }

    private void DeleteGroup()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        _groupService.Delete(id.Value);
        MinorMethods.ShowSuccess("Group deleted");
    }

    private void UpdateGroup()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        var group = _groupService.GetById(id.Value);

        if (group is null)
        {
            MinorMethods.ShowError("Group not found");
            return;
        }

        Console.WriteLine($"Editing Group: {group.Name} (Year {group.Year})");
        Console.WriteLine("Leave field empty to keep current value.");

        Console.Write("New Name: ");
        string inputName = Console.ReadLine() ?? string.Empty;
        string newName;

        if (string.IsNullOrWhiteSpace(inputName))
        {
            newName = group.Name;
        }
        else
        {
            newName = inputName;
        }

        Console.Write("New Year: ");
        string inputYear = Console.ReadLine() ?? string.Empty;
        int newYear = group.Year;

        if (!string.IsNullOrWhiteSpace(inputYear))
        {
            if (!int.TryParse(inputYear, out newYear))
            {
                MinorMethods.ShowError("Invalid year format. Update cancelled.");
                return;
            }
        }

        group.UpdateInfo(newName, newYear);

        _groupService.Update(group);

        MinorMethods.ShowSuccess("Group updated successfully.");

    }

    private void ListAllGroups()
    {
        var allGroups = _groupService.GetAll().ToList();
        int count = 0;

        if (!allGroups.Any())
        {
            MinorMethods.ShowError("List is empty.");
            return;
        }

        Console.Clear();
        MinorMethods.PrintHeader("ALL GROUPS");
        Console.WriteLine($"{"№",-3} | {"ID",-36} | {"Name",-15} | {"Year",-4} | {"Size",-4} | {"Status"}");
        Console.WriteLine(new string('-', 75));

        foreach (var group in allGroups)
        {
            string status = group.IsBusy ? "Busy" : "Free";
            Console.WriteLine($"{count + 1,-3} | {group.Id,-36} | {group.Name,-15} | {group.Year,-4} | {group.Size,-4} | {status}");
            count++;
        }

        MinorMethods.ShowSuccess("List of groups provided");
    }

    private void CheckByIdGroup()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        var group = _groupService.GetById(id.Value);

        if (group != null)
        {
            Console.Clear();
            MinorMethods.PrintHeader($"GROUP {group.Name}");
            Console.WriteLine($" Study Year : {group.Year}");
            Console.WriteLine($" Status     : {(group.IsBusy ? "Busy" : "Free")}");
            Console.WriteLine($" Students   : {group.Size}");
            MinorMethods.ShowSuccess("Group information provided");
        }
        else
        {
            MinorMethods.ShowError("Group not found");
            return;
        }
    }

    private void AddStudentToGroup()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        Console.WriteLine();
        Console.Write("First name : ");
        string firstName = Console.ReadLine() ?? string.Empty;

        Console.Write("Last name : ");
        string lastName = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            MinorMethods.ShowError("Name cannot be empty");
            return;
        }

        _groupService.AddStudentToGroup(id.Value, firstName, lastName);
        MinorMethods.ShowSuccess("Student added");
    }

    private void ViewGroupComposition()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        var groupComposition = _groupService.GetAllStudentsInGroup(id.Value).ToList();

        if (!groupComposition.Any())
        {
            MinorMethods.ShowError("Group is empty");
            return;
        }

        int count1 = 0;
        foreach (var student in groupComposition)
        {
            Console.WriteLine($"{count1 + 1}. {student.FirstName} {student.LastName}");
            count1++;
        }

        MinorMethods.ShowSuccess("Group composition provided");
    }

    private void ViewGradeBook()
    {
        Guid? id = GetGroupIdFromConsole();
        if (id is null) return;

        var group = _groupService.GetById(id.Value);

        if (group == null)
        {
            MinorMethods.ShowError("Group not found.");
            return;
        }

        MinorMethods.PrintHeader($" GRADEBOOK: {group.Name} ");

        if (!group.GradeBook.Any())
        {
            MinorMethods.ShowError("The gradebook is currently empty (No exams or credits taken yet).");
            return;
        }


        foreach (var record in group.GradeBook)
        {
            Console.WriteLine($"- Discipline: {record.Key}\n- Score: {record.Value}");
        }

        MinorMethods.ShowSuccess("Gradebook provided");
    }
}
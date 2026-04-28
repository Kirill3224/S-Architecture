

using Microsoft.VisualBasic;
using SL.App.Services;

namespace SL.Cli.Managers;

public class SimulationManager
{
    private readonly SimulationService _simulationService;
    private readonly DisciplineService _disciplineService;
    private readonly GroupService _groupService;

    public SimulationManager(SimulationService simulationService, DisciplineService disciplineService, GroupService groupService)
    {
        _simulationService = simulationService;
        _disciplineService = disciplineService;
        _groupService = groupService;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            MinorMethods.PrintHeader("STUDY SIMULATION");
            Console.WriteLine("1. Start a lesson");
            Console.WriteLine("2. Take an exam");
            Console.WriteLine("3. Take a credit");
            Console.WriteLine("4. View a grade book");
            Console.WriteLine("0. Return");
            Console.Write("\n> Action: ");

            string choice = Console.ReadLine() ?? string.Empty;
            if (choice == "0") break;
            try
            {

                switch (choice)
                {
                    case "1": StartLesson(); break;
                    case "2": TakeExam(); break;
                    case "3": TakeCredit(); break;
                    case "4": GradeBookView(); break;
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

    private void StartLesson()
    {
        Console.Clear();
        MinorMethods.PrintHeader("SELECT GROUP");
        Console.Write("Enter Group ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid groupId))
        {
            MinorMethods.ShowError("Invalid ID format");
            return;
        }

        Console.Clear();
        MinorMethods.PrintHeader("SELECT TEACHER");
        Console.Write("Enter Teacher ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid teacherId))
        {
            MinorMethods.ShowError("Invalid ID format");
            return;
        }

        Console.Clear();
        MinorMethods.PrintHeader("SELECT DISCIPLINE AND ACTIVITIES");

        var allDisciplines = _disciplineService.GetAll().ToList();

        if (!allDisciplines.Any())
        {
            MinorMethods.ShowError("No disciplines available in the system.");
            return;
        }

        Console.WriteLine("\n- Available disciplines: ");

        int count = 0;
        foreach (var discipline in allDisciplines)
        {
            Console.WriteLine($"{count + 1}. {discipline.Name}");
            count++;
        }

        Console.Write("\nSelect Discipline number: ");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > allDisciplines.Count)
        {
            MinorMethods.ShowError("Invalid discipline selection.");
            return;
        }

        var selectedDiscipline = allDisciplines[choice - 1];

        var activities = selectedDiscipline.Activities.ToList();

        if (!activities.Any())
        {
            MinorMethods.ShowError($"Discipline '{selectedDiscipline.Name}' has no activities.");
            return;
        }

        Console.WriteLine($"\n- Activities for {selectedDiscipline.Name}: ");

        count = 0;

        foreach (var activity in activities)
        {
            Console.WriteLine($"{count + 1}. {activity.Name}");
            count++;
        }

        Console.Write("\nSelect Activity number: ");
        if (!int.TryParse(Console.ReadLine(), out int actChoice) || actChoice < 1 || actChoice > activities.Count)
        {
            MinorMethods.ShowError("Invalid activity selection.");
            return;
        }

        var selectedActivity = activities[actChoice - 1];

        try
        {
            _simulationService.ConductLesson(teacherId, groupId, selectedDiscipline.Name, selectedActivity.Name);

            Console.Clear();
            Console.WriteLine("\nInitializing lesson... please wait.");
            for (int i = 0; i <= 20; i++)
            {
                Console.Write($"\r[LESSON IN PROGRESS] [{new string('#', i)}{new string('.', 20 - i)}] {i * 5}%");
                Thread.Sleep(250);
            }
            Console.WriteLine();

            MinorMethods.ShowSuccess($"\nLesson '{selectedActivity.Name}' on '{selectedDiscipline.Name}' conducted successfully!");
        }
        catch (Exception ex)
        {
            MinorMethods.ShowError($"\nLesson failed: {ex.Message}");
            return;
        }
    }

    private void TakeExam()
    {
        Console.Clear();
        MinorMethods.PrintHeader("EXAMINATION");

        Console.Write("Enter Group ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid groupId))
        {
            MinorMethods.ShowError("Invalid ID format.");
            return;
        }

        var allDisciplines = _disciplineService.GetAll()
            .Where(d => d.Activities.Any(a => a.Name == "Exam"))
            .ToList();

        int count = 0;

        if (!allDisciplines.Any())
        {
            MinorMethods.ShowError("No disciplines available in the system.");
        }

        foreach (var discipline in allDisciplines)
        {
            Console.WriteLine($"{count + 1}. {discipline.Name}");
            count++;
        }

        Console.Write("\nSelect Discipline number: ");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > allDisciplines.Count)
        {
            MinorMethods.ShowError("Invalid discipline selection.");
            return;
        }

        var selectedDiscipline = allDisciplines[choice - 1];

        try
        {
            int finalGrade = _simulationService.ConductExam(groupId, selectedDiscipline.Name);

            Console.Clear();
            Console.WriteLine("\nTesting students... please wait.");
            for (int i = 0; i <= 10; i++)
            {
                Console.Write($"\r[EXAM IN PROGRESS] [{new string('#', i)}{new string('.', 10 - i)}] {i * 10}%");
                Thread.Sleep(300);
            }
            Console.WriteLine();

            MinorMethods.ShowSuccess($"\nExam successful!\nGroup's score in {selectedDiscipline.Name}: {finalGrade}");
        }
        catch (Exception ex)
        {
            MinorMethods.ShowError(ex.Message);
        }
    }

    private void TakeCredit()
    {
        Console.Clear();
        MinorMethods.PrintHeader("TAKE A CREDIT");

        Console.Write("Enter Group ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid groupId))
        {
            MinorMethods.ShowError("Invalid ID format.");
            return;
        }

        var disciplinesWithCredit = _disciplineService.GetAll()
            .Where(d => d.Activities.Any(a => a.Name == "Credit"))
            .ToList();

        if (!disciplinesWithCredit.Any())
        {
            MinorMethods.ShowError("No disciplines with a Credit available in the system.");
            return;
        }

        Console.WriteLine("\n- Available disciplines for Credit: ");

        int count = 0;
        foreach (var discipline in disciplinesWithCredit)
        {
            Console.WriteLine($"{count + 1}. {discipline.Name}");
        }

        Console.Write("\nSelect Discipline (number): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > disciplinesWithCredit.Count)
        {
            MinorMethods.ShowError("Invalid discipline selection.");
            return;
        }

        var selectedDiscipline = disciplinesWithCredit[choice - 1];

        try
        {
            int finalGrade = _simulationService.ConductExam(groupId, selectedDiscipline.Name);

            Console.Clear();
            Console.WriteLine("\nEvaluating students' completed works... please wait.");
            for (int i = 0; i <= 10; i++)
            {
                Console.Write($"\r[CHECKING] [{new string('#', i)}{new string('.', 10 - i)}] {i * 10}%");
                Thread.Sleep(200);
            }
            Console.WriteLine();

            MinorMethods.ShowSuccess($"\nCredit successful!\nGroup passed '{selectedDiscipline.Name}' with score: {finalGrade}");
        }
        catch (Exception ex)
        {
            MinorMethods.ShowError(ex.Message);
        }
    }

    private void GradeBookView()
    {
        Console.Clear();
        MinorMethods.PrintHeader("GRADE JOURNAL");

        Console.Write("Enter Group ID: ");
        if (!Guid.TryParse(Console.ReadLine(), out Guid groupId))
        {
            MinorMethods.ShowError("Invalid ID format");
            return;
        }

        var group = _groupService.GetById(groupId);

        if (group is null)
        {
            MinorMethods.ShowError("Group not found");
            return;
        }

        Console.WriteLine($"\nJournal for Group: {group.Name} (Year {group.Year})");
        Console.WriteLine(new string('-', 40));

        if (!group.GradeBook.Any())
        {
            MinorMethods.ShowError("Grade book is empty. No exams or credits taken yet.");
            return;
        }

        else
        {
            foreach (var record in group.GradeBook)
            {
                string gradeDisplay = record.Value == 100 ? $"{record.Value} (Passed / Credit)" : record.Value.ToString();
                Console.WriteLine($"- {record.Key}: {gradeDisplay}");
            }
        }

        Console.WriteLine(new string('-', 40));

        Console.WriteLine("\nCompleted Works (Attendance):");
        if (!group.CompletedWorks.Any())
        {
            MinorMethods.ShowError("No works completed yet.");
            return;
        }

        else
        {
            var worksByDiscipline = group.CompletedWorks
                .GroupBy(w => w.Split(':')[0])
                .ToList();

            foreach (var w in worksByDiscipline)
            {
                Console.WriteLine($"- {w.Key}: {w.Count()} works done");
            }
        }

        MinorMethods.ShowSuccess("Grade book provided");
    }

}
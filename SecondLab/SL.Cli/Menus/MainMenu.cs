using SL.Cli;
using SL.Cli.Managers;
using SL.Domain.Common;
using SL.Domain.Interfaces;
using SL.Domain.Factories;
using SL.Domain.Entities;

namespace SL.App.Menus;

public class MainMenu
{
    private readonly GroupManager _groupManager;
    private readonly TeacherManager _teacherManager;
    private readonly SimulationManager _simulationManager;
    private readonly IRepository<Equipment> _equipRepo;

    public MainMenu(GroupManager groupManager, TeacherManager teacherManager, SimulationManager simulationManager, IRepository<Equipment> equipRepo)
    {
        _groupManager = groupManager;
        _teacherManager = teacherManager;
        _simulationManager = simulationManager;
        _equipRepo = equipRepo;
    }
    public void Show()
    {
        Console.Title = "Stduy Simulation System";
        CreateEquipment();

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"--- Study Simulation System ---");
            Console.ResetColor();

            Console.WriteLine("1. Manage study groups");
            Console.WriteLine("2. Manage teachers");
            Console.WriteLine("3. Enter study simulation");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect an option: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "0") break;

            switch (choice)
            {
                case "1": _groupManager.Run(); break;
                case "2": _teacherManager.Run(); break;
                case "3": _simulationManager.Run(); break;

                default:
                    MinorMethods.ShowError("Invalid option. Try again.");
                    break;
            }
        }
    }

    public void CreateEquipment()
    {
        var allEquipment = _equipRepo.GetAll().ToList();

        if (!allEquipment.Any())
        {
            for (int i = 0; i < 5; i++)
            {
                _equipRepo.Add(new Computer());
                _equipRepo.Add(new AudioSystem());
            }
        }
    }
}
using SL.App.Menus;
using SL.App.Services;
using SL.Cli.Managers;
using SL.Domain.Common;
using SL.Domain.Entities;
using SL.Domain.Factories;
using SL.Infra.Repositories;

namespace SL.Cli;

class Program
{
    static void Main()
    {
        try
        {
            var langFactory = new LanguageCourseFactory();
            var teachFactory = new TechnicalCourseFactory();

            var groupRepo = new JsonRepository<StudentGroup>("groups.json");
            var teacherRepo = new JsonRepository<Teacher>("teachers.json");
            var equipmentRepo = new JsonRepository<Equipment>("equipments.json");

            var groupService = new GroupService(groupRepo);
            var teacherService = new TeacherService(teacherRepo);
            var disciplineService = new DisciplineService(teachFactory, langFactory);
            var simulationService = new SimulationService(teacherRepo, groupRepo, equipmentRepo, disciplineService);

            var groupManager = new GroupManager(groupService);
            var teacherManager = new TeacherManager(teacherService);
            var simulationManager = new SimulationManager(simulationService, disciplineService, groupService);

            var mainMenu = new MainMenu(groupManager, teacherManager, simulationManager, equipmentRepo);

            mainMenu.Show();
        }
        catch (Exception ex)
        {
            MinorMethods.ShowError($"Cannot launch program {ex.Message}");
        }
    }
}

using System.ComponentModel.DataAnnotations;
using SL.Domain.Common;
using SL.Domain.Entities;

namespace SL.Domain.Validators;

public static class StudyValidator
{
    public static void ValidateGroup(StudentGroup group, Activity activity, Discipline discipline)
    {
        if (activity.IsRequireSplit && group.SubgroupSize < 10)
            throw new ValidationException($"Subgroup size {group.SubgroupSize} is too small");

        if (activity.Name == "Exam" || activity.Name == "Modular Tests")
        {
            int subjectWorksCount = group.CompletedWorks.Count(w => w.StartsWith(discipline.Name));

            if (subjectWorksCount < 2)
            {
                throw new ValidationException(
                    $"Group {group.Name} not admitted to {activity.Name}. " +
                    $"Submitted works for {discipline.Name}: {subjectWorksCount} (Need 2).");
            }
        }

        if (!discipline.CanBeStudiedBy(group))
            throw new ValidationException($"Group {group.Name} cannot study {discipline.Name}");
    }

    public static void ValidateTeacher(Teacher teacher)
    {
        if (teacher.IsBusy)
            throw new ValidationException($"{teacher.FirstName} is busy");
    }

    public static void ValidateExamResult(StudentGroup group, string disciplineName, int minScore = 60)
    {
        if (!group.GradeBook.TryGetValue(disciplineName, out int score))
        {
            throw new ValidationException($"Group {group.Name} did not receive a grade for '{disciplineName}'. Make sure they have attended lessons and submitted works!");
        }

        if (score < minScore)
        {
            throw new ValidationException($"Group {group.Name} failed '{disciplineName}'. Current grade: {score}. Min grade: {minScore}.");
        }
    }

    public static List<Equipment> GetRequiredEquipmentOrThrow(Discipline discipline, IEnumerable<Equipment> availableEquip)
    {
        var foundEquipment = new List<Equipment>();
        var usedIds = new HashSet<Guid>();

        foreach (var req in discipline.RequiredEquipment)
        {
            var freeItem = availableEquip.FirstOrDefault(e =>
                e.GetType() == req.GetType() &&
                !e.IsBusy &&
                !usedIds.Contains(e.Id)
            );

            if (freeItem == null)
            {
                throw new ValidationException($"Missing required equipment: {req.Name} for {discipline.Name}");
            }

            foundEquipment.Add(freeItem);
            usedIds.Add(freeItem.Id);
        }

        return foundEquipment;
    }
}
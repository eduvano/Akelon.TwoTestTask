using Akelon.TwoTestTask;
using Akelon.TwoTestTask.Dto;
using BetterConsoles.Tables;
using BetterConsoles.Tables.Configuration;

var employees = GetEmployees();
var employeesVacations = VacationService.GetVacations(employees);

PrintVacations(employeesVacations);

List<EmployeeDto> GetEmployees()
{
    return
    [
        new() 
        {
            Code = 1,
            FirstName = "Иван",
            LastName = "Иванов",
            MiddleName = "Иванович"
        },
        new() 
        {
            Code = 2,
            FirstName = "Иван2",
            LastName = "Иванов2",
            MiddleName = "Иванович2"
        },
        new() 
        {
            Code = 3,
            FirstName = "Иван3",
            LastName = "Иванов3",
            MiddleName = "Иванович3"
        },
        new() 
        {
            Code = 4,
            FirstName = "Иван4",
            LastName = "Иванов4",
            MiddleName = "Иванович4"
        },
        new()
        {
            Code = 5,
            FirstName = "Иван4",
            LastName = "Иванов4",
            MiddleName = "Иванович4"
        },
        new()
        {
            Code = 6,
            FirstName = "Иван4",
            LastName = "Иванов4",
            MiddleName = "Иванович4"
        },
        new()
        {
            Code = 7,
            FirstName = "Иван4",
            LastName = "Иванов4",
            MiddleName = "Иванович4"
        }
    ];
}

void PrintVacations(Dictionary<int, List<VacationDto>> employeesVacations)
{
    var table = new Table("Работник", "Количество", "Даты отпусков", "Дней");
    table.Config = TableConfig.Unicode();
    foreach (var employeeVacation in employeesVacations)
    {
        var firstVacation = employeeVacation.Value.First();
        var firstRowDates = $"{firstVacation.StartDate} - {firstVacation.EndDate}";
        table.AddRow(employeeVacation.Key, employeeVacation.Value.Count, firstRowDates, firstVacation.TotalDays);
        for (int i = 1; i < employeeVacation.Value.Count; i++)
            table.AddRow(string.Empty, string.Empty, $"{employeeVacation.Value[i].StartDate} - {employeeVacation.Value[i].EndDate}", employeeVacation.Value[i].TotalDays);

        table.AddRow(string.Empty, string.Empty, string.Empty, string.Empty);
    }

    Console.WriteLine(table);
}
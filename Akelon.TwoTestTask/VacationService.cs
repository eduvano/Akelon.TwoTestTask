using Akelon.TwoTestTask.Dto;

namespace Akelon.TwoTestTask;

/// <summary>
/// Сервис отпусков.
/// </summary>
public class VacationService
{
    /// <summary>
    /// Возможная непрерывная последовательность дней в отпуске.
    /// </summary>
    private static readonly int[] _daysInVacation = [7, 14];

    /// <summary>
    /// Всего количество дней отпуска в году.
    /// </summary>
    private const int VACATION_DAYS_PER_YEAR = 28;

    /// <summary>
    /// Пауза между отпусками разных работников в днях.
    /// </summary>
    private const int DAYS_BETWEEN_VACATIONS = 3;

    /// <summary>
    /// Пауза между отпусками одного работника в месяцах.
    /// </summary>
    private const int MONTH_BETWEEN_VACATIONS_EMPLOYEE = 1;

    /// <summary>
    /// дней в году.
    /// </summary>
    private const int YEAR_DAYS = 365;

    /// <summary>
    /// Получить отпуска для сотрудников без пересечений.
    /// </summary>
    /// <param name="employees"> Сотрудники.</param>
    /// <returns> Отпуска работников, key - код работника, value - отпуска сотрудника.</returns>
    public static Dictionary<int, List<VacationDto>> GetVacations(List<EmployeeDto> employees)
    {
        if (employees.Count * VACATION_DAYS_PER_YEAR > YEAR_DAYS)
            throw new ArgumentException("Невозможно рассчитать отпуска без пересечений для переданного количества сотрудников");

        var initEmployeesVacations = employees.ToDictionary(e => e.Code, e => new List<VacationDto>());
        var employeesVacations = new Dictionary<int, List<VacationDto>>(initEmployeesVacations);

        SetEmployeeVacations(employeesVacations);

        return employeesVacations;
    }

    /// <summary>
    /// Заполнить словарь сотрудников отпусками.
    /// </summary>
    /// <param name="employeesVacations"> Словарь сотрудников.</param>
    private static void SetEmployeeVacations(Dictionary<int, List<VacationDto>> employeesVacations)
    {
        var initVacationBalance = employeesVacations.Select(e => new KeyValuePair<int, int>(e.Key, VACATION_DAYS_PER_YEAR));
        var employeeVacationBalances = new Queue<KeyValuePair<int, int>>(initVacationBalance);

        var availableStartDate = new DateOnly(DateTime.Now.Year, 1, 1);
        while (employeeVacationBalances.Count != 0)
        {
            var vacationBalance = employeeVacationBalances.Dequeue();

            var employeeCode = vacationBalance.Key;
            var vacationDaysBalance = vacationBalance.Value;
            var lastVacationEndDate = employeesVacations[employeeCode].LastOrDefault()?.EndDate ?? DateOnly.MinValue;

            var employeeVacation = CalcVacation(vacationDaysBalance, lastVacationEndDate, availableStartDate);
            employeesVacations[employeeCode].Add(employeeVacation);

            availableStartDate = availableStartDate.AddDays(employeeVacation.TotalDays + DAYS_BETWEEN_VACATIONS);

            vacationDaysBalance -= employeeVacation.TotalDays;
            if (vacationDaysBalance != 0)
                employeeVacationBalances.Enqueue(new KeyValuePair<int, int>(employeeCode, vacationDaysBalance));
        }
    }

    /// <summary>
    /// Рассчитать отпуск.
    /// </summary>
    /// <param name="vacationDaysBalance"> Оставшиеся количество дней отпуска.</param>
    /// <param name="lastVacationEndDate"> Дата окончания последнего отпуска.</param>
    /// <param name="availableStartDate"> Последняя свободная дата начала отпуска.</param>
    /// <returns> Отпуск.</returns>
    private static VacationDto CalcVacation(int vacationDaysBalance, DateOnly lastVacationEndDate, DateOnly availableStartDate)
    {
         availableStartDate = MoveDateFromWeekend(availableStartDate);

        if (lastVacationEndDate != DateOnly.MinValue)
        {
            var minStartDate = lastVacationEndDate.AddMonths(MONTH_BETWEEN_VACATIONS_EMPLOYEE);
            availableStartDate = availableStartDate < minStartDate ? minStartDate : availableStartDate;
        }

        // если дней на балансе >= 14, то выбираем случайно, иначе берем нулевой индекс т.е. 7 дней отпуска.
        var daysInVacationIndex = vacationDaysBalance >= 14 ? Random.Shared.Next(_daysInVacation.Length) : 0;
        var daysInVacation = _daysInVacation[daysInVacationIndex];

        return new VacationDto
        {
            StartDate = availableStartDate,
            EndDate = availableStartDate.AddDays(daysInVacation),
            TotalDays = daysInVacation 
        };
    }

    /// <summary>
    /// Передвинуть дату с выходных дней.
    /// </summary>
    /// <param name="date"> Дата.</param>
    /// <returns> Дата буднего дня.</returns>
    private static DateOnly MoveDateFromWeekend(DateOnly date)
    {
        while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            date = date.AddDays(1);

        return date;
    }
}

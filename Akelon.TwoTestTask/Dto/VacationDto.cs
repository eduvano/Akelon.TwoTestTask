namespace Akelon.TwoTestTask.Dto;

/// <summary>
/// ДТО отпуска.
/// </summary>
public record VacationDto
{
    /// <summary>
    /// Дата начала отпуска.
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// Дата окончания отпуска.
    /// </summary>
    public DateOnly EndDate { get; init; }

    /// <summary>
    /// Всего дней.
    /// </summary>
    public int TotalDays { get; init; }
}

namespace Akelon.TwoTestTask.Dto;

/// <summary>
/// 
/// </summary>
public record EmployeeDto
{
    /// <summary>
    /// 
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string FirstName { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string LastName { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string? MiddleName { get; init; }
}

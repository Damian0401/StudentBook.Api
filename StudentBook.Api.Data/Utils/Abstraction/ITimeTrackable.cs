namespace StudentBook.Api.Data.Utils.Abstraction;

internal interface ITimeTrackable
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
namespace OpenDataPortal.Core.Wrappers.Results.Abstract
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}

namespace TemplateCleanArchi.Application.Interfaces
{
    /// <summary>
    /// Base interface for use cases with no input parameter
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    public interface IUseCase<TResult>
    {
        Task<TResult> Execute();
    }

    /// <summary>
    /// Base interface for use cases with one input parameter
    /// </summary>
    /// <typeparam name="TInput">The input parameter type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public interface IUseCase<in TInput, TResult>
    {
        Task<TResult> Execute(TInput input);
    }
}

using System;
using System.Threading.Tasks;

namespace MovieChest.ComponentModel;

public class Interaction<TInput, TOutput> : IInteraction<TInput, TOutput>
{
    private Func<TInput, TOutput>? handler;
    private Func<TInput, Task<TOutput>>? asyncHandler;

    public IDisposable Register(Func<TInput, TOutput> handler)
    {
        if (this.handler is not null || asyncHandler is not null)
        {
            throw new InvalidOperationException("Handler is already registered.");
        }
        this.handler = handler;
        return new ActionDisposable(() => this.handler = null);
    }

    public IDisposable Register(Func<TInput, Task<TOutput>> handler)
    {
        if (this.handler is not null || asyncHandler is not null)
        {
            throw new InvalidOperationException("Handler is already registered.");
        }
        asyncHandler = handler;
        return new ActionDisposable(() => asyncHandler = null);
    }

    public Task<TOutput> HandleAsync(TInput input)
    {
        if (handler is Func<TInput, TOutput> handle)
        {
            return Task.FromResult(handle(input));
        }
        else if (asyncHandler is Func<TInput, Task<TOutput>> handleAsync)
        {
            return handleAsync(input);
        }
        else
        {
            throw new InvalidOperationException("Handler is not registered.");
        }
    }
}
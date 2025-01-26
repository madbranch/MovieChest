using System;
using System.Threading.Tasks;

namespace MovieChest.ComponentModel;

public interface IInteraction<TInput, TOutput>
{
    IDisposable Register(Func<TInput, TOutput> handler);
    IDisposable Register(Func<TInput, Task<TOutput>> handler);
}
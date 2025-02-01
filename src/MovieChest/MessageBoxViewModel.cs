using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Immutable;

namespace MovieChest;

public record MessageBoxChoice(string Id, string Title);

public partial class MessageBoxViewModel(ImmutableArray<MessageBoxChoice> choices, string message) : ObservableObject
{
    public ImmutableArray<MessageBoxChoice> Choices { get; } = choices;
    public string Message { get; } = message;

    [ObservableProperty]
    private MessageBoxChoice? selectedChoice;
}
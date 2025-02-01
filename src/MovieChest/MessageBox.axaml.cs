using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace MovieChest;

public partial class MessageBox : Window
{
    public MessageBox()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button
            || button.DataContext is not MessageBoxChoice choice
            || DataContext is not MessageBoxViewModel viewModel)
        {
            return;
        }

        viewModel.SelectedChoice = choice;
        Close();
    }

    public static async Task<T?> ShowAsync<T>(Window owner, string message, params (string Id, string Title, T Value)[] choices)
    {
        if (choices.Length == 0)
        {
            throw new ArgumentException("At least one choice is required.", nameof(choices));
        }

        MessageBoxViewModel viewModel = new(choices.Select(x => new MessageBoxChoice(x.Id, x.Title)).ToImmutableArray(), message);
        MessageBox messageBox = new()
        {
            DataContext = viewModel,
        };

        if (await messageBox.ShowDialog<MessageBoxChoice?>(owner) is not MessageBoxChoice messageBoxChoice)
        {
            return default(T?);
        }

        foreach ((string id, string _, T value) in choices)
        {
            if (id == messageBoxChoice.Id)
            {
                return value;
            }
        }

        return default(T?);
    }
}
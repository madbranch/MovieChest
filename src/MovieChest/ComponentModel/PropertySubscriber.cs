using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MovieChest.ComponentModel;

public class PropertySubscriber<T> : IPropertySubscriber<T>
    where T : INotifyPropertyChanged
{
    private List<PropertyHandlerInfo> handlers = [];
    private Action? doAll;
    private readonly T subject;

    public PropertySubscriber(T subject)
    {
        this.subject = subject;
    }

    public class PropertyHandlerInfo
    {
        public required string PropertyName { get; init; }
        public Action? DoThis { get; set; }
    }

    public IPropertySubscriber<T> Any(string[] propertyNames, Action doThis)
    {
        foreach (string propertyName in propertyNames)
        {
            AddAction(propertyName, doThis);
        }

        doAll -= doThis;
        doAll += doThis;
        return this;
    }

    private void AddAction(string propertyName, Action doThis)
    {
        foreach (PropertyHandlerInfo handler in handlers)
        {
            if (handler.PropertyName == propertyName)
            {
                handler.DoThis -= doThis;
                handler.DoThis += doThis;
                return;
            }
        }

        handlers.Add(new PropertyHandlerInfo { PropertyName = propertyName, DoThis = doThis });
    }

    public IDisposable Subscribe()
    {
        subject.PropertyChanged += Subject_PropertyChanged;
        return new ActionDisposable(() => subject.PropertyChanged -= Subject_PropertyChanged );
    }

    private void Subject_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.PropertyName))
        {
            doAll?.Invoke();
            return;
        }

        foreach (PropertyHandlerInfo handler in handlers)
        {
            if (e.PropertyName == handler.PropertyName)
            {
                handler.DoThis!.Invoke();
            }
        }
    }
}
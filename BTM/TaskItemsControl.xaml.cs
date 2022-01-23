using System.Collections;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using BTM.Common;
using System.Collections.ObjectModel;
using BTM.Extensibility;
using System.Collections.Specialized;
using System.ComponentModel;
using BTM.Extensions;

namespace BTM
{
  /// <summary>
  /// Interaction logic for TaskItemsControl.xaml
  /// </summary>
  public partial class TaskItemsControl : UserControl
  {
    private DelegateCommand<ITask> _addCommand;
    private DelegateCommand<ITask> _deleteCommand;
    private DelegateCommand<ITask> _saveCommand;

    public ICommand AddCommand
    {
      get => _addCommand ?? (_addCommand = new DelegateCommand<ITask>(OnAdd, task => !TaskItemsSource.Any(item => item is DefaultTask)));
    }

    public ICommand SaveCommand
    {
      get => _saveCommand = (_saveCommand = new DelegateCommand<ITask>(OnSave, OnSaveCanExecute));
    }

    private bool OnSaveCanExecute(ITask task)
    {
      return ((DefaultTask)task).SelectedTask != null;
    }

    private void OnSave(ITask task)
    {
      if (task is DefaultTask defaultTask)
      {
        var index = TaskItemsSource.IndexOf(defaultTask);
        TaskItemsSource.Insert(index, defaultTask.SelectedTask.GetInstance());
        TaskItemsSource.Remove(defaultTask);

        if (index < TaskItemsSource.Count - 1) TaskItemsSource[index].Next = TaskItemsSource[index + 1];
        if (index > 0) TaskItemsSource[index].Previous = TaskItemsSource[index - 1];

        if (ItemsSource is ObservableCollection<ITask> itemsSource) itemsSource.Insert(index, TaskItemsSource[index]);
      }
    }

    private void OnAdd(ITask task)
    {
      var defaultTask = new DefaultTask();
      defaultTask.PropertyChanged += OnDefaultTaskPropertyChanged;
      if (task == null)
      {
        TaskItemsSource.Insert(0, defaultTask);
      }
      else
      {
        int index = TaskItemsSource.IndexOf(task);
        if (index == TaskItemsSource.Count - 1)
        {
          TaskItemsSource.Add(defaultTask);
        }
        else
        {
          TaskItemsSource.Insert(index + 1, defaultTask);
        }
      }
      TaskListItemsControl.ScrollIntoView(defaultTask);
    }

    public ICommand DeleteCommand
    {
      get => _deleteCommand ?? (_deleteCommand = new DelegateCommand<ITask>(OnDelete, OnDeleteCanExecute()));
    }

    private Func<ITask, bool> OnDeleteCanExecute()
    {
      return task => (task is DefaultTask && TaskItemsSource.Skip(1).Any()) || !TaskItemsSource.Any(item => item is DefaultTask);
    }


    private void OnDelete(ITask task)
    {
      DualSideDisconnect(task);

      if (task is DefaultTask) task.PropertyChanged -= OnDefaultTaskPropertyChanged;
      TaskItemsSource.Remove(task);
      if (ItemsSource is ObservableCollection<ITask> itemsSource) itemsSource.Remove(task);

      if (!TaskItemsSource.Any())
      {
        // taskList is empty, always keep one defaulttask
        DefaultTask defaultTask = new DefaultTask();
        defaultTask.PropertyChanged += OnDefaultTaskPropertyChanged;
        TaskItemsSource.Add(defaultTask);
      }
    }

    private void DualSideDisconnect(ITask task)
    {
      // Connect previous and next in both directions
      if (task.Next != null) task.Next.Previous = task.Previous;
      if (task.Previous != null) task.Previous.Next = task.Next;
    }

    private void OnDefaultTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SelectedTask" && _saveCommand != null) _saveCommand.RaiseCanExecuteChanged();
    }

    public IEnumerable ItemsSource
    {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TaskItemsControl), new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is TaskItemsControl taskItemsControl)
      {
        if (e.OldValue != null && e.OldValue is ObservableCollection<ITask> oldCOllection)
        {
          oldCOllection.CollectionChanged -= taskItemsControl.ItemsSourceCollectionChanged;
          taskItemsControl.TaskItemsSource.Clear();
        }
        if (e.NewValue != null && e.NewValue is ObservableCollection<ITask> newCollection)
        {
          if (!newCollection.Any())
          {
            var defaultTask = new DefaultTask();
            defaultTask.PropertyChanged += taskItemsControl.OnDefaultTaskPropertyChanged;
            taskItemsControl.TaskItemsSource.Add(defaultTask);
          }
          newCollection.CollectionChanged += taskItemsControl.ItemsSourceCollectionChanged;
        }
      }
    }

    private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (sender is IEnumerable<ITask> task)
      {
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
            foreach (var item in e.NewItems)
            {
              if(!(TaskItemsSource?.Contains(item) ?? false)) TaskItemsSource.Add((ITask)item);
            }
            break;
          case NotifyCollectionChangedAction.Remove:
            break;
          case NotifyCollectionChangedAction.Replace:
            break;
          case NotifyCollectionChangedAction.Move:
            break;
          case NotifyCollectionChangedAction.Reset:
            break;
        }
      }
    }

    public ObservableCollection<ITask> TaskItemsSource
    {
      get { return (ObservableCollection<ITask>)GetValue(TaskItemsSourceProperty); }
      set { SetValue(TaskItemsSourceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskItemsSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskItemsSourceProperty =
        DependencyProperty.Register("TaskItemsSource", typeof(ObservableCollection<ITask>), typeof(TaskItemsControl), new PropertyMetadata(default(ObservableCollection<ITask>)));

    private void EvaluateCanExecute()
    {
      if (_addCommand != null) _addCommand.RaiseCanExecuteChanged();
      if (_deleteCommand != null) _deleteCommand.RaiseCanExecuteChanged();
    }

    public TaskItemsControl()
    {
      InitializeComponent();
      TaskItemsSource = new ObservableCollection<ITask>();
      TaskItemsSource.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      EvaluateCanExecute();
      if (sender is ObservableCollection<ITask> collection && collection != null)
      {
        if (collection.Any(task => task.IsFirst))
        {
          foreach (var item in collection)
          {
            item.IsFirst = false;
          }
        }
        if (collection.Any()) collection.First().IsFirst = true;
      }
    }
  }
}

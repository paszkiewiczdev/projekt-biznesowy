using MVVMFirma.ViewModels;
using System;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVVMFirma.Views
{
    public class NowyTowarDialog : Window
    {
        public NowyTowarDialog()
        {
            Title = "Nowy towar";
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Content = BuildContent();

            DataContextChanged += OnDataContextChanged;
        }

        private UIElement BuildContent()
        {
            var dock = new DockPanel
            {
                Margin = new Thickness(12)
            };

            var form = new NowyTowarView();
            DockPanel.SetDock(form, Dock.Top);
            dock.Children.Add(form);

            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 12, 0, 0)
            };

            var saveButton = new Button
            {
                Content = "Zapisz",
                Width = 120,
                Margin = new Thickness(0, 0, 8, 0)
            };
            saveButton.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            var cancelButton = new Button
            {
                Content = "Anuluj",
                Width = 120
            };
            cancelButton.SetBinding(Button.CommandProperty, new Binding("CloseCommand"));

            buttonsPanel.Children.Add(saveButton);
            buttonsPanel.Children.Add(cancelButton);

            DockPanel.SetDock(buttonsPanel, Dock.Bottom);
            dock.Children.Add(buttonsPanel);

            return dock;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is WorkspaceViewModel oldViewModel)
                oldViewModel.RequestClose -= OnRequestClose;

            if (e.NewValue is WorkspaceViewModel newViewModel)
                newViewModel.RequestClose += OnRequestClose;
        }

        private void OnRequestClose(object sender, EventArgs e)
        {
            if (DataContext is NowyTowarViewModel viewModel && viewModel.WasSaved)
            {
                DialogResult = true;
                return;
            }

            Close();
        }
    }
}

using System.Windows.Controls;

namespace WebLab.RTR.Views;

/// <summary>
/// Interaction logic for TopicsView.xaml
/// </summary>
public partial class TopicsView : UserControl
{
    public TopicsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.TopicsViewModel vm)
        {
            await vm.LoadTopicsAsync();
        }
    }
}

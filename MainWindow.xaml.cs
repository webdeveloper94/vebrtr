using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WebLab.RTR;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Storyboard? _entranceStoryboard;
    private Storyboard? _ambientFloatingStoryboard;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnWindowLoaded;
        Closing += OnWindowClosing;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        // Start entrance animation once
        if (Resources["EntranceStoryboard"] is Storyboard entrance)
        {
            _entranceStoryboard = entrance;
            _entranceStoryboard.Begin(this, true);
        }

        // Start subtle ambient floating animation
        if (Resources["AmbientFloatingStoryboard"] is Storyboard ambient)
        {
            _ambientFloatingStoryboard = ambient;
            _ambientFloatingStoryboard.Begin(this, true);
        }
    }

    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        // Cleanly stop any running Storyboards to free render loop resources
        try
        {
            _entranceStoryboard?.Stop(this);
            _ambientFloatingStoryboard?.Stop(this);
        }
        catch
        {
            // Ignore during final teardown
        }
    }

    private void OnMinimizeWindowClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximizeWindowClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void OnCloseWindowClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Subtle, ultra-light parallax response to mouse movements for the 2.5D visual stage.
    /// Hardware-friendly using simple TranslateTransform offsets.
    /// </summary>
    private void OnMainWindowMouseMove(object sender, MouseEventArgs e)
    {
        if (StageTranslate == null || StageRotate == null || ActualWidth < 100 || ActualHeight < 100) return;

        Point mousePos = e.GetPosition(this);
        double relativeX = (mousePos.X / ActualWidth) - 0.5;
        double relativeY = (mousePos.Y / ActualHeight) - 0.5;

        // Micro-parallax offsets (smooth & balanced)
        StageTranslate.X = relativeX * -10;
        StageTranslate.Y = relativeY * -8;
        StageRotate.Angle = relativeX * 0.7;
    }
}
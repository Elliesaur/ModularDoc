using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.App.Views
{
  public class MainWindow
    : Window
  {
    private readonly NavigationManager m_navigator;
    private TransitioningContentControl? m_mainContent;
    //private readonly IPageTransition m_forward;
    private readonly IPageTransition m_back;

    public MainWindow()
    {
      m_navigator = TypeResolver.Resolve<NavigationManager>();
      m_navigator.NavigationChanged += OnNavigationChanged;
      InitializeComponent();

      //m_forward = new PageSlide(TimeSpan.FromMilliseconds(400));
      m_back = new CrossFade(TimeSpan.FromMilliseconds(120));
      ((CrossFade)m_back).FadeInEasing = new QuadraticEaseIn();
      m_navigator.NavigateTo(PageNames.HOME);

#if DEBUG
      this.AttachDevTools();
#endif
    }

    private void OnNavigationChanged(object? sender, NavigationManager.ViewData data)
    {
      var view = m_navigator.ResolveView(data.Name);

      if (data.Arguments.Count != 0)
        view.SetArguments(data.Arguments);

      if (data.NamedArguments.Count != 0)
        view.SetNamedArguments(data.NamedArguments);

      m_mainContent!.Content = view;

      m_mainContent!.PageTransition = m_back;
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
      m_mainContent = this.FindControl<TransitioningContentControl>("MainContent");
      m_mainContent!.PageTransition = m_back;
    }
  }
}
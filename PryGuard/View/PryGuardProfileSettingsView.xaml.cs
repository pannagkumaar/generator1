﻿using PryGuard.Core.ChromeApi.Settings;
using PryGuard.ViewModel;
using System.Windows.Controls;
using System;
using System.Diagnostics;

namespace PryGuard.View;
public partial class PryGuardProfileSettingsView : IBaseView
{
    public BaseViewModel ViewModel { get; set; }
    private bool _isPageLoaded = false;
    public PryGuardProfileSettingsView()
    {
        InitializeComponent();
        this.DataContext = new PryGuardProfileSettingsViewModel();
        this.Loaded += OnPageLoaded;
    }
    private void OnPageLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        _isPageLoaded = true;
    }
    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {

    }
    private void ComboBoxlang_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isPageLoaded)
        {
            // Page is not fully loaded, exit the method
            return;
        }
        var comboBox = sender as ComboBox;

        if (comboBox != null && comboBox.SelectedItem != null)
        {
            var viewModel = this.DataContext as PryGuardProfileSettingsViewModel;
            if (viewModel != null)
            {
                EChromeLanguage selectedLanguage;
                if (Enum.TryParse(comboBox.SelectedItem.ToString(), out selectedLanguage))
                {
                    viewModel.PryGuardProf.FakeProfile.ChromeLanguageInfo = EChromeLanguageHelper.GetFullInfo(selectedLanguage);
                }
            }
        }
    }

    private void rbMem_Checked(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
﻿using PryGuard.Model;
using System.Windows;
using PryGuard.Core.Web;
using System.Windows.Media;
using System.Threading.Tasks;
using PryGuard.Services.Commands;
using PryGuard.Core.ChromeApi.Model.Configs;
using System.Windows.Input;

namespace PryGuard.ViewModel;
public class PryGuardProfileSettingsViewModel : BaseViewModel
{
    #region Commands
    public RelayCommand CloseProfileSettingsCommand { get; private set; }
    public RelayCommand ChangeWindowStateCommand { get; private set; }
    public RelayCommand CheckProxyCommand { get; private set; }
    public RelayCommand SaveProfileCommand { get; private set; }
    public ICommand NewFingerprintCommand { get; }

    #endregion

    #region Properties
    private WindowState _windowState;
    public WindowState WindowState
    {
        get => _windowState;
        set => Set(ref _windowState, value);
    }

    private PryGuardProfilesViewModel _PryGuardProfilesVM;
    public PryGuardProfilesViewModel PryGuardProfilesVM
    {
        get => _PryGuardProfilesVM;
        set => Set(ref _PryGuardProfilesVM, value);
    }


    private string _saveProfileButtonContent = "Create";
    public string SaveProfileButtonContent
    {
        get => _saveProfileButtonContent;
        set => Set(ref _saveProfileButtonContent, value);
    }

    private Brush _tbProxyBrush = Brushes.White;
    public Brush TbProxyBrush
    {
        get => _tbProxyBrush;
        set => Set(ref _tbProxyBrush, value);
    }

    private PryGuardProfile _PryGuardProf;
    public PryGuardProfile PryGuardProf
    {
        get => _PryGuardProf;
        set => Set(ref _PryGuardProf, value);
    }
    #endregion

    #region Ctor
    public PryGuardProfileSettingsViewModel() { }
    public PryGuardProfileSettingsViewModel(PryGuardProfile PryGuardProfile)
    {
        CloseProfileSettingsCommand = new RelayCommand(CloseProfileSettings);
        SaveProfileCommand = new RelayCommand(SaveProfile);
        ChangeWindowStateCommand = new RelayCommand(CloseWindowState);
        CheckProxyCommand = new RelayCommand(CheckProxy);
        NewFingerprintCommand = new RelayCommand(GenerateNewFingerprint);

        PryGuardProf = PryGuardProfile;
    }
    #endregion

    #region Window Work & Actions
    private void GenerateNewFingerprint(object parameter)
    {
        if (PryGuardProf != null)
        {
            var newFakeProfile = FakeProfileFactory.Generate();

            PryGuardProf.FakeProfile = newFakeProfile;
            PryGuardProf.Status = "UPDATED"; // Or any status you want to set

            PryGuardProfilesVM.Setting.SaveSettings();
        }
        else
        {
            // Handle the case where PryGuardProf is null
            // Notify UI of the issue
        }
    }
    private void SaveProfile(object arg)
    {
        if (SaveProfileButtonContent == "Create")
        {
            ViewManager.Close(this);
            PryGuardProfilesVM.ProfileTabs.Add(new ProfileTab(PryGuardProfilesVM)
            {
                Name = PryGuardProf.Name,
                Id = PryGuardProf.Id,
                Status = PryGuardProf.Status,
                Tags = PryGuardProf.Tags,
                ProxyHostPort = PryGuardProf.Proxy.ProxyAddress == "" && PryGuardProf.Proxy.ProxyPort == 8080 ? "" : PryGuardProf.Proxy.ProxyAddress + ":" + PryGuardProf.Proxy.ProxyPort,
                ProxyLoginPass = PryGuardProf.Proxy.ProxyLogin == "" && PryGuardProf.Proxy.ProxyPassword == "" ? "" : PryGuardProf.Proxy.ProxyLogin + ":" + PryGuardProf.Proxy.ProxyPassword
            });
            PryGuardProfilesVM.Setting.SaveSettings();
        }
        else
        {
            ViewManager.Close(this);
            PryGuardProfilesVM.Setting.SaveSettings();
            PryGuardProfilesVM.ProfileTabs.Clear();
            PryGuardProfilesVM.LoadTabs();
        }
    }

    private async void CheckProxy()
    {
        if (PryGuardProf.Proxy.ProxyAddress == "") return;
        var result = await IpInfoClient.CheckClientProxy(PryGuardProf.Proxy);
        if (result == null)
        {
            TbProxyBrush = Brushes.Red;
            await Task.Delay(2000);
            TbProxyBrush = Brushes.White;
            return;
        }
        if (result.Ip == PryGuardProf.Proxy.ProxyAddress)
        {
            TbProxyBrush = Brushes.Green;
            await Task.Delay(2000);
            TbProxyBrush = Brushes.White;
        }
    }
    private void CloseProfileSettings(object arg)
    {
        ViewManager.Close(this);
    }
    private void CloseWindowState(object arg)
    {
        WindowState = WindowState.Minimized;
    }
    #endregion
}
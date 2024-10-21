﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class EnvironmentViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private string _name = string.Empty;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private ServerViewModel _server;

    public ServerViewModel Server
    {
        get
        {
            return _server;
        }
        set
        {
            _server = value;
            OnPropertyChanged();
        }
    }

    private VariableGroupViewModel _variableGroup;

    public VariableGroupViewModel VariableGroup
    {
        get
        {
            return _variableGroup;
        }
        set
        {
            _variableGroup = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
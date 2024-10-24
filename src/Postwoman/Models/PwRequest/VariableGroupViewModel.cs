﻿using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Postwoman.Models.PwRequest;

public class VariableGroupViewModel : INotifyPropertyChanged
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

    private VariableGroupViewModel _inherits;

    [JsonIgnore]
    public VariableGroupViewModel Inherits
    {
        get
        {
            return _inherits;
        }
        set
        {
            _inherits = value;
            InheritsGroupName = value?.Name;
            OnPropertyChanged();
        }
    }

    [JsonProperty("inherits")]
    public string InheritsGroupName { get; set; }

    public ObservableCollection<VariableViewModel> Variables { get; set; } = new();

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public VariableGroupViewModel Clone(string newName)
    {
        return new VariableGroupViewModel
        {
            Name = newName,
            Inherits = Inherits,
            Variables = new ObservableCollection<VariableViewModel>(Variables.Select(v => new VariableViewModel
            {
                Name = v.Name,
                Value = v.Value
            }))
        };
    }

}

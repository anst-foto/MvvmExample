using System;
using ReactiveUI;

namespace MvvmExample.Models;

public class Person : ReactiveObject
{
    private Guid _id;
    public Guid Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
    
    public string ShortId => Id.ToString()[0..8];
    
    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            this.RaiseAndSetIfChanged(ref _lastName, value);
            FullName = $"{_lastName} {_firstName}";
        }
    }
    
    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            this.RaiseAndSetIfChanged(ref _firstName, value);
            FullName = $"{_lastName} {_firstName}";
        }
    }
    
    private string _fullName;
    public string FullName
    {
        get => _fullName; 
        set => this.RaiseAndSetIfChanged(ref _fullName, value);
    }
    
    private DateTime _birthDate;
    public DateTime BirthDate
    {
        get => _birthDate;
        set => this.RaiseAndSetIfChanged(ref _birthDate, value);
    }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - _birthDate.Year;
            if (_birthDate.Date > today.AddYears(-age)) age--;
            
            return age;
        }
    }
}
using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MvvmExample.Models;

public class Person : ReactiveObject
{
    [Reactive] public Guid Id { get; set; }
    public string ShortId => Id.ToString()[0..8];
    
    [Reactive] public string LastName { get; set; }
    [Reactive] public string FirstName { get; set; }
    [Reactive] public string FullName { get; set; }
    [Reactive] public DateTime BirthDate { get; set; }
    [Reactive] public int Age { get; set; }

    public Person()
    {
        this.WhenAnyValue(p => p.BirthDate)
            .Subscribe(d => Age = CalculateAge());
        this.WhenAnyValue(p => p.FirstName)
            .Subscribe(firstName => FullName = $"{LastName} {firstName}");
        this.WhenAnyValue(p => p.LastName)
            .Subscribe(lastName => FullName = $"{lastName} {FirstName}");
    }

    private int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - BirthDate.Year;
        if (BirthDate.Date > today.AddYears(-age)) age--;
            
        return age;
    }
}
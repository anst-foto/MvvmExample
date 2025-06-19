using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text.Json;
using DynamicData;
using MvvmExample.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MvvmExample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    #region Properties
    
    [Reactive] public string? Id { get; set; }
    [Reactive] public string? LastName { get; set; }
    [Reactive] public string? FirstName { get; set; }
    [Reactive] public DateTimeOffset? BirthDate { get; set; }
    
    public ObservableCollection<Person> People { get; } = [];
    [Reactive] public Person? SelectedPerson { get; set; }
    
    #endregion

    #region Commands
    
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    
    #endregion

    public MainWindowViewModel()
    {
        PeopleFromJson();

        this.WhenAnyValue(vm => vm.SelectedPerson)
            .WhereNotNull()
            .Subscribe(person =>
            {
                Id = person.Id.ToString("D");
                LastName = person.LastName;
                FirstName = person.FirstName;
                BirthDate = new DateTimeOffset(person.BirthDate);
            });

        var canSave = this.WhenAnyValue(
            vm => vm.LastName,
            vm => vm.FirstName,
            (lastName, firstName) => !string.IsNullOrWhiteSpace(lastName) && 
                                     !string.IsNullOrWhiteSpace(firstName));
        var canDelete = this.WhenAnyValue(
            vm => vm.SelectedPerson,
            vm => vm.Id,
            (person, id) => person is not null && 
                            !string.IsNullOrWhiteSpace(id));
        var canClear = this.WhenAnyValue(
            vm => vm.LastName,
            vm => vm.FirstName,
            (lastName, firstName) => !string.IsNullOrWhiteSpace(lastName) ||
                                     !string.IsNullOrWhiteSpace(firstName));

        SaveCommand = ReactiveCommand.Create(Save, canSave);
        DeleteCommand = ReactiveCommand.Create(Delete, canDelete);
        ClearCommand = ReactiveCommand.Create(Clear, canClear);
    }

    #region Methods
    
    private void Load(IEnumerable<Person> people)
    {
        People.Clear();
        People.AddRange(people);
    }

    private void Save()
    {
        if (Id is null)
        {
            People.Add(new Person()
            {
                Id = Guid.NewGuid(),
                LastName = LastName!,
                FirstName = FirstName!,
                BirthDate = BirthDate!.Value.DateTime
            });
        }
        else
        {
            var id = Guid.Parse(Id);
            var person = People.SingleOrDefault(p => p.Id == id);

            if (person is null) return;

            person.LastName = LastName!;
            person.FirstName = FirstName!;
            person.BirthDate = BirthDate!.Value.DateTime;
        }

        PeopleToJson();
        
        Clear();
    }

    private void Delete()
    {
        var id = Guid.Parse(Id!);
        var person = People.SingleOrDefault(p => p.Id == id);

        if (person is null) return;

        People.Remove(person);

        PeopleToJson();
        
        Clear();
    }

    private void Clear()
    {
        SelectedPerson = null;
        Id = null;
        LastName = null;
        FirstName = null;
        BirthDate = null;
    }

    private void PeopleFromJson(string path = "people.json")
    {
        if (!File.Exists(path)) return;
        
        var json = File.ReadAllText(path);
        var people = JsonSerializer.Deserialize<List<Person>>(json);
        Load(people!);
    }

    private void PeopleToJson(string path = "people.json")
    {
        var json = JsonSerializer.Serialize(People.ToList());
        File.WriteAllText(path, json);
    }
    
    #endregion
}
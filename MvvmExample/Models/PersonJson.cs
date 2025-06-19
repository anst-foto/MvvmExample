using System;

namespace MvvmExample.Models;

public class PersonJson
{
    public Guid Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDate { get; set; }

    public static explicit operator Person(PersonJson json) => new Person()
    {
        Id = json.Id,
        LastName = json.LastName,
        FirstName = json.FirstName,
        BirthDate = json.BirthDate
    };

    public static explicit operator PersonJson(Person person) => new PersonJson()
    {
        Id = person.Id,
        LastName = person.LastName,
        FirstName = person.FirstName,
        BirthDate = person.BirthDate
    };
}
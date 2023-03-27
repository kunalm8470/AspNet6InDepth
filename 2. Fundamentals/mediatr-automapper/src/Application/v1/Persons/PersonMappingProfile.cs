using Application.v1.Persons.Commands.AddPerson;
using Application.v1.Persons.Commands.UpdatePerson;
using AutoMapper;
using Domain.Entities;

namespace Application.v1.Persons;

public class PersonMappingProfile : Profile
{
	public PersonMappingProfile()
	{
		CreateMap<AddPersonCommand, Person>()
		.ForMember(x => x.Id, (options) => options.MapFrom(y => Guid.NewGuid()));

		CreateMap<UpdatePersonCommand, Person>();
    }
}

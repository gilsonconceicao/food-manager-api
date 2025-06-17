using AutoMapper;
using Application.Contacts.Commands;
using Domain.Models;

namespace Application.Mappings;

public class ContactMappers : Profile
{
    public ContactMappers()
    {
        CreateMap<ContactCreateCommand, Contact>();
    }
}

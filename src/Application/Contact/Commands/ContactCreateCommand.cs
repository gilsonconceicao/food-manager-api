using Api.Workflows.JobSchedulerService;
using Application.Workflows.Workflows;
using AutoMapper;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
#nullable disable
namespace Application.Contacts.Commands;

public class ContactCreateCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public string PhoneNumber { get; set; }
}

public class ContactCreateCommandHandler : IRequestHandler<ContactCreateCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly DataBaseContext _context;
    private readonly IJobSchedulerService _jobSchedulerService;


    public ContactCreateCommandHandler(IMapper mapper, DataBaseContext context, IJobSchedulerService jobSchedulerService)
    {
        _mapper = mapper;
        _context = context;
        _jobSchedulerService = jobSchedulerService;
    }

    public async Task<bool> Handle(ContactCreateCommand request, CancellationToken cancellationToken)
    {
        Contact contact = _mapper.Map<Contact>(request);
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        _jobSchedulerService.Enqueue<SendEmailContactWorkflow>((job) => job.SendEmailAsync(contact.Id));

        return true;
    }
}

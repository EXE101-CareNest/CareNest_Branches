
using CareNest_Branches.Application.Interfaces.CQRS.Commands;

namespace CareNest_Branches.Application.Features.Commands.Delete
{
    public class DeleteCommand : ICommand
    {
        public required string Id { get; set; }
    }
}

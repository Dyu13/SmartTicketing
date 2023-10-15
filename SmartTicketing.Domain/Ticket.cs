using SmartTicketing.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartTicketing.Domain;

public class Ticket
{
    public int TicketId { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public ETicketStatus Status { get; set; }

    [Required]
    public int CreatedByUserId { get; set; }

    public int AssignToUserId { get; set; }
}

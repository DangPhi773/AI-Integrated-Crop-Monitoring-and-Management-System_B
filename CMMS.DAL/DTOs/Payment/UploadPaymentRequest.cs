using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CMMS.DAL.DTOs.Payment;

public class UploadPaymentRequest
{
    [Required]
    public Guid SpecialistId { get; set; }

    [Required]
    public DateTime Month { get; set; }

    [Required]
    public IFormFile BillFile { get; set; } = null!;
}

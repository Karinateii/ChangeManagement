using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.Models.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters.")]
        [Display(Name = "Request Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority is required.")]
        [RegularExpression("^(Low|Medium|High|Critical)$", ErrorMessage = "Priority must be Low, Medium, High, or Critical.")]
        [Display(Name = "Priority Level")]
        public string Priority { get; set; } = "Medium";

        [StringLength(50)]
        [Display(Name = "Status")]
        public string? Status { get; set; } = "Pending";

        [Required]
        [Display(Name = "Submission Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Admin Approval Date")]
        [DataType(DataType.DateTime)]
        public DateTime? AdminApprovalDate { get; set; }

        [StringLength(1000, ErrorMessage = "Admin reason cannot exceed 1000 characters.")]
        [Display(Name = "Admin Reason")]
        public string? AdminReason { get; set; }

        [Required(ErrorMessage = "Submitter information is required.")]
        [StringLength(100)]
        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; } = string.Empty;
    }
}

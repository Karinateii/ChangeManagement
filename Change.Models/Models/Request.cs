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

        //[Display(Name = "Name")]
        //public String RequestedBy { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        
        public string? Status { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime Date { get; set; }

        // Admin approval date (hidden from regular users)
        [Display(Name = "Admin Approval Date")]
        public DateTime? AdminApprovalDate { get; set; }

        // Admin reason (required for admin)
        //[Required(ErrorMessage = "Admin Reason is required.")]
        public string? AdminReason { get; set; }

         // New property to store the user name
        public string SubmittedBy { get; set; }

    }
}

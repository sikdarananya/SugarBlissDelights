//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SweetShop_MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TBLUserInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLUserInfo()
        {
            this.orderts = new HashSet<ordert>();
        }
    
        public int IdUs { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage ="This is required feild is required")]
        

        public string UsernameUs { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage ="This is required field is required")]
       // [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="This required field is required")]
        //[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password's don't match, try again!")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ordert> orderts { get; set; }
    }
}

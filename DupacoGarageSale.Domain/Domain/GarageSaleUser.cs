using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupacoGarageSale.Data.Domain
{
    public class GarageSaleUser
    {
        public int UserId { get; set; }

        [Required(ErrorMessage="First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]?([0-9]{3})[-]?([0-9]{4})$", ErrorMessage = "Enter your phone number in the following format: 555-323-1234")]
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ProfilePicLink { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Retype your password to confirm it")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Your password entries must match")]
        public string ConfirmPassword { get; set; }

        public byte[] BytePassword { get; set; }
        public UserAddress Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyUser { get; set; }
        public bool Active { get; set; }
        public int UserTypeId { get; set; }
    }
}

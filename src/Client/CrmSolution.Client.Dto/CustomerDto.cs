using Bit.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms.StateSquid;

namespace CrmSolution.Shared.Dto
{
    public partial class CustomerDto : Bindable
    {
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public State CurrentState { get; set; }
    }
}

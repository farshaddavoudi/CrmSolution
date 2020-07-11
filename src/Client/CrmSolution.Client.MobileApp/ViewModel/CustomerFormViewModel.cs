using Acr.UserDialogs;
using Bit.Core.Exceptions;
using Bit.ViewModel;
using CrmSolution.Client.MobileApp.Core;
using CrmSolution.Shared.Dto;
using Prism.Navigation;
using Simple.OData.Client;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomerFormViewModel : BitViewModelBase
    {
        public IODataClient ODataClient { get; set; }
        public IUserDialogs UserDialogs { get; set; }

        public ObservableCollection<CustomerDto> Customers { get; set; }
        public OperationKind OperationKind { get; set; } = OperationKind.Edit;
        public CustomerDto Customer { get; set; }

        public BitDelegateCommand AddOrUpdateCustomerCommand { get; set; }

        public CustomerFormViewModel()
        {
            AddOrUpdateCustomerCommand = new BitDelegateCommand(AddOrUpdateCustomer);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);

            Customer = parameters.GetValue<CustomerDto>("customer");

            if (Customer == null)
            {
                Customer = new CustomerDto();
                OperationKind = OperationKind.Add;
            }

            ObservableCollection<CustomerDto> customers = parameters.GetValue<ObservableCollection<CustomerDto>>("customers");

            Customers = customers;
        }

        private async Task AddOrUpdateCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.FirstName) || string.IsNullOrWhiteSpace(Customer.LastName))
            {
                throw new DomainLogicException("Please fill both first name and last name to submit");
            }

            if (OperationKind == OperationKind.Add)
            {
                // Add in Database
                CustomerDto newCustomer = await ODataClient.Customers()
                    .Set(Customer).InsertEntryAsync();

                // Add in Customers list
                Customers.Add(newCustomer);
            }
            else
            {
                // Edit in Database
                await ODataClient.Customers()
                    .Key(Customer.Id)
                    .Set(Customer)
                    .UpdateEntryAsync();
            }

            await NavigationService.GoBackAsync();
        }
    }
}
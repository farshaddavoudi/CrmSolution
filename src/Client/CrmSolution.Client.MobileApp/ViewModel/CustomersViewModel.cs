using Acr.UserDialogs;
using Bit.ViewModel;
using CrmSolution.Shared.Dto;
using Prism.Navigation;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.StateSquid;

namespace CrmSolution.Client.MobileApp.ViewModel
{
    public class CustomersViewModel : BitViewModelBase
    {
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IODataClient ODataClient { get; set; }
        public IUserDialogs UserDialogs { get; set; }

        public ObservableCollection<CustomerDto> AllCustomers { get; set; }
        public ObservableCollection<CustomerDto> CustomersView { get; set; }
        public string SearchText { get; set; }
        public State CurrentState { get; set; }
        public CustomerDto SelectedCustomer { get; set; }

        public BitDelegateCommand GoToAddCustomerCommand { get; set; }
        public BitDelegateCommand<CustomerDto> GoToEditCustomerCommand { get; set; }
        public BitDelegateCommand<CustomerDto> DeleteCustomerCommand { get; set; }

        public CustomersViewModel()
        {
            GoToAddCustomerCommand = new BitDelegateCommand(AddCustomer);
            GoToEditCustomerCommand = new BitDelegateCommand<CustomerDto>(EditCustomer);
            DeleteCustomerCommand = new BitDelegateCommand<CustomerDto>(DeleteCustomer);
        }

        public async void OnSearchTextChanged()
        {
            try
            {
                _cancellationTokenSource?.Cancel();

                _cancellationTokenSource = new CancellationTokenSource();

                if (string.IsNullOrWhiteSpace(SearchText) || SearchText.Length < 3)
                {
                    CustomersView = AllCustomers;
                }
                else
                {
                    CustomersView = new ObservableCollection<CustomerDto>(await ODataClient.Customers()
                        .Where(c => c.FirstName.Contains(SearchText) || c.LastName.Contains(SearchText))
                        .FindEntriesAsync(_cancellationTokenSource.Token));
                }
            }
            catch (Exception exception)
            {
                ExceptionHandler.OnExceptionReceived(exception);
            }
        }

        async Task AddCustomer()
        {
            await NavigationService.NavigateAsync("CustomerForm", ("customers", CustomersView));
        }

        async Task DeleteCustomer(CustomerDto customerDto)
        {
            try
            {
                customerDto.CurrentState = State.Saving;

                await ODataClient.Customers()
                    .Key(customerDto.Id)
                    .DeleteEntryAsync();

                CustomersView.Remove(customerDto);

                await UserDialogs.AlertAsync($"{customerDto.FullName} Successfully Deleted!");
            }
            finally
            {
                customerDto.CurrentState = State.None;
            }
        }

        async Task EditCustomer(CustomerDto selectedCustomerDto)
        {
            await NavigationService.NavigateAsync("CustomerForm",
                ("customer", selectedCustomerDto));
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            try
            {
                await base.OnNavigatedToAsync(parameters);

                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    CurrentState = State.Loading;
                    List<CustomerDto> allCustomers = (await ODataClient.Customers().FindEntriesAsync()).ToList();
                    CustomersView = AllCustomers = new ObservableCollection<CustomerDto>(allCustomers);
                }
            }
            finally
            {
                CurrentState = State.None;
            }
        }
    }
}

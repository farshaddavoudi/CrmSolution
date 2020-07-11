using Bit.View;
using CrmSolution.Client.MobileApp.Core;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrmSolution.Client.MobileApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerFormView : ContentPage
    {
        public CustomerFormView()
        {
            InitializeComponent();
        }
    }

    public class OperationKindToTitleConverter : ValueConverter<OperationKind, string>
    {
        protected override string Convert(OperationKind value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == OperationKind.Add ? "Add New Customer" : "Update Customer";
        }
    }
}
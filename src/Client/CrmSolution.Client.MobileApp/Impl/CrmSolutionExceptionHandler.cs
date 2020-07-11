using Acr.UserDialogs;
using Bit.Core.Exceptions.Contracts;
using Bit.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrmSolution.Client.MobileApp.Impl
{
    public class CrmSolutionExceptionHandler : BitExceptionHandler
    {
        public IUserDialogs UserDialogs { get; set; }
        public override void OnExceptionReceived(Exception exp, IDictionary<string, string> properties = null)
        {
            if (exp is IOException && (exp.Message == "Canceled" || exp.Message == "Socket closed" ))
                return;

            UserDialogs.Alert(exp is IKnownException ? exp.Message : "Something bad happened!!");
#if DEBUG

            System.Diagnostics.Debugger.Break();

#endif

            base.OnExceptionReceived(exp, properties);
        }
    }
}

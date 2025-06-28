using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _saleVM;
        private readonly IEventAggregator _events;

        public ShellViewModel(SalesViewModel saleVM, IEventAggregator events)
        {
            _saleVM = saleVM;
            
            _events = events;

            _events.SubscribeOnPublishedThread(this);
            
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {

            await ActivateItemAsync(_saleVM);

        }

    
    }
}

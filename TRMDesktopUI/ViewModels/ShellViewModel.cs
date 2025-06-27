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

        private SimpleContainer _container { get; }

        public ShellViewModel(SalesViewModel saleVM, IEventAggregator events,
            SimpleContainer container)
        {
            _saleVM = saleVM;
            
            _events = events;
            _container = container;

            _events.SubscribeOnPublishedThread(this);
            
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {

            await ActivateItemAsync(_saleVM);

        }

    
    }
}

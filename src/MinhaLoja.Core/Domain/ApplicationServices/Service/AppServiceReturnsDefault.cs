using Flunt.Notifications;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System.Collections.Generic;
using System.Linq;

namespace MinhaLoja.Core.Domain.ApplicationServices.Service
{
    public abstract partial class AppService<TDataResponse>
    {
        protected IResponseAppService<bool> ReturnSuccess()
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseAppService<bool>(true, null);
        }

        protected IResponseAppService<TDataResponse> ReturnData(
            TDataResponse data, 
            PagedDataResponseService pagination = null)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseAppService<TDataResponse>(data, pagination);
        }

        protected IResponseAppService<TDataResponse> ReturnNotification(string key, string message)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseAppService<TDataResponse>(new Notification(key, message));
        }

        protected IResponseAppService<TDataResponse> ReturnNotifications(IReadOnlyCollection<Notification> notifications)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseAppService<TDataResponse>(notifications.ToList());
        }
    }
}

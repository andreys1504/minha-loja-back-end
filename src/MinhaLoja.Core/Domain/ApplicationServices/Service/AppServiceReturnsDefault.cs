using Flunt.Notifications;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System.Collections.Generic;
using System.Linq;

namespace MinhaLoja.Core.Domain.ApplicationServices.Service
{
    public abstract partial class AppService<TDataResponse>
    {
        protected IResponseService<bool> ReturnSuccess()
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseService<bool>(true, null);
        }

        protected IResponseService<TDataResponse> ReturnData(
            TDataResponse data, 
            PagedDataResponseService pagination = null)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseService<TDataResponse>(data, pagination);
        }

        protected IResponseService<TDataResponse> ReturnNotification(string key, string message)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseService<TDataResponse>(new Notification(key, message));
        }

        protected IResponseService<TDataResponse> ReturnNotifications(IReadOnlyCollection<Notification> notifications)
        {
            DisposeTransactionAsync().GetAwaiter();
            return new ResponseService<TDataResponse>(notifications.ToList());
        }
    }
}

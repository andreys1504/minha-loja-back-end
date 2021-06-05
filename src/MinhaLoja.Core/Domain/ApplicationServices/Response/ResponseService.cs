using Flunt.Notifications;
using System.Collections.Generic;

namespace MinhaLoja.Core.Domain.ApplicationServices.Response
{
    public class ResponseService<TDataResponse> : IResponseService<TDataResponse>
    {
        protected ResponseService()
        {
        }

        public ResponseService(
            TDataResponse data, 
            PagedDataResponseService pagination = null)
        {
            Success = true;
            Data = data;
            Notifications = null;
            Pagination = pagination;
        }

        public ResponseService(IList<Notification> notifications)
        {
            Success = false;
            Notifications = notifications;
        }

        public ResponseService(Notification notifications)
        {
            Success = false;
            Notifications = new List<Notification> { notifications };
        }

        public bool Success { get; set; }
        public TDataResponse Data { get; set; }
        public IList<Notification> Notifications { get; set; }
        public PagedDataResponseService Pagination { get; set; }
    }
}

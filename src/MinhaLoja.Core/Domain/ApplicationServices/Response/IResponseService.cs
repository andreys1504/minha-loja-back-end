using Flunt.Notifications;
using System.Collections.Generic;

namespace MinhaLoja.Core.Domain.ApplicationServices.Response
{
    public interface IResponseService<TDataResponse> : IResponse
    {
        TDataResponse Data { get; set; }
        IList<Notification> Notifications { get; set; }
        bool Success { get; set; }
        public PagedDataResponseService Pagination { get; set; }
    }

    public interface IResponse
    {
        bool Success { get; set; }
    }
}

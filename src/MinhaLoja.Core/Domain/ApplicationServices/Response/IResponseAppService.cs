using Flunt.Notifications;
using System.Collections.Generic;

namespace MinhaLoja.Core.Domain.ApplicationServices.Response
{
    public interface IResponseAppService<TDataResponse> : IResponse
    {
        TDataResponse Data { get; set; }
        IList<Notification> Notifications { get; set; }
        public PagedDataResponseService Pagination { get; set; }
    }
}

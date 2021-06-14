using System.Collections.Generic;

namespace MinhaLoja.Core.Authorizations
{
    public class AuthorizationsApplications
    {
        public class AdminLoja
        {
            public static List<string> GetAll
                => new()
                {
                    UsuarioMaster,
                    UsuarioVendedor
                };

            public const string UsuarioMaster = "UsuarioMaster";
            public const string UsuarioVendedor = "UsuarioVendedor";
        }
    }
}

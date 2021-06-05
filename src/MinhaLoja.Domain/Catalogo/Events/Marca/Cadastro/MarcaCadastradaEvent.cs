using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.Catalogo.Events.Marca.Cadastro
{
    public class MarcaCadastradaEvent : DomainEvent, INotification
    {
        public MarcaCadastradaEvent(
            int idMarca, 
            string nomeMarca)
        {
            IdMarca = idMarca;
            NomeMarca = nomeMarca;
        }

        public int IdMarca { get; private set; }
        public string NomeMarca { get; private set; }
    }
}

namespace MinhaLoja.Domain
{
    //Organização: Bounded Context (Catalogo) > Entidade (==Marca) > Funcionalidade ( Marca: Cadastro)
    public class MessagesDomain
    {
        public class Catalogo
        {
            //==TipoProduto

            // TipoProduto: Cadastro
            public const string TipoProduto_Cadastro_NomeTipoProdutoIsNotNullOrWhiteSpace = "Informe o Nome";
            public const string TipoProduto_Cadastro_NomeTipoProdutoIsGreaterOrEqualsThan = "Nome inválido";
            public const string TipoProduto_Cadastro_NomeTipoProdutoIsLowerOrEqualsThan = "Nome inválido";

            public const string TipoProduto_Cadastro_IdTipoProdutoSuperiorIsGreaterOrEqualsThan = "Tipo Superior inválido";
            public const string TipoProduto_Cadastro_CaracteristicasTipoProdutoIsNotNull = "Informe Caracteristica(s) do Tipo Produto";
            public const string TipoProduto_Cadastro_CaracteristicasTipoProdutoCount = "Informe Caracteristica(s) do Tipo Produto";

            public const string TipoProduto_Cadastro_NomeCaracteristicaIsNotNullOrWhiteSpace = "Informe o Nome da Caracteristica";
            public const string TipoProduto_Cadastro_NomeCaracteristicaIsLowerOrEqualsThan = "Informe uma Caracteristica entre 3 e 30 caracteres";
            public const string TipoProduto_Cadastro_NomeCaracteristicaIsGreaterOrEqualsThan = "Informe uma Caracteristica entre 3 e 30 caracteres";
            public const string TipoProduto_Cadastro_ObservacaoCaracteristicaIsLowerOrEqualsThan = "Informe uma Caracteristica de até 35 caracteristicas";


            public const string TipoProduto_Cadastro_TipoProdutoExistente = "Tipo Produto existente no sistema";
            public const string TipoProduto_Cadastro_TipoProdutoSuperiorNulo = "Tipo Produto Superior/Pai inexistente";
            public const string TipoProduto_Cadastro_CaracteristicasRepetidasEnviada = "Caracteristicas com nomes iguais enviadas";
            public const string TipoProduto_Cadastro_CaracteristicasExistentesNoSistema = "Caracteristicas já existentes neste Grupo de Tipo de Produto";


            //==Marca

            // Marca: Cadastro
            public const string Marca_Cadastro_NomeMarcaIsNotNullOrWhiteSpace = "Informe o Nome da Marca";
            public const string Marca_Cadastro_NomeMarcaIsGreaterOrEqualsThan = "Nome inválido";
            public const string Marca_Cadastro_NomeMarcaIsLowerOrEqualsThan = "Informe um nome com até 40 caracteres";


            public const string Marca_Cadastro_MarcaJaCadastradaSistema = "Marca existente no sistema";


            //==Produto

            // Produto: Cadastro
            public const string Produto_Cadastro_NomeProdutoIsNotNullOrWhiteSpace = "Informe o Nome do Produto";
            public const string Produto_Cadastro_NomeProdutoIsGreaterOrEqualsThan = "Nome do Produto - Min: 15, Max: 150 caracteres";
            public const string Produto_Cadastro_NomeProdutoIsLowerOrEqualsThan = "Nome do Produto - Min: 15, Max: 150 caracteres";

            public const string Produto_Cadastro_ValorIsGreaterOrEqualsThan = "Informe um Valor válido";
            public const string Produto_Cadastro_IdMarcaIsGreaterOrEqualsThan = "Informe uma Marca válida";

            public const string Produto_Cadastro_DescricaoProdutoIsNotNullOrWhiteSpace = "Informe a Descrição do Produto";
            public const string Produto_Cadastro_DescricaoProdutoIsGreaterOrEqualsThan = "Descrição do Produto - Minimo: 15 caracteres";
            public const string Produto_Cadastro_DescricaoProdutoIsLowerOrEqualsThan = "Descrição muito grande";

            public const string Produto_Cadastro_IdExternoIsLowerOrEqualsThan = "Id Externo inválido";
            
            public const string Produto_Cadastro_IdTipoProdutoIsGreaterOrEqualsThan = "Informe um Tipo de produto válido";
            
            public const string Produto_Cadastro_CaracteristicaProdutoIsNotNull = "Informe as Caracteristicas do Produto";
            public const string Produto_Cadastro_CaracteristicaProdutoIsGreaterOrEqualsThan = "Informe as Caracteristicas do Produto";

            public const string Produto_Cadastro_IdVendedorIsGreaterOrEqualsThan = "Informe um Vendedor válido";


            public const string Produto_Cadastro_IdMarcaInvalida = "Marca inválida";
            public const string Produto_Cadastro_IdTipoProdutoInvalido = "Tipo de Produto inválido";
            public const string Produto_Cadastro_ProdutoJaCadastradaSistema = "Produto existente no sistema";
            public const string Produto_Cadastro_CaracteristicaProdutoInvalida = "Características inválidas informadas";
            public const string Produto_Cadastro_IdVendedorInvalido = "Vendedor inválido";
        }

        public class ContaUsuarioAdministrador
        {
            //==UsuarioAdministrador
            
            // UsuarioAdministrador: Autenticacao
            public const string UsuarioAdministrador_Autenticacao_UsernameIsNotNullOrWhiteSpace = "Informe o Login";
            public const string UsuarioAdministrador_Autenticacao_UsernameSenhaMensagemGenerica = "Login e/ou Senha inválido(s)";
            public const string UsuarioAdministrador_Autenticacao_SenhaIsNotNullOrWhiteSpace = "Informe a Senha";

            // UsuarioAdministrador: CadastroUsuarioMaster
            public const string UsuarioMaster_Cadastro_NomeIsNotNullOrWhiteSpace = "Informe o Nome";
            public const string UsuarioMaster_Cadastro_NomeIsGreaterOrEqualsThan = "Nome inválido";
            public const string UsuarioMaster_Cadastro_NomeIsLowerOrEqualsThan = "Nome inválido";

            public const string UsuarioMaster_Cadastro_UsernameIsNotNullOrWhiteSpace = "Informe o Login";
            public const string UsuarioMaster_Cadastro_UsernameIsGreaterOrEqualsThan = "Login inválido";
            public const string UsuarioMaster_Cadastro_UsernameIsLowerOrEqualsThan = "Login inválido";

            public const string UsuarioMaster_Cadastro_SenhaIsNotNullOrWhiteSpace = "Informe a Senha";
            public const string UsuarioMaster_Cadastro_SenhaIsGreaterOrEqualsThan = "Informe uma senha entre 8 e 20 caracteres";
            public const string UsuarioMaster_Cadastro_SenhaIsLowerOrEqualsThan = "Informe uma senha entre 8 e 20 caracteres";

            public const string UsuarioMaster_Cadastro_UsuarioJaCadastradaSistema = "Usuário já cadastrado no sistema";

            // UsuarioAdministrador: CadastroUsuarioVendedor
            public const string Vendedor_Cadastro_NomeIsNotNullOrWhiteSpace = "Informe o Nome";
            public const string Vendedor_Cadastro_NomeIsGreaterOrEqualsThan = "Nome inválido";
            public const string Vendedor_Cadastro_NomeIsLowerOrEqualsThan = "Nome inválido";

            public const string Vendedor_Cadastro_SenhaIsNotNullOrWhiteSpace = "Informe a Senha";
            public const string Vendedor_Cadastro_SenhaIsGreaterOrEqualsThan = "Informe uma senha entre 8 e 20 caracteres";
            public const string Vendedor_Cadastro_SenhaIsLowerOrEqualsThan = "Informe uma senha entre 8 e 20 caracteres";

            public const string Vendedor_Cadastro_EmailIsNotNullOrWhiteSpace = "Informe o E-mail";
            public const string Vendedor_Cadastro_EmailIsEmail = "E-mail inválido";
            public const string Vendedor_Cadastro_EmailIsLowerOrEqualsThan = "E-mail inválido";

            public const string Vendedor_Cadastro_CnpjIsNotNullOrWhiteSpace = "Informe o CNPJ";
            public const string Vendedor_Cadastro_CnpjIsCnpj = "CNPJ inválido";

            public const string Vendedor_Cadastro_NotificacaoUsuarioExistente = "Usuário já cadastrado no sistema";
            public const string Vendedor_Cadastro_Sucesso = "Cadastro realizado. Por favor, valide seu e-mail";


            //==Vendedor
            public const string Vendedor_AssuntoMensagemValidacaoEmail = "MinhaLoja - Validação E-mail";
            public const string Vendedor_AssuntoMensagemAprovacaoCadastro = "MinhaLoja - Status Aprovação Cadastro";
            public const string Vendedor_ValidacaoEmail_Pendente = "Valide o seu E-mail! Uma nova mensagem foi enviada";
            public const string Vendedor_ValidacaoEmail_DataValidacaoVencida = "Código Vencido! Uma nova mensagem foi enviada ao seu e-mail";
            public const string Vendedor_AprovacaoCadastro_CadastroNaoAprovado = "Seu cadastro não foi aprovado";
            public const string Vendedor_AprovacaoCadastro_CadastroAprovacaoPendente = "Seu cadastro ainda não foi aprovado, por favor, aguarde o periodo informado anteriormente";

            // Vendedor: Aprovar
            public const string Vendedor_Aprovar_IdVendedorIsGreaterOrEqualsThan = "Vendedor inválido";

            public const string Vendedor_Aprovar_NotificacaoUsuarioInexistente = "Vendedor inválido";
            public const string Vendedor_Aprovar_NotificacaoErroAprovacao = "Erro na aprovação do cadastro";
            public const string Vendedor_Aprovar_MensagemEmail01 = "Cadastro Aprovado!";
            public const string Vendedor_Aprovar_MensagemEmail02 = "Autentique-se na plataforma";

            // Vendedor: Rejeitar
            public const string Vendedor_Rejeitar_IdVendedorIsGreaterOrEqualsThan = "Vendedor inválido";
            
            public const string Vendedor_Rejeitar_NotificacaoUsuarioInexistente = "Vendedor inválido";
            public const string Vendedor_Rejeitar_NotificacaoErroRejeicao = "Erro na rejeição do cadastro";
            public const string Vendedor_Rejeitar_MensagemEmail01 = "Cadastro Rejeitado!";
            public const string Vendedor_Rejeitar_MensagemEmail02 = "Infelizmente seu cadastro foi rejeitado";
        
            // Vendedor: ValidarEmail
            public const string Vendedor_ValidarEmail_CodigoIsNotNullOrWhiteSpace = "Código inválido";
            
            public const string Vendedor_ValidarEmail_Sucesso01 = "E-mail Validado";
            public const string Vendedor_ValidarEmail_Sucesso02 = "Seus dados passarão por uma validação, em até 3 dias úteis você receberá um e-mail informando sucesso ou rejeição";
        }
    }
}

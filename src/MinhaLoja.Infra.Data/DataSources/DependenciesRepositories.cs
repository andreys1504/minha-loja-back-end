using MinhaLoja.Infra.Data.DataSources.DatabaseMain;
using MinhaLoja.Infra.Data.DataSources.DatabaseSecondary;

namespace MinhaLoja.Infra.Data.DataSources
{
    public class DependenciesRepositories
    {
        public DependenciesRepositories(
            MinhaLojaContext minhaLojaContext,
            MinhaLojaContextSecondaryDatabase minhaLojaContextSecondaryDatabase)
        {
            MinhaLojaContext = minhaLojaContext;
            MinhaLojaContextSecondaryDatabase = minhaLojaContextSecondaryDatabase;
        }

        public MinhaLojaContext MinhaLojaContext { get; private set; }
        public MinhaLojaContextSecondaryDatabase MinhaLojaContextSecondaryDatabase { get; private set; }
    }
}

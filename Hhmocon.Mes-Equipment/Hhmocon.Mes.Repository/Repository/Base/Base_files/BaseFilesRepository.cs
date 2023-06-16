using Hhmocon.Mes.Util.AutofacManager;

namespace Hhmocon.Mes.Repository
{
    public class BaseFilesRepository : IBaseFilesRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        public BaseFilesRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }

    }
}

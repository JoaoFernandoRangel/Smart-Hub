using System.Collections.Generic;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _repository;

        public ProcedureService(IProcedureRepository repository)
        {
            _repository = repository;
        }

        public Procedure GetProcedure(int id)
        {
            return _repository.GetProcedure(id);
        }

        public IEnumerable<Procedure> ListProcedures()
        {
            return _repository.ListProcedures();
        }
    }
}

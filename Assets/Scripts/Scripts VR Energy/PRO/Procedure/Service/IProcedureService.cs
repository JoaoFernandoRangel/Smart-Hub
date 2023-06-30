using System.Collections.Generic;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public interface IProcedureService
    {
        Procedure GetProcedure(int id);
        IEnumerable<Procedure> ListProcedures();
    }
}

using System.Collections.Generic;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public interface ISceneService
    {
        Scene GetScene(int id);
        IEnumerable<Scene> ListScenes();
    }
}

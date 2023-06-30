using System.Collections.Generic;

namespace VREnergy.PRO.Model
{
    [System.Serializable]
    public class Scene
    {
        public int Id { get; set; }
        public string AddressableKey { get; set; }
        public IEnumerable<SceneObject> SceneObjects { get; set; }
    }
}

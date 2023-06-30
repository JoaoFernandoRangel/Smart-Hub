using UnityEngine;

namespace VREnergy.PRO.Model
{
    [System.Serializable]
    public class SceneObject
    {
        public string UnityId { get; set; }
        public string AddressableKey { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
    }
}

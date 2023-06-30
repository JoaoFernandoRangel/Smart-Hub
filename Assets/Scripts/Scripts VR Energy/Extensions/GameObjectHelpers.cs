using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace VREnergy.Extensions
{
    public static class GameObjectHelpers
    {
        public static IEnumerable<T> FindObjectsOfInterface<T>() where T : class
        {
            return FindMonoBehaviours().OfType<T>();
        }
        
        public static IEnumerable<T> FindObjectsOfInterface<T>(bool isEnable) where T : class
        {
            return FindMonoBehaviours().Where(x => x.enabled == isEnable).OfType<T>();
        }

        private static IEnumerable<MonoBehaviour> FindMonoBehaviours()
        {
            return GameObject.FindObjectsOfType<MonoBehaviour>();
        }
    }
}
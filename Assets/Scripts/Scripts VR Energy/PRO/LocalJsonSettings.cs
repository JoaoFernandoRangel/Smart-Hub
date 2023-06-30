using System.IO;
using UnityEngine;

namespace VREnergy.PRO
{
    public static class LocalJsonSettings
    {
        private const string DatabaseFolder = "Resources/Database";

        public static string GetDatabasePath()
        {
            return Path.Combine(GetApplicationDataPath(), DatabaseFolder);
        }

        private static string GetApplicationDataPath()
        {
            return Path.GetFullPath(Application.dataPath);
        }
        
        /*
         * NOTE:
         * Application.dataPath returns the following paths:
         *
         * If Unity Editor:
         * <path to project folder>/Assets
         *
         * If Unity Build:
         * <path to project build>/<product name>_Data
         */
    }
}

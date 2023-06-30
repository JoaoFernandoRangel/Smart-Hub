using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public class LocalJsonSceneRepository : ISceneRepository
    {
        public Scene GetScene(int id)
        {
            return ListScenes().FirstOrDefault(scene => scene.Id == id);
        }

        public IEnumerable<Scene> ListScenes()
        {
            //string path = GetSceneDirectoryPath();
            IList<Scene> scenes = new List<Scene>();

            // foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            // {
            //     string json = File.ReadAllText(file);
            //     Scene scene = JsonConvert.DeserializeObject<Scene>(json);
            //     scenes.Add(scene);
            // }
            
            string json = @"{
  'Id': 1,
  'AddressableKey': 'Tests/VRTrainingRoom',
  'SceneObjects': [
    
  ]
}
";
            Scene scene = JsonConvert.DeserializeObject<Scene>(json);
            scenes.Add(scene);
            
            bool isScenesListEmpty = scenes.Count == 0;
            
            return !isScenesListEmpty ? scenes : null;
        }

        private string GetSceneDirectoryPath()
        {
            return Path.Combine(LocalJsonSettings.GetDatabasePath(), nameof(Scene));
        }
    }
}

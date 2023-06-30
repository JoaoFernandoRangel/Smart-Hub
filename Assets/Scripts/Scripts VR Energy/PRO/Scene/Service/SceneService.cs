using System.Collections.Generic;
using VREnergy.PRO.Model;

namespace VREnergy.PRO
{
    public class SceneService : ISceneService
    {
        private readonly ISceneRepository _sceneRepository;

        public SceneService(ISceneRepository sceneRepository)
        {
            _sceneRepository = sceneRepository;
        }

        public Scene GetScene(int id)
        {
            return _sceneRepository.GetScene(id);
        }

        public IEnumerable<Scene> ListScenes()
        {
            return _sceneRepository.ListScenes();
        }
    }
}

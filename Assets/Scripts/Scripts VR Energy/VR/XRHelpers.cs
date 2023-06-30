using UnityEngine.XR;

namespace VREnergy.VR
{
    /// <summary>
    /// Classe com funções auxiliares para XR.
    /// </summary>
    public static class XRHelpers
    {
        private static readonly string viveName = nameof(VRControllerModel.Vive).ToLower();
        private static readonly string oculusName = nameof(VRControllerModel.Oculus).ToLower();
        
        /// <summary>
        /// Identifica o tipo do controle a partir de uma string.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static VRControllerModel GetInputDeviceControllerModel(string deviceName)
        {
            if (deviceName != null)
            {
                deviceName = deviceName.ToLower();
                if (deviceName.Contains(viveName))
                {
                    return VRControllerModel.Vive;
                }
                if (deviceName.Contains(oculusName))
                {
                    return VRControllerModel.Oculus;
                }
            }
            
            return VRControllerModel.None;
        }
    
        /// <summary>
        /// Pega o nome do <see cref="UnityEngine.XR.InputDevice"/> e identifica qual é o tipo do controle.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static VRControllerModel GetInputDeviceControllerModel(InputDevice device)
        {
            return GetInputDeviceControllerModel(device.name);
        }
    }
}

/// <summary>
/// Lista dos tipos de controles suportados do VREnergy.
/// Veja <a href="https://docs.unity3d.com/2019.4/Documentation/Manual/xr_input.html">esse link</a> para ver a documentação do XR Input da unity.
/// </summary>
public enum VRControllerModel
{
    None,
    /// <summary>
    /// Touch Controller.
    /// Veja <a href="https://docs.unity3d.com/2019.2/Documentation/Manual/OculusControllers.html">esse link</a> para ver a documentação da Unity.
    /// </summary>
    Oculus,
    /// <summary>
    /// HTC Vive Controller.
    /// Veja <a href="https://docs.unity3d.com/2019.2/Documentation/Manual/OpenVRControllers.html">esse link</a> para ver a documentação da Unity.
    /// </summary>
    Vive
}

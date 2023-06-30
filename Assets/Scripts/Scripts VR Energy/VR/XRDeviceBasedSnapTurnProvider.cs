using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.VR
{
    public class XRDeviceBasedSnapTurnProvider : SnapTurnProviderBase
    {
        /// <summary>
        /// Sets which input axis to use when reading from controller input.
        /// </summary>
        /// <seealso cref="turnUsage"/>
        public enum InputAxes
        {
            /// <summary>
            /// Use the primary touchpad or joystick on a device.
            /// </summary>
            Primary2DAxis = 0,
            /// <summary>
            /// Use the secondary touchpad or joystick on a device.
            /// </summary>
            Secondary2DAxis = 1,
        }

        [SerializeField]
        [Tooltip("The 2D Input Axis on the controller devices that will be used to trigger a snap turn.")]
        InputAxes m_TurnUsage = InputAxes.Primary2DAxis;
        /// <summary>
        /// The 2D Input Axis on the controller devices that will be used to trigger a snap turn.
        /// </summary>
        public InputAxes turnUsage
        {
            get => m_TurnUsage;
            set => m_TurnUsage = value;
        }

        [SerializeField]
        [Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled, snap turn will not work.")]
        List<XRBaseController> m_Controllers = new List<XRBaseController>();
        /// <summary>
        /// The XRControllers that allow SnapTurn.  An XRController must be enabled in order to Snap Turn.
        /// </summary>
        public List<XRBaseController> controllers
        {
            get => m_Controllers;
            set => m_Controllers = value;
        }

        [SerializeField]
        [Tooltip("The deadzone that the controller movement will have to be above to trigger a snap turn.")]
        float m_DeadZone = 0.75f;
        /// <summary>
        /// The deadzone that the controller movement will have to be above to trigger a snap turn.
        /// </summary>
        public float deadZone
        {
            get => m_DeadZone;
            set => m_DeadZone = value;
        }

        /// <summary>
        /// Mapping of <see cref="InputAxes"/> to actual common usage values.
        /// </summary>
        static readonly InputFeatureUsage<Vector2>[] k_Vec2UsageList =
        {
            CommonUsages.primary2DAxis,
            CommonUsages.secondary2DAxis,
        };

        static readonly InputFeatureUsage<bool>[] k_BoolUsageList =
        {
            CommonUsages.primary2DAxisClick,
            CommonUsages.secondary2DAxisClick,
        };
        
        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            if (m_Controllers.Count == 0)
                return Vector2.zero;

            // Accumulate all the controller inputs
            var input = Vector2.zero;
            var feature = k_Vec2UsageList[(int)m_TurnUsage];
            var sqrDeadZone = m_DeadZone * m_DeadZone;
            for (var i = 0; i < m_Controllers.Count; ++i)
            {
                var controller = m_Controllers[i] as XRController;
                if (controller != null &&
                    controller.enableInputActions &&
                    GetDeviceInput(controller, out var controllerInput) &&
                    controllerInput.sqrMagnitude > sqrDeadZone)
                {
                    input += controllerInput;
                }
            }

            return input;
        }

        private bool GetDeviceInput(XRController controller, out Vector2 axisValue)
        {
            var axisFeature = k_Vec2UsageList[(int) m_TurnUsage];
            if (XRHelpers.GetInputDeviceControllerModel(controller.inputDevice) == VRControllerModel.Vive)
            {
                var clickFeature = k_BoolUsageList[(int) m_TurnUsage];
                return controller.inputDevice.TryGetFeatureValue(axisFeature, out axisValue) &&
                       controller.inputDevice.TryGetFeatureValue(clickFeature, out var clickValue) && clickValue;
            }
            else
            {
                return controller.inputDevice.TryGetFeatureValue(axisFeature, out axisValue);
            }
        }
    }
}
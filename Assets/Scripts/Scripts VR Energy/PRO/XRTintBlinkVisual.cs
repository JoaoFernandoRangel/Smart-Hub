using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VREnergy.PRO
{
    /// <summary>
    /// Componente responsável por fazer o material dar blink, utilizando a emissão do material.
    /// Note: requires use of a shader that supports emission (such as Standard shader) with the variant included in the game build.
    /// </summary>
    [DisallowMultipleComponent]
    public class XRTintBlinkVisual : MonoBehaviour
    {
        [SerializeField, Tooltip("Tint color for interactable.")]
        Color m_TintColor = Color.yellow;

        /// <summary>
        /// The tint color for interactable.
        /// </summary>
        public Color tintColor
        {
            get => m_TintColor;
            set => m_TintColor = value;
        }

        [SerializeField, Tooltip("Renderer(s) to use for tinting (will default to any Renderer on the GameObject if not specified).")]
        List<Renderer> m_TintRenderers = new List<Renderer>();

        /// <summary>
        /// The <see cref="Renderer"/>(s) to use for tinting (will default to any <see cref="Renderer"/> on the <see cref="GameObject"/> if not specified).
        /// </summary>
        public List<Renderer> tintRenderers
        {
            get => m_TintRenderers;
            set => m_TintRenderers = value;
        }
        
        [SerializeField, Range(0.1f, 5f)]
        private float blinkSpeed = 1f;

        MaterialPropertyBlock m_TintPropertyBlock;

        bool m_EmissionEnabled;

        bool m_HasLoggedMaterialInstance;

        private float m_lerpTime;

        private bool m_isReverseBlink;

        /// <summary>
        /// Reusable list of type <see cref="Material"/> to reduce allocations.
        /// </summary>
        static readonly List<Material> s_Materials = new List<Material>();

        protected void Awake()
        {
            if (m_TintRenderers.Count == 0)
            {
                m_TintRenderers = GetComponentsInChildren<Renderer>().ToList();
                if (m_TintRenderers.Count == 0)
                    Debug.LogWarning($"Could not find required Renderer component on {gameObject} for tint visual.", this);
            }

            // Determine if Emission is enabled on the material, or if material instances will need
            // to be created to enable it.
            m_EmissionEnabled = GetEmissionEnabled();

            m_TintPropertyBlock = new MaterialPropertyBlock();
        }

        /// <summary>
        /// Apply or remove a tint to all Renderers used for tinting.
        /// </summary>
        /// <param name="on">Whether to apply a tint when <see langword="true"/>, or remove the tint when <see langword="false"/>.</param>
        protected virtual void SetTint(bool on)
        {
            var emissionColor = on ? m_TintColor * Mathf.LinearToGammaSpace(1f) : Color.black;

            if (!m_EmissionEnabled && !m_HasLoggedMaterialInstance)
            {
                Debug.LogWarning("Emission is not enabled on a Material used by a tint visual, a Material instance will need to be created.", this);
                m_HasLoggedMaterialInstance = true;
            }

            foreach (var render in m_TintRenderers)
            {
                if (render == null)
                    continue;

                // Create material instances to enable Emission
                if (!m_EmissionEnabled)
                {
                    render.GetMaterials(s_Materials);
                    foreach (var material in s_Materials)
                    {
                        if (on)
                            material.EnableKeyword("_EMISSION");
                        else
                            material.DisableKeyword("_EMISSION");
                    }
                    s_Materials.Clear();
                }

                render.GetPropertyBlock(m_TintPropertyBlock);
                m_TintPropertyBlock.SetColor(ShaderPropertyLookup.emissionColor, emissionColor);
                render.SetPropertyBlock(m_TintPropertyBlock);
            }
        }

        /// <summary>
        /// Gets whether all shared materials on the Renderers used for tinting have emission enabled.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if all materials used for tinting have emission enabled. Returns <see langword="false"/> otherwise.</returns>
        protected virtual bool GetEmissionEnabled()
        {
            foreach (var render in m_TintRenderers)
            {
                if (render == null)
                    continue;

                render.GetSharedMaterials(s_Materials);
                foreach (var sharedMaterial in s_Materials)
                {
                    if (!sharedMaterial.IsKeywordEnabled("_EMISSION"))
                    {
                        s_Materials.Clear();
                        return false;
                    }
                }
            }

            s_Materials.Clear();
            return true;
        }

        struct ShaderPropertyLookup
        {
            public static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");
        }
        
        private IEnumerator StartBlink()
        {
            while (true)
            {
                Color lerpedColor = Color.Lerp(Color.black, m_TintColor, m_lerpTime);

                foreach (var render in m_TintRenderers)
                {
                    if (render == null)
                        continue;
                    
                    Color emissionColor = lerpedColor * Mathf.LinearToGammaSpace(1f);
                    
                    render.GetPropertyBlock(m_TintPropertyBlock);
                    m_TintPropertyBlock.SetColor(ShaderPropertyLookup.emissionColor, emissionColor);
                    render.SetPropertyBlock(m_TintPropertyBlock);
                }

                if (!m_isReverseBlink)
                {
                    m_lerpTime += Time.deltaTime * blinkSpeed;
                }
                else
                {
                    m_lerpTime -= Time.deltaTime * blinkSpeed;
                }

                if (m_lerpTime <= 0f)
                {
                    m_isReverseBlink = false;
                }
                else if(m_lerpTime >= 1f)
                {
                    m_isReverseBlink = true;
                }
                
                yield return null;
            }
        }
        
        public void ToggleTint(bool value)
        {
            SetTint(value);
            if (value)
            {
                StartCoroutine(StartBlink());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }
}
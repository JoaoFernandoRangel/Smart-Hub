using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.VR
{
    public class MesaMovimentacaoInteractable : XRGrabInteractable
    {
        [Header("Settings")]
        [Range(0.01f, 1f)]
        [Tooltip("Define a escala que o MesaMovimentação terá ao ser pegada pela mão. A escala do objeto será multiplicada por esse valor.")]
        [SerializeField] private float targetSizeRatio = 0.25f;

        [Header("Animation")]
        [Range(0f, 3f)]
        [Tooltip("Duração da animação que irá escalar o objeto.")]
        [SerializeField] private float sizingDuration = 0.25f;
    
        [Tooltip("Função Ease que será usada na animação de escala do objeto.")]
        [SerializeField] private Ease sizingEase = Ease.OutExpo;
    
        private Vector3 _originalScale = Vector3.one;

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
            _originalScale = transform.localScale;
        }

        #endregion

        /// <inheritdoc />
        protected override void OnSelectEntered(SelectEnterEventArgs interactor)
        {
            base.OnSelectEntered(interactor);

            if (IsHandInteractor((XRBaseInteractor)interactor.interactorObject))
            {
                ShrinkObject();
            }
        }

        /// <inheritdoc />
        protected override void OnSelectExited(SelectExitEventArgs interactor)
        {
            base.OnSelectExited(interactor);

            if (IsHandInteractor((XRBaseInteractor)interactor.interactorObject))
            {
                SetToOriginalScale();
            }
        }

        private void ShrinkObject()
        {
            AnimateScale(_originalScale * targetSizeRatio);
        }
    
        private void SetToOriginalScale()
        {
            AnimateScale(_originalScale);
        }

        private void AnimateScale(Vector3 target)
        {
            transform.DOScale(target, sizingDuration).SetEase(sizingEase);
        }

        /// <summary>
        /// Diz se o interactor é a mão (<see cref="UnityEngine.XR.Interaction.Toolkit.XRDirectInteractor"/>).
        /// </summary>
        /// <param name="interactor"></param>
        /// <returns></returns>
        private bool IsHandInteractor(XRBaseInteractor interactor)
        {
            return interactor is XRDirectInteractor;
        }
    }
}

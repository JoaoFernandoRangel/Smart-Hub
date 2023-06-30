using UnityEngine;

namespace VREnergy
{
    [RequireComponent(typeof(Rigidbody))]
    public class RestorePositionOnCollision : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClipToPlay;
        
        private Rigidbody rb;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 targetScale;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            StoreObjectTransform();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                SetObjectTransform();
                PlayAudio();
            }
        }

        private void StoreObjectTransform()
        {
            targetPosition = transform.position;
            targetRotation = transform.rotation;
            targetScale    = transform.localScale;
        }

        private void SetObjectTransform()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position   = targetPosition;
            transform.rotation   = targetRotation;
            transform.localScale = targetScale;
        }

        private void PlayAudio()
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioClipToPlay);
            }
        }
    }
}

using UnityEngine;

namespace IntoTheCrypt.Weapons.Sword.Controllers
{
    public class SwordController : MonoBehaviour
    {
        #region Public

        #region Members
        [Tooltip("Game object that will count for collision with enemies.")]
        public GameObject Damager;
        #endregion

        #endregion

        #region Private

        #region Members
        private Animator _animator;
        private BoxCollider _damagerCollider;
        #endregion

        #region Member Methods
        private void HandleActiveComboAction(string clipName)
        {
            switch (clipName)
            {
                case null:
                    _animator.SetTrigger("StartCombo");
                    _animator.SetInteger("CurrentCombo", 1);
                    break;
                case "Combo 1":
                    break;
                case "Post Combo 1":
                    _animator.SetInteger("CurrentCombo", 2);
                    break;
                case "Combo 2":
                    break;
                case "Post Combo 2":
                    _animator.SetInteger("CurrentCombo", 0);
                    break;
                default:
                    throw new System.Exception("Invalid combo");
            }
        }

        private void HandleIdleComboAction(string clipName)
        {
            switch (clipName)
            {
                case null:
                    _damagerCollider.enabled = false;
                    break;
                case "Combo 1":
                    _damagerCollider.enabled = true;
                    break;
                case "Post Combo 1":
                    _damagerCollider.enabled = false;
                    // Reset combo if nothing is being pressed and no other combo has been queued
                    int combo = _animator.GetInteger("CurrentCombo");
                    if (combo != 2)
                    {
                        _animator.SetInteger("CurrentCombo", 0);
                    }
                    break;
                case "Combo 2":
                    _damagerCollider.enabled = true;
                    break;
                case "Post Combo 2":
                    _damagerCollider.enabled = false;
                    _animator.SetInteger("CurrentCombo", 0);
                    break;
                default:
                    throw new System.Exception("Invalid combo");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _damagerCollider = Damager.GetComponent<BoxCollider>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {
            AnimatorClipInfo[] currentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            string clipName = null;
            if (currentClipInfo.Length > 0)
            {
                clipName = currentClipInfo[0].clip.name;
            }
            if (Input.GetMouseButtonDown(0))
            {
                HandleActiveComboAction(clipName);
            }
            else
            {
                HandleIdleComboAction(clipName);
            }
        }
        #endregion

        #endregion
    }
}
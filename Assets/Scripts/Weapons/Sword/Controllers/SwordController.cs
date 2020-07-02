using UnityEngine;

namespace IntoTheCrypt.Weapons.Sword.Controllers
{
    public class SwordController : MonoBehaviour
    {
        #region Public

        #region Members
        [Tooltip("Weapon Animator")]
        public Animator Animator;
        [Tooltip("Collider of the Damager game object")]
        public BoxCollider DamagerCollider;
        #endregion

        #endregion

        #region Private

        #region Member Methods
        private void HandleActiveComboAction(string clipName)
        {
            switch (clipName)
            {
                case null:
                    Animator.SetTrigger("StartCombo");
                    Animator.SetInteger("CurrentCombo", 1);
                    break;
                case "Combo 1":
                    break;
                case "Post Combo 1":
                    Animator.SetInteger("CurrentCombo", 2);
                    break;
                case "Combo 2":
                    break;
                case "Post Combo 2":
                    Animator.SetInteger("CurrentCombo", 0);
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
                    DamagerCollider.enabled = false;
                    break;
                case "Combo 1":
                    DamagerCollider.enabled = true;
                    break;
                case "Post Combo 1":
                    DamagerCollider.enabled = false;
                    // Reset combo if nothing is being pressed and no other combo has been queued
                    int combo = Animator.GetInteger("CurrentCombo");
                    if (combo != 2)
                    {
                        Animator.SetInteger("CurrentCombo", 0);
                    }
                    break;
                case "Combo 2":
                    DamagerCollider.enabled = true;
                    break;
                case "Post Combo 2":
                    DamagerCollider.enabled = false;
                    Animator.SetInteger("CurrentCombo", 0);
                    break;
                default:
                    throw new System.Exception("Invalid combo");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {
            AnimatorClipInfo[] currentClipInfo = Animator.GetCurrentAnimatorClipInfo(0);
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
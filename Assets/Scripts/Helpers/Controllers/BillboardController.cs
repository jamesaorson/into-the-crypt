using UnityEngine;

namespace IntoTheCrypt.Helpers.Controllers
{
    public class BillboardController : MonoBehaviour
    {
        #region Private

        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void LateUpdate()
        {
            var cameraForward = Camera.main.transform.forward;
            transform.forward = new Vector3(cameraForward.x, 0f, cameraForward.z);
        }
        #endregion

        #endregion
    }
}
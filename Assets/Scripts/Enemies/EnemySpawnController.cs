using UnityEngine;

namespace IntoTheCrypt.Enemies
{
    public class EnemySpawnController : MonoBehaviour
    {
        #region Public

        #region Members
        public GameObject SquogPrefab;
        #endregion

        #endregion

        #region Protected

        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        protected void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var enemy = Instantiate(SquogPrefab, child);
                enemy.transform.localPosition = Vector3.zero;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        protected void Update()
        {
        }
        #endregion

        #endregion
    }
}
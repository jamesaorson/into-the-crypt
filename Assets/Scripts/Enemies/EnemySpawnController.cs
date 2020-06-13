using UnityEngine;

namespace IntoTheCrypt.Enemies
{
    public class EnemySpawnController : MonoBehaviour
    {
        #region Public

        #region Members
        public GameObject TestEnemyPrefab;
        #endregion

        #endregion

        #region Protected

        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        protected void Start()
        {
            for (float i = 2; i <= 5.0; ++i)
            {
                for (float j = 2; j <= 5.0; ++j)
                {
                    var enemy = Instantiate(TestEnemyPrefab, transform);
                    enemy.transform.localPosition = new Vector3(i, 0.5f, j);
                }
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
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Public

    #region Member Methods
    public void OnExitGameClick()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnLoadGameClick()
    {
        Debug.Log("Load game");
    }

    public void OnNewGameClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void OnSettingsClick()
    {
        Debug.Log("Settings");
    }
    #endregion

    #endregion

    #region Private
    
    #region Member Methods
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Update()
    {
        
    }
    #endregion
    
    #endregion
}

#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

using UnityEngine;

public class RuntimeScript : MonoBehaviour
#if UNITY_EDITOR
    , IPreprocessBuildWithReport
#endif
{
#if UNITY_EDITOR
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Disabling objects before building...");
        DisableObjects();
    }

    void DisableObjects()
    {
        GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("EditorOnly");

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
#endif
}

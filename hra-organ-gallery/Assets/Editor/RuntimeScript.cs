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
        SetObjectState(false);
    }

    public void OnPostprocessBuild(BuildReport report) {
        Debug.Log("Enabling objects after building...");
        SetObjectState(true);
    }

    void SetObjectState(bool newState)
    {
        GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("EditorOnly");

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(newState);
        }
    }
#endif
}

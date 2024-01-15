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

    //note that thuis currenlt does not work as GameObject.FindGameObjectsWithTag("EditorOnly") only returns active game objects, not inactive ones
    public void OnPostprocessBuild(BuildReport report) {
        Debug.Log("Enabling objects after building...");
        SetObjectState(true);
    }

    void SetObjectState(bool newState)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EditorOnly");

        foreach (GameObject obj in objects)
        {
            obj.SetActive(newState);
        }
    }
#endif
}

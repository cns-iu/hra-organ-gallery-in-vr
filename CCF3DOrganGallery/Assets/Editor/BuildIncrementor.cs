using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildIncrementor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        string currentVersion = FindCurrentVersion();
        UpdateVersion(currentVersion);
    }

    private string FindCurrentVersion()
    {
        string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');
        return currentVersion[0];
    }

    private void UpdateVersion(string version)
    {
        if (int.TryParse(version, out int buildNumber))
        {
            int newBuildNumber = buildNumber++;
            string date = DateTime.Now.ToString("d");

            PlayerSettings.bundleVersion = string.Format("v0.6.0, build number: [{0}], {1]", newBuildNumber, date);
        }

        Debug.Log(PlayerSettings.bundleVersion);
    }
}

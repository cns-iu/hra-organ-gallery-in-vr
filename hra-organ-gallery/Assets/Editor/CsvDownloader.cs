using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Shared;

/// <summary>
/// A class for an EditorWindow to download a CSV file and saving it to a user-defined path. In this case, we download
/// all the AS-CT combinations with cell counts and cell percentages. It contains these for ALL organs.
/// </summary>
public class CsvDownloader : EditorWindow
{
    private string
        csvUrl =
            "https://grlc.io/api-git/hubmapconsortium/ccf-grlc/subdir/hra-pop//cell_types_in_anatomical_structurescts_per_as.csv";

    private string savePath = $"Assets/Resources/as-ct-hra-pop.csv";

    private string
        description =
            "Use this window to enter a download URL for grlc/SPARQL." +
            "Also enter a path where to save the response as a CSV.";

    [MenuItem("Tools/1. Update hra-pop Data/1. Download CSV")]
    public static void ShowWindow()
    {
        GetWindow<CsvDownloader>("Download CSV");
    }

    private void OnGUI()
    {
        // Display the description with word wrapping
        EditorGUILayout.LabelField(description, Utils.GetStyleForDescription());

        GUILayout.Label("CSV Downloader", EditorStyles.boldLabel);

        csvUrl = EditorGUILayout.TextField("URL for Downloading CSV", csvUrl);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Download CSV"))
        {
            DownloadCSV (csvUrl, savePath);
        }
    }

    private void DownloadCSV(string url, string path)
    {
        using (WebClient client = new WebClient())
        {
            try
            {
                // Set the User-Agent header
                client.Headers.Add("User-Agent", "UnityEditor");
                client.Headers.Add("accept", "text/csv");

                string csvData = client.DownloadString(url);
                File.WriteAllText (path, csvData);
                AssetDatabase.Refresh();
                Debug.Log("CSV downloaded and saved to: " + path);
            }
            catch (WebException webEx)
            {
                var response = (HttpWebResponse) webEx.Response;
                if (response != null)
                {
                    Debug.LogError("HTTP Status Code: " + response.StatusCode);
                    using (
                        var reader =
                            new StreamReader(response.GetResponseStream())
                    )
                    {
                        Debug.LogError("Response: " + reader.ReadToEnd());
                    }
                }
                else
                {
                    Debug.LogError("Failed to download CSV: " + webEx.Message);
                }
            }
            catch (IOException ioEx)
            {
                Debug.LogError("Failed to save CSV: " + ioEx.Message);
            }
        }
    }
}

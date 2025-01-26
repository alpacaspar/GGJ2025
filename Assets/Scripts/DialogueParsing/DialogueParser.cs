using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueParser : EditorWindow
{
    private string file;
    public string File
    {
        get { return file; }
        private set
        {
            file = value;
            filePath = file;
        }
    }

    private string filePath = "";
    private Object dialogueObject;

    [MenuItem("Window/Dialogue Parser")]
    public static void ShowWindow()
    {
        GetWindow<DialogueParser>("Dialogue Parser");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dialogue Parser", EditorStyles.boldLabel);

        dialogueObject = EditorGUILayout.ObjectField("Dialogue Object", dialogueObject, typeof(Object), false);

        if (dialogueObject != null)
        {
            filePath = AssetDatabase.GetAssetPath(dialogueObject);
            File = filePath;
        }

        if (!string.IsNullOrEmpty(filePath))
        {
            GUILayout.Label("Selected File: " + filePath);
        }

        if (GUILayout.Button("Parse Dialogue") && !string.IsNullOrEmpty(filePath))
        {
            ParseDialogueFile(filePath);
        }
    }

    private void ParseDialogueFile(string path)
    {
        string[] fileLines = System.IO.File.ReadAllLines(path);

        DialogueObject dialogueObject = null;
        List<DialogueObject> dialogueObjects = new();
        List<string> headerLines = new();

        foreach (string line in fileLines)
        {
            if (line.StartsWith("#"))
            {
                headerLines.Add(line.Substring(1).Trim());
                if (dialogueObject != null && dialogueObject.dialogueTexts.Count > 0)
                {
                    dialogueObjects.Add(dialogueObject);
                }
                dialogueObject = CreateInstance<DialogueObject>();
                dialogueObject.dialogueTexts = new List<string>();
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                if (dialogueObject == null)
                {
                    dialogueObject = CreateInstance<DialogueObject>();
                    dialogueObject.dialogueTexts = new List<string>();
                }
                dialogueObject.dialogueTexts.Add(line);
            }
        }

        if (dialogueObject != null && dialogueObject.dialogueTexts.Count > 0)
        {
            dialogueObjects.Add(dialogueObject);
        }

        for (int i = 0; i < dialogueObjects.Count; i++)
        {
            string assetPath = $"Assets/DialogueObjects/{Path.GetFileNameWithoutExtension(path)}_Line_{headerLines[i]}.asset";
            AssetDatabase.CreateAsset(dialogueObjects[i], assetPath);
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Dialogue Parser", "Dialogue file parsed and saved as ScriptableObjects.", "OK");

        // Insert header lines at the end
    
    }
}

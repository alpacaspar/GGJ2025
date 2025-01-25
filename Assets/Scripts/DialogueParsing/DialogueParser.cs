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

        DialogueObject dialogueObject = CreateInstance<DialogueObject>();
        dialogueObject.dialogueTexts = new List<string>();

        foreach (string line in fileLines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                dialogueObject.dialogueTexts.Add(line);
            }
        }

        string assetPath = $"Assets/DialogueObjects/{Path.GetFileNameWithoutExtension(path)}_Line.asset";
        AssetDatabase.CreateAsset(dialogueObject, assetPath);

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Dialogue Parser", "Dialogue file parsed and saved as ScriptableObjects.", "OK");
    }
}

[System.Serializable]
public class DialogueObject : ScriptableObject
{
    public List<string> dialogueTexts;
}

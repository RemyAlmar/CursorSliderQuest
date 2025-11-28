using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot_SO", menuName = "Scriptable Objects/Slot_Data")]
public class Slot_SO : ScriptableObject
{
    public string idName;
    public int id;
    public int size = 1;
    public bool canBeDeactivated = true;
    [SerializeReference] public Action action;
    public Color32 Color = new(255, 255, 255, 1);

#if UNITY_EDITOR
    public void OnEnable()
    {
        idName = string.IsNullOrEmpty(idName) ? name : idName;
        id = Animator.StringToHash(idName);
    }
    public void OnValidate()
    {
        id = Animator.StringToHash(idName);

        EditorApplication.delayCall += () =>
        {
            if (this == null) return;   // évite les refs détruites
            RenameAsset(this, idName, "SlotData_");
        };
    }
    public static void RenameAsset(Object asset, string _newName, string _prefix = null, string _suffix = null)
    {
        if (EditorApplication.isUpdating) return;
        if (EditorApplication.isCompiling) return;

        if (!string.IsNullOrEmpty(_newName))
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string _newPath = $"{_prefix}{_newName}{_suffix}";
            if (!string.IsNullOrEmpty(_newPath))
            {
                AssetDatabase.RenameAsset(path, _newPath);
            }
        }
        AssetDatabase.SaveAssets();
    }
#endif
}

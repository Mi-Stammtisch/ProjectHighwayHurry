using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Text3D : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private List<Nr> nr;

    public void SetText(string text)
    {
        this.text = text;
        Generate();
    }

    public void Generate()
    {
        foreach (Transform child in transform)
        {
            #if UNITY_EDITOR
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            #else
            while (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            #endif
        }
        float totalWidth = 0;
        foreach (char c in text)
        {
            if (c == ' ')
            {
                totalWidth -= spacing;
                continue;
            }
            Nr n = nr.Find(x => x.prefab.name == c.ToString());
            if (n == null)
            {
                Debug.LogWarning("No prefab found for " + c);
                continue;
            }
            GameObject g = Instantiate(n.prefab, transform);
            
            g.transform.localPosition = new Vector3(-totalWidth - n.leftSpacing, 0, 0);
            totalWidth += n.leftSpacing + n.rightSpacing + spacing;

        }
    }





}

[System.Serializable]
class Nr
{    
    public GameObject prefab;
    public float leftSpacing;
    public float rightSpacing;
}
#if UNITY_EDITOR

[CustomEditor(typeof(Text3D))]
class Text3DEditor: Editor
{
    public override void OnInspectorGUI()
    {
        
        Text3D t = (Text3D) target;
        if (GUILayout.Button("Generate"))
        {
            t.Generate();
        }
        base.OnInspectorGUI();
    }
}
#endif





using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of scenes to additively load")]
    public Object[] Scenes;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Object scene in Scenes)
        {
            SceneManager.LoadScene(AssetDatabase.GetAssetPath(scene), LoadSceneMode.Additive);
        }
    }
}

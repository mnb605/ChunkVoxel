using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRecouces : MonoBehaviour {
    public static MaterialRecouces Instance = null;

    [SerializeField]
    private Material[] MaterialArray;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public static Material GetMaterial(int index)
    {
        return Instance.MaterialArray[index];
    }
	
}

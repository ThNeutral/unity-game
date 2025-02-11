using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMaterial : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer m_Renderer;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer.material.SetColor("_Color", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

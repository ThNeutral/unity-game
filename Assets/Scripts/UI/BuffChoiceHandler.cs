using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffChoiceHandler : MonoBehaviour
{
    [SerializeField]
    private Button buff1Handler;

    [SerializeField]
    private Button buff2Handler;

    private LootController lootController;
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
        buff1Handler.onClick.AddListener(() => HandleSelect(1));
        buff2Handler.onClick.AddListener(() => HandleSelect(2));
        buff1Handler.gameObject.SetActive(false); 
        buff2Handler.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PresentBuffChoice()
    {
        Time.timeScale = 0f;
        buff1Handler.gameObject.SetActive(true);
        buff2Handler.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HandleSelect(int selection)
    {
        lootController.AddBuff(selection);

        Time.timeScale = 1;
        buff1Handler.gameObject.SetActive(false);
        buff2Handler.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

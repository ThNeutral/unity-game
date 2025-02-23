using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffChoiceHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject buffHandlerUIPrefab;

    private LootController lootController;

    private List<GameObject> buttons = new();
    // Start is called before the first frame update
    void Start()
    {
        lootController = FindFirstObjectByType<LootController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PresentBuffChoice(int numOfBuffs)
    {
        for (int i = 0; i < numOfBuffs; i++)
        {
            var button = Instantiate(buffHandlerUIPrefab, transform.position - new Vector3(-200, 0, 0) * ((numOfBuffs - 1) / 2 - i), Quaternion.identity, transform);
            button.GetComponent<Button>().onClick.AddListener(() => HandleSelect(i));
            buttons.Add(button);
        }


        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HandleSelect(int selection)
    {
        lootController.AddBuff(selection);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (var button in buttons)
        {
            DestroyImmediate(button);
        }
        buttons.Clear();
    }
}

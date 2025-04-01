using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MoveUIController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonPanel;

    
    

    public void ShowMoveOptions(int value)
    {
        // 👉 버튼이 몇 개 있는지 확인 (위치 조정을 위해 필요)
        int existingButtons = buttonPanel.childCount;

        GameObject btnObj = Instantiate(buttonPrefab, buttonPanel);

        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 20 * existingButtons);  

        btnObj.GetComponentInChildren<TMP_Text>().text = $"{value}칸 이동";

        btnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
          
            GameManager.Instance.OnMoveSelected(value);
            btnObj.GetComponent<Button>().interactable = false;
            Destroy(btnObj);
        });

        

        // 🟢 4(윷) 또는 5(모)일 경우, 다시 던질 수 있도록 버튼 활성화 유지
        if (value == 4 || value == 5)
        {
            GameManager.Instance.throwButton.interactable = true;
        }
        else
        {
            GameManager.Instance.throwButton.interactable = false;
        }
    }

    
}

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button controlsButton;
    
    
    private void OnEnable()
    {
        gameStartButton.onClick.AddListener(OnStartButtonClick);
        gameStartButton.onClick.AddListener(OnControlButtonClick);
    }
    
    private void OnDisable()
    {
        gameStartButton.onClick.RemoveListener(OnStartButtonClick);
        gameStartButton.onClick.RemoveListener(OnControlButtonClick);
    }

    private void OnStartButtonClick()
    {
        SceneManager.PlayGame();
    }
    
    private void OnControlButtonClick()
    {
        //ToDo: open control popup
    }
}

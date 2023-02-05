using UnityEngine;

public class InputController : MonoBehaviour
{
    private const string HorizontalInputParamName = "Horizontal";
    
    
    [SerializeField] private BasePlayer player;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            player.Attack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

        player.UpdateDirection(Input.GetAxisRaw(HorizontalInputParamName));
    }  
}
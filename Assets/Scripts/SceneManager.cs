public static class SceneManager
{
    private const string GameSceneName = "Game";
    private const string MenuSceneName = "Menu";
    
    
    public static void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName);
    }

    public static void ExitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MenuSceneName);
    }
}

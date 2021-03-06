using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public static BackgroundLoop instance;

    private void Awake()
    {
        /** Singleton pattern for background music player **/

        // Prevent duplicating the music loop
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Prevent music from being restarted on death
        DontDestroyOnLoad(gameObject);
    }
}

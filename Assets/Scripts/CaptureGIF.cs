using UnityEngine;
using System.Collections;
using System.IO;

public class CaptureGIF: MonoBehaviour
{
    public int ScreenshotWidth = 1000;
    public int ScreenshotHeight = 1000;
    public string ScreenshotDir = "Car_Baru";
    public string ScreenshotPrefix = "car_";
    public float FramesPerSecond = 30.0f;

    /// <summary>
    /// Takes an asset path (starting with 'Asset')
    /// and returns the full (absolute) system path.
    /// </summary>
    static string GetFullPath(string aAssetPath)
    {
        // Get the full path of the Unity's project folder (without 'Assets')
        var projectPath = System.IO.Path.GetDirectoryName(Application.dataPath);

        // Combine it with the given asset path
        return System.IO.Path.Combine(projectPath, aAssetPath);
    }

    /// <summary>
    /// Run when you play the scene
    /// </summary>
    [System.Obsolete]
    void Start()
    {
        StartCoroutine(TakeSequence());
    }

    /// <summary>
    /// This is where we do all the work!
    /// </summary>
    IEnumerator TakeSingleScreenshot()
    {
        // Create the screenshot directory if necessary
        string basePath = GetFullPath(ScreenshotDir);
        if (!System.IO.Directory.Exists(basePath))
        {
            System.IO.Directory.CreateDirectory(basePath);
        }

        // Compute width and height based on the camera
        int width = (int)ScreenshotWidth;
        int height = (int)ScreenshotHeight;

        // Create the render texture that we will use to capture the scene
        RenderTexture renderTexture = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);

        // Tell Unity that it needs to use it
        var prevCameraTexture = Camera.main.targetTexture;
        Camera.main.targetTexture = renderTexture;

        // Wait until rendering has happened!
        yield return new WaitForEndOfFrame();

        // Copy the content of the render texture into a Texture2D (so we can access the texture buffer)
        var prevRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotTexture.Apply();

        // Encode the buffer to PNG and write it to disk!
        byte[] bytes = screenshotTexture.EncodeToPNG();
        string screenshotPathname = System.IO.Path.Combine(basePath, ScreenshotPrefix) + ".png";
        File.WriteAllBytes(screenshotPathname, bytes);

        // Clean up
        Destroy(screenshotTexture);
        Camera.main.targetTexture = prevCameraTexture;
        RenderTexture.active = prevRenderTexture;

        Debug.Log("Screenshot Captured!");
    }

    [System.Obsolete]
    IEnumerator TakeSequence()
    {
        // ------
        // First, record the animator
        // ------

        // Give Animator a chance to initialize
        yield return new WaitForEndOfFrame();

        // We only capture the "default" state's animation
        // I.e. this component doesn't worry about trying to set the Animator in the proper substate.
        var animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        int idleStateHash = animator.GetCurrentAnimatorStateInfo(0).nameHash;

        // Force the animator to restart that clip (really just a safety) and then record the animator.
        // This is the only for us to be able to manually step the animator frame by frame and capture images.
        animator.SetTrigger("rotate");
        animator.StartRecording(0);

        // Wait for the anim to finish and return to Idle
        int stateHash = 0;
        do
        {
            yield return null;
            stateHash = animator.GetCurrentAnimatorStateInfo(0).nameHash;
        }
        while (stateHash == idleStateHash);
        do
        {
            yield return null;
            stateHash = animator.GetCurrentAnimatorStateInfo(0).nameHash;
            Debug.Log("Screenshot Captured!");
        }
        while (stateHash != idleStateHash);

        animator.StopRecording();

        // Compute the clip length as a result
        float clipLength = animator.recorderStopTime - animator.recorderStartTime;

        // ------
        // Now we can set up render texture and all that
        // ------

        // Create the screenshot directory if necessary
        string basePath = GetFullPath(ScreenshotDir);
        if (!System.IO.Directory.Exists(basePath))
        {
            System.IO.Directory.CreateDirectory(basePath);
        }

        // Compute width and height based on the camera
        int width = (int)ScreenshotWidth;
        int height = (int)ScreenshotHeight;

        // Create the render texture that we will use to capture the scene
        RenderTexture renderTexture = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);

        // Tell Unity that it needs to use it
        var prevCameraTexture = Camera.main.targetTexture;
        Camera.main.targetTexture = renderTexture;

        // Create the screenshot texture once, we'll reuse it throughout the process
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // ------
        // Tell the animator to playback the recorded animation, we'll frame by frame it and record images
        // ------

        animator.StartPlayback();

        float frameDelta = 1.0f / FramesPerSecond;
        float currentTime = 0.0f;
        int screenshotNumber = 1;
        while (currentTime < clipLength)
        {
            // Manually set the playback time on the Animator
            animator.playbackTime = animator.recorderStartTime + currentTime;

            // Do the capture
            yield return new WaitForEndOfFrame();

            // Copy the content of the render texture into a Texture2D (so we can access the texture buffer)
            var prevRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshotTexture.Apply();
            RenderTexture.active = prevRenderTexture;

            // Encode the buffer to PNG and write it to disk!
            byte[] bytes = screenshotTexture.EncodeToPNG();
            string screenshotPathname = System.IO.Path.Combine(basePath, ScreenshotPrefix) + screenshotNumber.ToString("D3") + ".png";
            File.WriteAllBytes(screenshotPathname, bytes);

            // Increment the PNG number, so we don't overwrite previous files
            ++screenshotNumber;

            // Next frame
            currentTime += frameDelta;
        }

        // Clean up
        animator.StopPlayback();
        Destroy(screenshotTexture);
        Camera.main.targetTexture = prevCameraTexture;

        Debug.Log("Screenshot Captured!");
    }
}
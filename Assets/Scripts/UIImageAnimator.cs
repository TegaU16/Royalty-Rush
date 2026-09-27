using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIImageAnimator : MonoBehaviour
    {
        public event Action AnimationFinished;

        [SerializeField] private Image targetImage;
        [SerializeField] private Sprite[] frames;
        [SerializeField] private float frameRate = 12f;
        [SerializeField] private bool startOnAwake;
        [SerializeField] private bool loop = true;

        [Header("Oscillation")]
        [SerializeField] private bool oscillateLastFrames;
        [SerializeField] private int oscillationFrameCount = 3;

        private int currentFrame;
        private int frameDirection = 1;

        private float frameTimer;
        private float frameDuration;
        private Sprite originalSprite;
        private bool isPlaying;

        private void Awake()
        {
            if (targetImage == null)
            {
                Debug.LogWarning($"UIImageAnimator on {gameObject.name}: Target Image missing.");
                enabled = false;
                return;
            }

            frameDuration = 1f / frameRate;
            originalSprite = targetImage.sprite;

            if (startOnAwake)
                Play();
        }

        private void Update()
        {
            if (!isPlaying || frames == null || frames.Length == 0) return;

            frameTimer += Time.deltaTime;

            while (frameTimer >= frameDuration)
            {
                frameTimer -= frameDuration;

                if (oscillateLastFrames)
                {
                    AdvanceOscillation();
                    continue;
                }

                AdvanceNormally();
            }
        }

        private void AdvanceNormally()
        {
            currentFrame++;

            if (currentFrame < frames.Length)
            {
                targetImage.sprite = frames[currentFrame];
                return;
            }

            if (!loop)
            {
                currentFrame = frames.Length - 1;
                targetImage.sprite = frames[currentFrame];
                isPlaying = false;

                AnimationFinished?.Invoke();
                gameObject.SetActive(false);
                return;
            }

            currentFrame = 0;
            targetImage.sprite = frames[currentFrame];
        }

        private void AdvanceOscillation()
        {
            int oscillationStart = Mathf.Max(
                0,
                frames.Length - oscillationFrameCount
            );

            currentFrame += frameDirection;

            if (currentFrame >= frames.Length)
            {
                currentFrame = frames.Length - 2;
                frameDirection = -1;
            }
            else if (currentFrame < oscillationStart)
            {
                currentFrame = oscillationStart + 1;
                frameDirection = 1;
            }

            targetImage.sprite = frames[currentFrame];
        }

        public void Play()
        {
            if (frames == null || frames.Length == 0)
            {
                Debug.LogWarning($"No frames assigned on {gameObject.name}");
                return;
            }

            if (targetImage == null)
            {
                Debug.LogWarning($"Target Image missing on {gameObject.name}");
                return;
            }

            if (frames[0] == null)
            {
                Debug.LogWarning($"First frame is null on {gameObject.name}");
                return;
            }

            currentFrame = 0;
            frameTimer = 0f;
            targetImage.sprite = frames[0];
            isPlaying = true;
        }

        public void Stop(bool restoreOriginalSprite = true)
        {
            isPlaying = false;

            if (restoreOriginalSprite && targetImage != null && originalSprite != null)
                targetImage.sprite = originalSprite;
        }

        public void SetLooping(bool shouldLoop) => loop = shouldLoop;

        public void SetOscillation(bool shouldOscillate) => oscillateLastFrames = shouldOscillate;

        public void SetOscillationFrameCount(int count) => oscillationFrameCount = Mathf.Max(2, count);
    }
}

using System;
using System.Threading;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        private const float Delay = 1.75f;
        private const float AnimationDuration = 0.65f;
        
        [SerializeField] private RectTransform _right;
        [SerializeField] private RectTransform _left;
        [SerializeField] private TMP_Text _loadingText;

        private CancellationTokenSource _loadingTextCts;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public event Action StartedShowLoadingEvent;
        public event Action FinishedShowLoadingEvent;
        
        public bool IsActive { get; private set; }

        public void Show()
        {
            IsActive = true;
            gameObject.SetActive(true);
            
            _left.anchoredPosition = Vector2.zero;
            _right.anchoredPosition = Vector2.zero;
            
            StartedShowLoadingEvent?.Invoke();
        }

        public async void Hide()
        {
            StartLoadingTextAnimation();
            await AnimationOpenAsync();
        }

        private async UniTask AnimationOpenAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Delay));
            
            float screenWidth = Screen.width;
            float elapsedTime = 0f;

            Vector2 leftStart = _left.anchoredPosition;
            Vector2 leftTarget = new Vector2(-screenWidth / 2, leftStart.y);
            
            Vector2 rightStart = _right.anchoredPosition;
            Vector2 rightTarget = new Vector2(screenWidth / 2, rightStart.y);

            while (elapsedTime < AnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / AnimationDuration;
                _left.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, t);
                _right.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, t);
                await UniTask.Yield();
            }

            FinishedShowLoadingEvent?.Invoke();
            
            _left.anchoredPosition = leftTarget;
            _right.anchoredPosition = rightTarget;
            
            StopLoadingTextAnimation();
            gameObject.SetActive(false);
            IsActive = false;
        }

        private void StartLoadingTextAnimation()
        {
            _loadingTextCts = new CancellationTokenSource();
            LoadingTextEffectAsync(_loadingTextCts.Token).Forget();
        }

        private void StopLoadingTextAnimation()
        {
            _loadingTextCts?.Cancel();
            _loadingTextCts?.Dispose();
            _loadingTextCts = null;
        }

        private async UniTask LoadingTextEffectAsync(CancellationToken cancellationToken)
        {
            string baseText = "Loading";
            while (!cancellationToken.IsCancellationRequested)
            {
                _loadingText.text = baseText + "";
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: cancellationToken);
                _loadingText.text = baseText + ".";
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: cancellationToken);
                _loadingText.text = baseText + "..";
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: cancellationToken);
                _loadingText.text = baseText + "...";
                await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken: cancellationToken);
            }
        }
    }
}

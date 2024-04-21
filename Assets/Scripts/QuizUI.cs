using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Recorder;
using UnityEditor;
#endif

namespace TikTokContentCreator
{
    public class QuizUI : MonoBehaviour
    {
        public const float HEIGHT_EXPLANATIONPOPUP_NOCODE = 350.0f;
        public const float HEIGHT_EXPLANATIONPOPUP_CODE = 1200.0f;

        [SerializeField] private QuizData data;
        [Space(5f)]
        [SerializeField] private float typingDuration;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private float initialWaitDuration;
        [SerializeField] private float questionWaitDuration;
        [SerializeField] private float correctAnswerDuration;
        [SerializeField] private float postAnswerWaitDuration;
        [SerializeField] private float explanationCodeWaitDuration;
        [SerializeField] private float likeAndFollowWaitDuration;
        [Space(5f)]
        [Header("Images:")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image clockImage;
        [Header("Texts:")]
        [SerializeField] private TextMeshProUGUI languageLabel;
        [SerializeField] private TextMeshProUGUI topicLabel;
        [SerializeField] private TextMeshProUGUI difficultyLabel;
        [SerializeField] private TextMeshProUGUI mainLabel;
        [SerializeField] private TextMeshProUGUI codeLabel;
        [SerializeField] private TextMeshProUGUI optionALabel;
        [SerializeField] private TextMeshProUGUI optionBLabel;
        [SerializeField] private TextMeshProUGUI optionCLabel;
        [SerializeField] private TextMeshProUGUI optionDLabel;
        [SerializeField] private TextMeshProUGUI likeAndFollowLabel;
        [Space(5f)]
        [Header("Containers:")]
        [SerializeField] private GameObject initialPopUp;
        [SerializeField] private GameObject codeBlockContainer;
        [SerializeField] private GameObject codeBlock;
        [SerializeField] private GameObject explanationCodeBlock;
        [SerializeField] private RectTransform optionsContainer;
        [SerializeField] private RectTransform likeAndFollowPopUp;
        [Space(5f)]
        [Header("Audio:")]
        [SerializeField] private AudioSource clockTickingSource;
        [SerializeField] private AudioClip loopForC;
        [SerializeField] private AudioClip loopForCPP;
        [SerializeField] private AudioClip loopForJAVA;
        [SerializeField] private AudioClip loopForJavaScript;
        [SerializeField] private AudioClip loopForPython;
        [SerializeField] private AudioClip loopForGeneral;
        [SerializeField] private AudioClip spaceKeySFX;
        [SerializeField] private AudioClip[] keySFXs;
        private AudioSource audioSource;
        private float originalOptionSize;
        private float augmentedOptionSize;
        private Color originalBackgroundColor;

#region Getters/Setters:
        public void SetData(QuizData _data) { data = _data; }

        private TextMeshProUGUI[] GetCorrectLabel(ref TextMeshProUGUI label)
        {
            switch (data.correctAnswer)
            {
                case 0:
                    label = optionALabel;
                break;

                case 1:
                    label = optionBLabel;
                break;

                case 2:
                    label = optionCLabel;
                break;

                case 3:
                    label = optionDLabel;
                break;

                default:
                    label = optionALabel;
                break;
            }

            List<TextMeshProUGUI> incorrectLabels = new List<TextMeshProUGUI>();

            if (optionALabel != label) incorrectLabels.Add(optionALabel);
            if (optionBLabel != label) incorrectLabels.Add(optionBLabel);
            if (optionCLabel != label) incorrectLabels.Add(optionCLabel);
            if (optionDLabel != label) incorrectLabels.Add(optionDLabel);

            return incorrectLabels.ToArray();
        }

        private AudioClip GetLoop()
        {
            switch (data.language)
            {
                case Language.C:            return loopForC;
                case Language.CPP:          return loopForCPP;
                case Language.JAVA:         return loopForJAVA;
                case Language.JavaScript:   return loopForJavaScript;
                case Language.Python:       return loopForPython;
                case Language.General:      return loopForGeneral;
                default:                    return loopForGeneral;
            }
        }
#endregion

        private void Awake()
        {
            originalOptionSize = optionALabel.fontSize;
            augmentedOptionSize = originalOptionSize * 2.0f;
            languageLabel.text = "Language: " + data.FormattedLanguage();
            topicLabel.text = "Topic: " + data.topic;
            difficultyLabel.text = "Level: " + data.difficulty.ToString();
            mainLabel.text = string.Empty;
            codeLabel.text = string.Empty;
            optionALabel.text = string.Empty;
            optionBLabel.text = string.Empty;
            optionCLabel.text = string.Empty;
            optionDLabel.text = string.Empty;
            optionALabel.fontSize = originalOptionSize;
            optionBLabel.fontSize = originalOptionSize;
            optionCLabel.fontSize = originalOptionSize;
            optionDLabel.fontSize = originalOptionSize;
            codeBlockContainer.SetActive(!string.IsNullOrEmpty(data.code));
            clockImage.fillAmount = 0.0f;
            mainLabel.text = data.question;
            mainLabel.color = Color.clear;
            optionALabel.text = "a) " + data.optionA;
            optionBLabel.text = "b) " + data.optionB;
            optionCLabel.text = "c) " + data.optionC;
            optionDLabel.text = "d) " + data.optionD;
            optionALabel.color = Color.clear;
            optionBLabel.color = Color.clear;
            optionCLabel.color = Color.clear;
            optionDLabel.color = Color.clear;
            codeLabel.text = data.explanationCode.CodeHighlighted(data.language);
            originalBackgroundColor = backgroundImage.color;
            codeBlock.transform.localScale = Vector3.zero;
            clockTickingSource.Stop();
            likeAndFollowPopUp.gameObject.SetActive(false);
            initialPopUp.SetActive(true);

            if (TryGetComponent<AudioSource>(out audioSource))
            {
                audioSource.clip = GetLoop();
                audioSource.Play();
            }
        }

        private void Start()
        {
            BeginInitialScreenRoutine();
        }

        public void UpdateData()
        {
            languageLabel.text = "Language: " + data.FormattedLanguage();
            topicLabel.text = "Topic: " + data.topic;
            difficultyLabel.text = "Level: " + data.difficulty.ToString();
            if(string.IsNullOrEmpty(data.code))
            {
                codeBlockContainer.SetActive(false);
                codeLabel.text = string.Empty;
            }
            else
            {
                codeBlockContainer.SetActive(true);
                codeLabel.text = data.code.CodeHighlighted(data.language);
            }
            mainLabel.text = data.question;
            optionALabel.text = "a) " + data.optionA;
            optionBLabel.text = "b) " + data.optionB;
            optionCLabel.text = "c) " + data.optionC;
            optionDLabel.text = "d) " + data.optionD;
        }

#region Routines:
        private void BeginInitialScreenRoutine()
        {
            StartCoroutine(Coroutines.Wait(initialWaitDuration, () => ShowPopUp(initialPopUp, false, true, BeginQuiz)));
        }

        private void FadeBackground(bool fadeIn = true, Action onFadeEnds = null)
        {
            float d = fadeIn ? fadeInDuration : fadeOutDuration;
            Color a = fadeIn ? Color.clear : originalBackgroundColor;
            Color b = fadeIn ? originalBackgroundColor : Color.clear;

            backgroundImage.color = a;

            StartCoroutine(backgroundImage.LerpColor(b, d, Math.EaseOutSine, onFadeEnds));
        }

        private void BeginQuiz()
        {
            BeginTextFadeRoutine(mainLabel, true,
                () => OnQuestionWritten());
        }

        private void BeginTypingRoutine(TextMeshProUGUI _textMesh, string _text, Action onTypingEnds = null)
        {
            StartCoroutine(TypingRoutine(_textMesh, _text, onTypingEnds));
        }

        private void BeginTextFadeRoutine(TextMeshProUGUI _textMesh, bool _fadeIn = true, Action onFadeInEnds = null)
        {
            Color color = _fadeIn ? Color.white : Color.clear;
            float duration = _fadeIn ? fadeInDuration : fadeOutDuration;

            StartCoroutine(_textMesh.LerpColor(color, duration, Math.EaseOutSine, onFadeInEnds));
        }

        private void BeginTextErasureRoutine(TextMeshProUGUI _textMesh, Action onTextErased = null)
        {
            Action<string> onTextModified = text =>
            {
                _textMesh.text = text;
            };
            StartCoroutine(Coroutines.DecrementTextOverTime(_textMesh.text, Coroutines.WAIT_TEXTDECREMENT, onTextModified, onTextErased));
        }

        private void BeginOptionsRoutine(Action onOptionsEnd = null)
        {
            Color c = Color.white;
            float additionalWait = 0.0f;

            additionalWait += optionALabel.text.CalculateReadTime();
            additionalWait += optionBLabel.text.CalculateReadTime();
            additionalWait += optionCLabel.text.CalculateReadTime();
            additionalWait += optionDLabel.text.CalculateReadTime();

            Action OnOptionsEnd = () =>
            {
                Wait(additionalWait, onOptionsEnd);
            };

            StartCoroutine(Coroutines.RunAllCoroutines(OnOptionsEnd,
                optionALabel.LerpColor(c, fadeInDuration, Math.EaseOutSine),
                optionBLabel.LerpColor(c, fadeInDuration, Math.EaseOutSine),
                optionCLabel.LerpColor(c, fadeInDuration, Math.EaseOutSine),
                optionDLabel.LerpColor(c, fadeInDuration, Math.EaseOutSine)));
        }

        private void BeginQuestionWaitRoutine(Action onWaitEnds = null)
        {
            clockImage.color = Color.white;
            clockTickingSource.time = 0.0f;
            clockTickingSource.Play();
            StartCoroutine(Coroutines.DoWhileWaiting(questionWaitDuration, (t) => clockImage.fillAmount = t, onWaitEnds));
        }

        private void ShowPopUp(GameObject _popUp, bool _show, bool _fade = true, Action onRoutineEnds = null)
        {
            Func<float, float> f = _show ? Math.EaseOutBounce : Math.EaseInBack;
            Vector3 a = _show ? Vector3.zero : Vector3.one;
            Vector3 b = _show ? Vector3.one : Vector3.zero;
            float d = _show ? fadeInDuration : fadeOutDuration;

            _popUp.transform.localScale = a;

            Action showPopUp = () =>
            {
                StartCoroutine(_popUp.transform.Scale(b, d, f, onRoutineEnds));
            };

            switch (_show)
            {
                case true:
                    if(_fade) FadeBackground(true, showPopUp);
                    else showPopUp();
                break;

                case false:
                    if(_fade) StartCoroutine(_popUp.transform.Scale(b, d, f, ()=> FadeBackground(false, onRoutineEnds)));
                    else showPopUp();
                break;
            }
        }

        private void ShowCodeBlock(GameObject _codeBlock, bool _show = true, Action onRoutineEnds = null)
        {
            Func<float, float> f = _show ? Math.EaseOutBounce : Math.EaseInBack;
            Vector3 a = _show ? Vector3.zero : Vector3.one;
            Vector3 b = _show ? Vector3.one : Vector3.zero;
            float d = _show ? fadeInDuration : fadeOutDuration;

            _codeBlock.transform.localScale = a;

            StartCoroutine(_codeBlock.transform.Scale(b, d, f, onRoutineEnds));
        }

        private void ShowCorrectResult()
        {
            clockImage.fillAmount = 1.0f;
            clockTickingSource.Stop();
            StartCoroutine(ShowCorrectResultRoutine(OnAnswerShown));
        }

        private void ShowLikeAndFollowPopUp()
        {
            likeAndFollowPopUp.gameObject.SetActive(true);
            likeAndFollowPopUp.transform.localScale = Vector3.zero;
            ShowPopUp(likeAndFollowPopUp.gameObject, true, false,
            ()=>
            {
                Wait(likeAndFollowWaitDuration, StopRecording);
            });
        }

        private void Wait(float t, Action onWaitEnds = null)
        {
            StartCoroutine(Coroutines.Wait(t, onWaitEnds));
        }

        private void StopRecording()
        {
#if UNITY_EDITOR
            RecorderWindow window = (RecorderWindow)EditorWindow.GetWindow(typeof(RecorderWindow));

            if (window != null && window.IsRecording()) window.StopRecording();
#endif
            string tiktokTitle = data.GetTikTokTitle();

            GUIUtility.systemCopyBuffer = data.GetTikTokTitle();
            Debug.Log(tiktokTitle + " title copied to Clipboard.");
        }
#endregion

#region Callbacks
        private void OnQuestionWritten()
        {
            switch (string.IsNullOrEmpty(data.code))
            {
                case true:
                {
                    /* If the quiz data has no code:
                     * 1.- Show options.
                     * 2.- Wait for clock to finish ticking.
                     * 3.- Show correct answer.
                    */
                    BeginOptionsRoutine(() => BeginQuestionWaitRoutine(ShowCorrectResult));
                }
                break;

                case false:
                {
                    /* If the quiz data has code
                     * 1.- Show Code block.
                     * 2.- Write Code snippet.
                     * 3.- Show options.
                     * 4.- Wait for clock to finish ticking.
                     * 5.- Show correct answer.
                    */
                    ShowCodeBlock(codeBlock, true,
                        () => BeginTypingRoutine(codeLabel, data.code.CodeHighlighted(data.language),
                            () => BeginOptionsRoutine(() => BeginQuestionWaitRoutine(ShowCorrectResult))));
                }
                break;
            }
        }

        private void OnAnswerShown()
        {
            switch (string.IsNullOrEmpty(data.explanation))
            {
                case true:
                    // If there is no explanation: Show Like & Follow Pop-Up.
                    ShowLikeAndFollowPopUp();
                break;

                case false:
                {
                    /* If there is explanation:
                     * 1.- Fade-out question.
                     * 2.- Fade-in explanation in that same Text field.
                     * 3.- Wait until explanation is read.
                     * 4.- Proceed to OnExplanationShown's callback.
                     */
                    BeginTextFadeRoutine(mainLabel, false,
                    ()=>
                    {
                        float waitTime = data.explanation.CalculateReadTime();

                        mainLabel.text = data.explanation;
                        BeginTextFadeRoutine(mainLabel, true,
                            ()=> Wait(waitTime, OnExplanationShown));
                    });
                }
                break;

            }
        }

        private void OnExplanationShown()
        {
            switch(string.IsNullOrEmpty(data.explanationCode))
            {
                case true:
                    // If there is no explanation code: Show Like & Follow Pop-Up.
                    ShowLikeAndFollowPopUp();
                break;

                case false:
                    /* If there is explanation code:
                     * 1.- Erase current code.
                     * 2.- Write explanation code in that same space.
                     * 3.- Wait until code has been read.
                     * 4.- Show Like & Follow Pop-Up
                     */
                    BeginTextErasureRoutine(codeLabel,
                        () => BeginTypingRoutine(codeLabel, data.explanationCode.CodeHighlighted(data.language),
                            () => Wait(explanationCodeWaitDuration, ShowLikeAndFollowPopUp)));
                break;
            }
        }
#endregion


#region Coroutines
        private IEnumerator TypingRoutine(TextMeshProUGUI _textMesh, string _text, Action onTypingEnds = null)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder tagBuilder = new StringBuilder();
            AudioClip keySFX = null;
            bool spaceWritten = false;
            bool spaceSFXPlayed = false;
            bool tagOpen = false;
            _textMesh.text = string.Empty;

            for(int i = 0; i < _text.Length; i++)
            {
                char character = _text[i];

                if (character == '<' && (_text[i + 1] == 'c' || _text[i + 1] == '/') && !tagOpen)
                {
                    tagBuilder.Clear();
                    tagBuilder.Append(character);
                    tagOpen = true;
                    continue;

                } else if (character == '>')
                {
                    tagBuilder.Append(character);
                    tagOpen = false;
                    builder.Append(tagBuilder.ToString());
                } else if (tagOpen)
                {
                    tagBuilder.Append(character);
                    continue;

                } else
                {
                    spaceWritten = character == ' ';
                    keySFX = spaceWritten ? spaceKeySFX : keySFXs.Random();

                    if (keySFX != null && (!spaceWritten || (spaceWritten && !spaceSFXPlayed)))
                    audioSource.PlayOneShot(keySFX);

                    spaceSFXPlayed = spaceWritten;
                    builder.Append(character);
                    _textMesh.text = builder.ToString();

                    WaitForSeconds wait = new WaitForSeconds(typingDuration);
                    yield return wait;
                }
            }

            if (onTypingEnds != null) onTypingEnds();
        }

        private IEnumerator ShowCorrectResultRoutine(Action onAnswerShown = null)
        {
            TextMeshProUGUI correctLabel = null;
            TextMeshProUGUI[] incorrectLabels = GetCorrectLabel(ref correctLabel);
            Color a = Color.white;
            Color b = Color.white;
            float t = 0.0f;
            float i = 1.0f / fadeOutDuration;
            b.a = 0.3f;

            while (t < 1.0f)
            {
                foreach(TextMeshProUGUI incorrectLabel in incorrectLabels)
                {
                    incorrectLabel.color = Color.LerpUnclamped(a, b, t);
                }
                clockImage.color = Color.LerpUnclamped(a, Color.clear, t);
                t += (Time.deltaTime * i);
                yield return null;
            }

            foreach(TextMeshProUGUI incorrectLabel in incorrectLabels)
            {
                incorrectLabel.color = b;
            }
            clockImage.color = Color.clear;

            /*float originalSize = originalOptionSize;
            float targetSize = augmentedOptionSize;
            float d = 1.0f / correctAnswerDuration;
            t = 0.0f;


            while (t < 1.0f)
            {
                correctLabel.fontSize = Mathf.LerpUnclamped(originalSize, targetSize, Math.EaseOutBounce(t));
                t += (Time.deltaTime * d);
                yield return null;
            }

            correctLabel.fontSize = targetSize;*/

            WaitForSeconds wait = new WaitForSeconds(postAnswerWaitDuration);

            yield return wait;

            if (onAnswerShown != null) onAnswerShown();
        }
#endregion
    }
}

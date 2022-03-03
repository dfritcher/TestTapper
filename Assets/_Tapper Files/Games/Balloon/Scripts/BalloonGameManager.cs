using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tapper.GamePlay.BalloonPop
{
    public class BalloonGameManager : MonoBehaviour
    {
        #region Fields, Properties
        #region UI Elements 
        [Header("Main Screen References"), Space(8)]
        [SerializeField] private CanvasGroup _mainScreen = null;
        [SerializeField] private CanvasGroup _gameOverlay = null;
        [SerializeField] private GameObject _readyPanel = null;
        [SerializeField] private TextMeshProUGUI[] _titleDisplay = null;
        [SerializeField] private UIButton _playButton = null;
        
        [Header("Game Screen References"), Space(8)]
        [SerializeField] private CanvasGroup _gameElements = null;
        [SerializeField] private TextMeshProUGUI _userPromptDisplay = null;
        [SerializeField] private TextMeshProUGUI _tapCountDisplay = null;
        [SerializeField] private TextMeshProUGUI _balloonState = null;
        [SerializeField] private SpriteRenderer _balloon = null;
        [SerializeField] private Image _sliderFill = null;
        [SerializeField] private Image _star = null;
        [SerializeField] private Slider _popSlider = null;
        
        [Header("End Screen References"), Space(8)]
        [SerializeField] private CanvasGroup _endScreen = null;
        [SerializeField] private UIButton _playAgainButton = null;
        [SerializeField] private TextMeshProUGUI _currentTimeDisplay = null;
        [SerializeField] private TextMeshProUGUI _bestTimeDisplay = null;
        [SerializeField] private TextMeshProUGUI _newRecordDisplay = null;
        #endregion UI Elements (end)

        #region Class References
        [Header("Class References"), Space(8)]
        [SerializeField] private TapHandler _tapHandler = null;
        [SerializeField] private TimerHandler _timerHandler = null;
        [SerializeField] private TimerUtility _timerUtility = null;
        #endregion Class References (end)

        #region Audio
        [Header("Audio References"), Space(8)]
        [SerializeField] private AudioClip _balloonPop = null;
        [SerializeField] private AudioClip[] _audioPrompts = null;
        [SerializeField] private AudioSource _audioSourceSFX = null;
        [SerializeField] private AudioSource _audioSourceMusic = null;
        #endregion Audio (end)

        #region Animation
        [Header("Animation References"), Space(8)]
        [SerializeField] private Animator _balloonAnimator = null;
        [SerializeField] private Animator _topperAnimator = null;
        [SerializeField] private ParticleSystem _balloonPopPS = null;
        #endregion Animation (end)

        #region Local Variables
        [Header("Local References"), Space(8)]
        [SerializeField] private Game _game; // reference to this games information. Set in scene.
        [SerializeField] private Color _orange;
        [SerializeField] private int _normalFontSize = 200;
        [SerializeField] private int _increasedFontSize = 300;
        [SerializeField] private int _largeFontSize = 400;
        [SerializeField] private int _titleDisplayIndexer;
        [SerializeField] private Vector3 _balloonOriginalLocalScale;
        private float _timer = 0;
        private WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
        private bool _setRoundEnd = false;
        public bool IsRoundEnded { get { return _setRoundEnd; } }
        private bool _triggerRoundEnd = true;
        private float _increaseAmount = .05f;
        private bool _hasTransitioned = false;

        private GameDirector _gameDirector = null;
        private PlayerManager _playerManager = null;
        #endregion Local Variables (end)
        #endregion Fields, Properties (end)

        #region Methods
        #region Unty Engine Methods
        private void Start()
        {
            _gameDirector = FindObjectOfType<GameDirector>();
            _playerManager = FindObjectOfType<PlayerManager>();
            SetMainScreenState(true);
            SetGameElementsState(false);
            SetGameScreenState(false);
            SetEndScreenState(false);
            SetInitialState();
            SetButtonEvents();
        }

        private void Update()
        {
            if (_setRoundEnd)
                return;
            if (_balloon.transform.localScale.x >= 2.09f)
            {
                _setRoundEnd = true;
            }

            if (_setRoundEnd && _triggerRoundEnd)
            {
                InitializeEndScreen();
                HandleGameEnd();
                SetEndScreenState(true);
                StartCoroutine(PlayAgainCoroutine());
            }
            _timer += Time.deltaTime;
        }
        #endregion Unity Engine Methods (end)

        #region Unity Called Methods
        public void PlayClicked(UIButton uIButton)
        {
            SetMainScreenState(false);
            SetGameElementsState(true);
            SetGameScreenState(true);
            StartCoroutine(PlayClickedMainHandler());
        }

        private IEnumerator PlayClickedMainHandler()
        {
            yield return StartCoroutine(PlayClickedCoroutine());
            yield return StartCoroutine(StartGameCoroutine());
        }

        private void PlayAgainButton_OnClicked(UIButton uIButton)
        {
            SetMainScreenState(true);
            SetGameScreenState(false);
            SetGameElementsState(false);
            SetEndScreenState(false);
            SetInitialState();
        }

        public IEnumerator PlayAgainCoroutine()
        {
            yield return new WaitForSeconds(2f);
            _playAgainButton.gameObject.SetActive(true);
        }

        public void OnBackButtonClicked()
        {
            SceneManager.LoadScene("Main Game");
        }
        #endregion Unity Called Methods (end)

        internal void TapHandler(int tapCount)
        {
            _balloon.transform.localScale = new Vector3(_balloon.transform.localScale.x + _increaseAmount, _balloon.transform.localScale.y + _increaseAmount, _balloon.transform.localScale.z);

            if (_balloon.transform.localScale.x <= 1f)
            {
                _sliderFill.color = Color.green;
                _star.color = Color.green;
                _topperAnimator.SetFloat("Speed", 1.0f);
                _topperAnimator.SetBool("Shake", false);
                _increaseAmount = .05f;
            }
            else if (_balloon.transform.localScale.x <= 1.75f)
            {
                _sliderFill.color = Color.yellow;
                _star.color = Color.yellow;
                _topperAnimator.SetFloat("Speed", 1.5f);
                _topperAnimator.SetBool("Shake", true);
                _increaseAmount = .025f;

            }
            else if (_balloon.transform.localScale.x <= 1.9f)
            {
                _sliderFill.color = _orange;
                _star.color = _orange;
                _topperAnimator.SetFloat("Speed", 1.9f);
                _increaseAmount = .007f;
            }
            else if (_balloon.transform.localScale.x <= 2.2f)
            {
                _sliderFill.color = Color.red;
                _star.color = Color.red;
                _topperAnimator.SetFloat("Speed", 2.3f);
                _increaseAmount = .005f;
            }

            _timer = 0f;
            HandlePopSlider();
            _tapCountDisplay.text = tapCount.ToString();
        }

        internal void HandlePopSlider()
        {
            _popSlider.value = _balloon.transform.localScale.x;
        }

        private IEnumerator PlayClickedCoroutine()
        {
            yield return new WaitForSeconds(.1f);

            _userPromptDisplay.text = "<color=red><size=40%>READY</size></color>";
            _audioSourceSFX.PlayOneShot(_audioPrompts[0]);
            yield return _waitOneSecond;

            _userPromptDisplay.text = "<color=yellow><size=60%>SET</size></color>";
            _audioSourceSFX.PlayOneShot(_audioPrompts[0]);
            yield return _waitOneSecond;

            _userPromptDisplay.text = "<color=green><size=100%>TAP!</size></color>";
            _audioSourceSFX.PlayOneShot(_audioPrompts[1]);
            yield return new WaitForSeconds(.5f);
            _readyPanel.SetActive(false);
        }

        private IEnumerator StartGameCoroutine()
        {
            yield return new WaitForSeconds(.5f);
            _timerHandler.StartTimer();
        }

        private void SetInitialState()
        {
            _hasTransitioned = false;
            _balloonAnimator.SetBool("Pop", false);
            _tapHandler.Reset();
            _setRoundEnd = false;
            _triggerRoundEnd = true;
            _readyPanel.SetActive(true);
            _userPromptDisplay.text = string.Empty;
            _balloonState.text = string.Empty;
            _balloon.transform.localScale = _balloonOriginalLocalScale;
            _popSlider.value = _balloonOriginalLocalScale.x;
            _sliderFill.color = Color.green;
            _star.color = Color.green;
            _tapCountDisplay.text = "0";
            _titleDisplayIndexer = 0;
            _timerHandler.ResetTimer();
            _playAgainButton.Reset();
            _playAgainButton.SetButtonVisibility(false);
        }

        private void SetButtonEvents()
        {
            _timerUtility.OneSecond += TimerHandler_OneSecond;
            _playButton.OnClicked += PlayClicked;
            _playAgainButton.OnClicked += PlayAgainButton_OnClicked;            
        }

        private void SetMainScreenState(bool isEnabled)
        {
            _mainScreen.alpha = isEnabled ? 1 : 0;
            _mainScreen.interactable = _mainScreen.blocksRaycasts = isEnabled;

        }

        private void SetGameScreenState(bool isEnabled)
        {
            _gameOverlay.alpha = isEnabled ? 1 : 0;
            _gameOverlay.interactable = _gameOverlay.blocksRaycasts = isEnabled;
        }

        private void SetGameElementsState(bool isEnabled)
        {
            _gameElements.alpha = isEnabled ? 1 : 0;
            _gameElements.interactable = _gameElements.blocksRaycasts = isEnabled;
        }

        private void TimerHandler_OneSecond()
        {
            //if (_titleDisplayIndexer == 0)
            //{
            //    //Increase first item in the array
            //    _titleDisplay[_titleDisplayIndexer].fontSize = _increasedFontSize;

            //    //Descrease Last item size in the array
            //    _titleDisplay[_titleDisplay.Length - 1].fontSize = _normalFontSize;
            //}
            //else if (_titleDisplayIndexer == _titleDisplay.Length - 1)
            //{
            //    //Double the last item in the array
            //    _titleDisplay[_titleDisplayIndexer].fontSize = _largeFontSize;
            //    //Decrease previous item size            
            //    _titleDisplay[_titleDisplayIndexer - 1].fontSize = _normalFontSize;
            //}
            //else
            //{
            //    //Increase index item size
            //    _titleDisplay[_titleDisplayIndexer].fontSize = _increasedFontSize;

            //    //Decrease previous indexed item size            
            //    _titleDisplay[_titleDisplayIndexer - 1].fontSize = _normalFontSize;
            //}
            //_titleDisplayIndexer++;
            //if (_titleDisplayIndexer >= _titleDisplay.Length)
            //    _titleDisplayIndexer = 0;
        }

        #region End Screen Related Methods
        private void HandleGameEnd()
        {
            _currentTimeDisplay.text = _timerHandler.GetTimeDisplay();
            var activePlayer = _playerManager.GetActivePlayers().First();
            if (activePlayer == null)
            {
                return;
            }
            else
            {
                //Check for best time and new record
                var game = activePlayer.Games?.Find(g => g.Id == _game.Id);
                if (game == null)
                {
                    _playerManager.AddNewGameForPlayer(activePlayer, new Game() { Id = _game.Id, Name = _game.Name, Score = _timerHandler.GetTime().ToString("g")});
                    _bestTimeDisplay.text = _currentTimeDisplay.text;
                }
                else
                {
                    var previousTime = TimeSpan.Parse(game.Score);
                    if (_timerHandler.GetTime().TotalMilliseconds < previousTime.TotalMilliseconds)
                    {
                        _newRecordDisplay.gameObject.SetActive(true);
                        _bestTimeDisplay.text = _currentTimeDisplay.text;
                        _playerManager.UpdateGameScore(activePlayer, game, _timerHandler.GetTime().ToString("g"));                        
                    }
                    else
                    {
                        _bestTimeDisplay.text = TimeSpan.Parse(game.Score).ToString(@"ss\:ff");
                    }
                }
            }                        
        }

        private void InitializeEndScreen()
        {
            _currentTimeDisplay.text = string.Empty;
            _bestTimeDisplay.text = string.Empty;
            _newRecordDisplay.gameObject.SetActive(false);
            _triggerRoundEnd = false;
            _balloonState.text = "POP!";
            _balloonAnimator.SetBool("Pop", true);
            _balloonPopPS.Play();
            _audioSourceSFX.PlayOneShot(_balloonPop);
            _timerHandler.StopTimer();
            _topperAnimator.SetBool("Shake", false);
            _tapHandler.SetButtonVisibility(false);
        }

        private void SetEndScreenState(bool isEnabled)
        {
            _endScreen.alpha = isEnabled ? 1 : 0;
            _endScreen.interactable = _endScreen.blocksRaycasts = isEnabled;
        }
        #endregion End Screen Related Methods (end)
        #endregion Methods (end)
    }
}
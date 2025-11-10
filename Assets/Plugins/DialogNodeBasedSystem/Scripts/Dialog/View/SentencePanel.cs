using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cherrydev
{
    public class SentencePanel : MonoBehaviour
    {
	    [SerializeField] private TextMeshProUGUI _dialogNameTextLeft;
	    [SerializeField] private TextMeshProUGUI _dialogTextLeft;
	    [SerializeField] private Image _dialogCharacterImageLeft;
	    [SerializeField] private TextMeshProUGUI _dialogNameTextRight;
	    [SerializeField] private TextMeshProUGUI _dialogTextRight;
	    [SerializeField] private Image _dialogCharacterImageRight;

		private TextMeshProUGUI _dialogNameText;
        private TextMeshProUGUI _dialogText;
        private Image _dialogCharacterImage;

        private const string _CharaString = "H4CKROÏ";
        private const string _KidString = "KID";


		private string _currentFullText;
        
        /// <summary>
        /// Setting dialogText max visible characters to zero
        /// </summary>
        public void ResetDialogText()
        {
			_dialogTextLeft.maxVisibleCharacters = 0;
			_dialogTextRight.maxVisibleCharacters = 0;
            _currentFullText = string.Empty;
        }

        /// <summary>
        /// Set dialog text max visible characters to dialog text length
        /// </summary>
        /// <param name="text"></param>
        public void ShowFullDialogText(string text)
        {
            _currentFullText = text;
            _dialogText.text = text;
            _dialogText.maxVisibleCharacters = text.Length;
        }

        /// <summary>
        /// Increasing max visible characters
        /// </summary>
        public void IncreaseMaxVisibleCharacters() => _dialogText.maxVisibleCharacters++;
        
        /// <summary>
        /// Assigning dialog name text, character image sprite and dialog text
        /// </summary>
        public void Setup(string characterName, string text, Sprite sprite)
        {
            if(characterName == _CharaString)
            {
                _dialogNameText = _dialogNameTextLeft;
                _dialogText = _dialogTextLeft;
                _dialogCharacterImage = _dialogCharacterImageLeft;

				_dialogNameTextRight.gameObject.SetActive(false);
                _dialogTextRight.gameObject.SetActive(false);
                _dialogCharacterImageRight.gameObject.SetActive(false);

			}
            else
            {
                _dialogNameText = _dialogNameTextRight;
                _dialogText = _dialogTextRight;
                _dialogCharacterImage = _dialogCharacterImageRight;

                _dialogNameTextLeft.gameObject.SetActive(false);
                _dialogTextLeft.gameObject.SetActive(false);
                _dialogCharacterImageLeft.gameObject.SetActive(false);
			}

			_dialogNameText.text = characterName;
            _dialogText.text = text;
            _currentFullText = text;

            _dialogNameText.gameObject.SetActive(true);
            _dialogText.gameObject.SetActive(true);
            _dialogCharacterImage.gameObject.SetActive(true);

			if (sprite == null)
            {
                _dialogCharacterImage.color = new Color(_dialogCharacterImage.color.r,
                    _dialogCharacterImage.color.g, _dialogCharacterImage.color.b, 0);
                return;
            }

            _dialogCharacterImage.color = new Color(_dialogCharacterImage.color.r,
                _dialogCharacterImage.color.g, _dialogCharacterImage.color.b, 255);
            _dialogCharacterImage.sprite = sprite;
        }
    }
}
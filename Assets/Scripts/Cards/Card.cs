using Holo.Cam;
using Holo.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Holo.Cards
{
    /// <summary>
    /// Monobehaviour responsible for displaying and manipulating card gameobjects
    /// </summary>
    public class Card : MonoBehaviour, IRaycastable
    {
        // required ScriptableObject
        public CardData CardData;

        // components to update with data
        [SerializeField] private TextMeshProUGUI cardNameTMP;
        [SerializeField] private TextMeshProUGUI powerTMP;
        [SerializeField] private TextMeshProUGUI cardDescriptionTMP;
        [SerializeField] private Image imageImage;
        [SerializeField] private Image lineImage;

        CardLocation activeLocation;

        [SerializeField][Min(0f)] private float moveSpeed = 5f;
        [SerializeField][Min(0f)] private float rotationSpeed = 540f;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        public int Position { get; set; }
        public int Power { get; private set; }

        public Effect Effect => CardData.Effect;
        public bool HasEffect => CardData.HasEffect;

        public bool IsPlayers = false;

        private Animator anim;
        private AudioPlayer audioPlayer;

        private void Awake()
        {
            anim = this.GetComponent<Animator>();
            audioPlayer = FindObjectOfType<AudioPlayer>();
            DisplayCard(CardData);

            targetPosition = this.transform.position;
            targetRotation = this.transform.rotation;
        }

        public void DisplayCard(CardData newCardData)
        {
            if (newCardData == null) return;
            // overwrites default with new value
            CardData = newCardData;
            this.Power = CardData.Power;

            // assign data to components 
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (this.CardData == null) return;

            // assign data to components 
            cardNameTMP.text = CardData.CardName;
            powerTMP.text = this.Power.ToString();
            cardDescriptionTMP.text = CardData.CardDescription;
            imageImage.sprite = CardData.Image;
            lineImage.sprite = CardData.LineImage;
        }

        public void SetActiveLocation(CardLocation activeLocation)
        {
            this.activeLocation = activeLocation;
            this.activeLocation.AddCardToLocation(this);
        }

        public void OnHover()
        {
            activeLocation?.SetHighlightedCard(this);
        }

        public void OnEndHover()
        {
            activeLocation?.SetHighlightedCard(null);
        }

        private void Update()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public void MoveToPoint(Vector3 position, Quaternion rotation)
        {
            this.targetPosition = position;
            this.targetRotation = rotation;
        }

        public void Attack()
        {
            anim.SetTrigger("attack");
            
        }

        public void SetPower(int power)
        {
            this.Power = power;
            UpdateDisplay();
        }

        public void ResetPower()
        {
            this.Power = CardData.Power;
            UpdateDisplay();
        }

        public void CamShake()
        {
            CameraShake.instance.StandardCameraShake();
            audioPlayer.playBattleClip();
        }
    }
}

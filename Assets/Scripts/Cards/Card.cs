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

        CardLocation activeLocation;

        [SerializeField][Min(0f)] private float moveSpeed = 5f;
        [SerializeField][Min(0f)] private float rotationSpeed = 540f;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        public int Position { get; set; }
        public int Power { get; private set; }

        private Animator anim;

        private void Awake()
        {
            anim = this.GetComponent<Animator>();
            DisplayCard(CardData);

            targetPosition = this.transform.position;
            targetRotation = this.transform.rotation;
        }

        public void DisplayCard(CardData newCardData)
        {
            if (newCardData == null) return;
            // overwrites default with new value
            CardData = newCardData;

            // assign data to components 
            cardNameTMP.text = CardData.CardName;
            powerTMP.text = CardData.Power.ToString();
            cardDescriptionTMP.text = CardData.CardDescription;
            imageImage.sprite = CardData.Image;
            this.Power = CardData.Power;
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

        private void Start()
        {
        //    Attack();
        }

        public void Attack()
        {
            Debug.Log("Attack called");
            anim.SetTrigger("attack");
        }

        public void CamShake()
        {
            Debug.Log("Cam shake called");
            CameraShake.instance.StandardCameraShake();
        }
    }
}

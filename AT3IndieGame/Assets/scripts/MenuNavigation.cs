using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    public delegate void MenuInputDelegate(float axis);

    [SerializeField] private float bufferTime = 0.2f;

    private float timer = -1f;
    private int currentButtonIndex = 0;
    private MenuButton currentButton;

    public MenuButton SelectedButton
    {
        get { return currentButton; }
        set
        {
            if(Buttons.Contains(value) == true)
            {
                currentButton = value;
                currentButtonIndex = Buttons.IndexOf(currentButton);
            }
        }
    }
    public List<MenuButton> Buttons { get; private set; } = new List<MenuButton>();
    public int CurrentButtonIndex
    {
        get { return currentButtonIndex; }
        set
        {
            currentButtonIndex = value;
            if(currentButtonIndex >= Buttons.Count)
            {
                currentButtonIndex = 0;
            }
            else if(currentButtonIndex < 0)
            {
                currentButtonIndex = Buttons.Count - 1;
            }
        }
    }

    public event MenuInputDelegate VerticalInputEvent = delegate { };

    private void Awake()
    {
        foreach(MenuButton button in GetComponentsInChildren<MenuButton>())
        {
            Buttons.Add(button);
        }
        VerticalInputEvent = new MenuInputDelegate(SelectButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Buttons.Count > 0)
        {
            SelectButton(0);
            ///This is being called before the start functions on each button
            ///which means the selection colour is being overriden by the default colour
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region timer functionality
        if (timer < 0)
        {
            float axis = Input.GetAxis("Vertical");
            if(axis != 0)
            {
                VerticalInputEvent.Invoke(axis);
            }
        }
        else
        {
            timer += Time.deltaTime;
            if(timer >= bufferTime)
            {
                timer = -1;
            }
        }
        #endregion

        if(SelectedButton != null)
        {
            if(Input.GetButtonDown("Submit") == true)
            {
                SelectedButton.Activate();
            }
        }
    }

    private void SelectButton(float axis)
    {
        if (axis < 0)
        {
            CurrentButtonIndex++;
        }
        else if(axis > 0)
        {
            CurrentButtonIndex--;
        }
        Buttons[currentButtonIndex].Select();
        timer = 0;
    }
}

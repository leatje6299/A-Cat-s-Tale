using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Story : MonoBehaviour
{
    [SerializeField] private List<Sprite> storyImages;
    [SerializeField] private Image storyImage;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject continueText;
    [SerializeField] private TMP_Text text;
    public float typingSpeed = 0.02f;
    private int storyImageOrder = 0;
    private string[] textStory = new string[] { "There was once a magical cat wandering around chasing a beautiful robin.",
                                                                    "He started swinging around with his tail to reach the bird in high places.",
                                                                    "Suddenly, he tripped and fell from the tree, rolling over for a couple of feet...",    
                                                                    "Until he landed in the backyard of a mysterious house." };

    private void Awake()
    {
        storyImage.sprite = storyImages[storyImageOrder];
        continueButton.SetActive(false);
        continueText.SetActive(false);
        StartCoroutine(Type());
        storyImageOrder = 0;
    }

    public void NextSlide()
    {
        continueButton.SetActive(false);
        continueText.SetActive(false);
        storyImageOrder++;
        if(storyImageOrder == 4)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            storyImage.sprite = storyImages[storyImageOrder];
            StartCoroutine(Type());
        }
    }

    IEnumerator Type()
    {
        text.text = " ";
        foreach(char letter in textStory[storyImageOrder].ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.SetActive(true);
        continueText.SetActive(true);
    }
}

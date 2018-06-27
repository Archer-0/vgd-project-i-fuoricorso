using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {

    public int firstImportantLines = 0;
    public float firstImportantLinesTimer = 0;
    public int lastImportantLines = 0;
    public float lastImportantLinesTimer = 0;

    public float interpolationTime = 5;

    public Text text;
    public CanvasGroup alphaText;

    [TextArea(2, 5)]
    public string[] lines;

    private AudioSource source;

    public AudioClip[] clips;

    bool canEnd = false;

	void Start () {
        StartCoroutine(ShowCredits());
        source = GetComponent<AudioSource>();

        StatsAndOther stats = GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>();

        source.clip = stats.hiddenLevelReached ? clips[0] : clips[1];

        source.Play();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}

    IEnumerator ShowCredits() {
        int i = 0;

        // scrive le prime linee imoprtanti
        for (i = 0; i < firstImportantLines; i++) {
            while (alphaText.alpha > 0) {
                alphaText.alpha -= interpolationTime * Time.deltaTime;
                yield return null;
            }

            text.text = lines[i];

            while (alphaText.alpha < 1) {
                alphaText.alpha += interpolationTime * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(firstImportantLinesTimer);
        }

        text.text = "";

        // quelle non importanti
        for (; i < (lines.Length - lastImportantLines); i++) {
            while (alphaText.alpha > 0) {
                alphaText.alpha -= interpolationTime * Time.deltaTime;
                yield return null;
            }

            text.text = lines[i];

            while (alphaText.alpha < 1) {
                alphaText.alpha += interpolationTime * Time.deltaTime;
                yield return null;
            }

            //yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(GetLineReadTime(lines[i]));
        }

        text.text = "";

        Debug.Log("i: " + i);

        // scrive le ultime importanti
        for (; i < lines.Length; i++) {
            while (alphaText.alpha > 0) {
                alphaText.alpha -= interpolationTime * Time.deltaTime;
                yield return null;
            }

            text.text = lines[i];


            while (alphaText.alpha < 1) {
                alphaText.alpha += interpolationTime * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(lastImportantLinesTimer);
        }

        canEnd = true;
    }
    
    float GetLineReadTime(string line) {
        char[] delimiters = new char[] { ' ', '\r', ',', '.'};

        float constant = 1 / 1.5F;

        int nWords = line.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries).Length;

        Debug.Log(nWords);

        return (nWords * constant) + 1;

    }


    private void FixedUpdate() {
        if (canEnd) {
            text.fontSize = 20;
            text.alignment = TextAnchor.LowerCenter;
            text.text = "Press any key to return to main menu or let the song flow";
            alphaText.alpha = Mathf.PingPong(Time.time, 1);

            if (Input.anyKeyDown) {
                BackToMenu();
            }

            if (!source.isPlaying) {
                BackToMenu();
            }

        }
    }

    public void BackToMenu() {
        LoadLevel.LoadScene(0);
    }
}

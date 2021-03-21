using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public int health;//health of player
    public int ammo;//current ammo
    public int ammoCapacity;//max clip of ammo
    public int score;//current score

    private Text ammoText;
    private Text heartText;
    private Text scoreText;

    void Start()
    {
        ammoText = transform.Find("AmmoText").GetComponent<Text>();
        heartText = transform.Find("HeartText").GetComponent<Text>();
        scoreText = transform.Find("ScoreText").GetComponent<Text>();

        score = GameObject.FindWithTag("Player").GetComponent<Player>().score;
        health = GameObject.FindWithTag("Player").GetComponent<Player>().health;
        ammo = GameObject.FindWithTag("ActiveGun").GetComponent<Pistol>().clip;
        ammoCapacity = GameObject.FindWithTag("ActiveGun").GetComponent<Pistol>().maxClip;

    }

    // Update is called once per frame
    void Update()
    {
        //update UI
        ammoText.text = ammo + "/" + ammoCapacity;
        heartText.text = "x" + health;
        scoreText.text = "score: " + score;
    }
}

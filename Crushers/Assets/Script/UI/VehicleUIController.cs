using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class VehicleUIController : MonoBehaviour
{
   

    [SerializeField] private TextMeshProUGUI levelTimerTxt;
    public GameObject startTimer;

    [SerializeField] private TextMeshProUGUI startTimerTxt;
    [SerializeField] private UnityEngine.UI.Slider abilityResetSlider;

    [SerializeField] private AbilityManager abilityManager;

    private Coroutine cooldownRoutine;

    void OnEnable()
    {
        LevelManager.levelTimeChanged +=  UpdateLevelTimer;
        LevelManager.startTimeChanged +=  UpdateStartTimer;

        AbilityManager.OnAbilityUse.AddListener(StartCooldownUI);
    }

    void OnDisable()
    {
        LevelManager.levelTimeChanged -=  UpdateLevelTimer;
        LevelManager.startTimeChanged -=  UpdateStartTimer;

        AbilityManager.OnAbilityUse.RemoveListener(StartCooldownUI);
    }

    private void Start()
    {
        abilityManager = gameObject.GetComponentInParent<AbilityManager>();
        //abilityResetSlider.maxValue = abilityManager.GetAbilityCooldownTime();
        abilityResetSlider.value = abilityResetSlider.maxValue;
    }

    private void StartCooldownUI(float cooldownTime)
    {
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }
        cooldownRoutine = StartCoroutine(CooldownUI(cooldownTime));
    }

    private IEnumerator CooldownUI(float cooldownTime)
    {
        float elapsedTime = 0f;
        abilityResetSlider.value = 0; 

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            abilityResetSlider.value = Mathf.Min(abilityResetSlider.maxValue, elapsedTime / cooldownTime * abilityResetSlider.maxValue);
            yield return null;
        }

        abilityResetSlider.value = abilityResetSlider.maxValue; 
    }

    public void UpdateLevelTimer(int time){
        if(time <= 10){
            StartCountdown(time);
            return; 
        }

        // convert time to minutes and seconds
        int minutes = time / 60;
        int seconds = time % 60;

        levelTimerTxt.SetText(string.Format("{0:0}:{1:00}", minutes, seconds));
    }

    public void UpdateStartTimer(int time){

        if(time == 0){
            // set text to "GO"
            startTimerTxt.SetText("GO!");
            startTimerTxt.color = Color.green;
            StartCoroutine(ClearStartTimer());
        } else {
            startTimerTxt.SetText(time.ToString());
        }
    }

    private void StartCountdown(int time){
        levelTimerTxt.gameObject.SetActive(false);
        startTimerTxt.color = Color.red;
        startTimerTxt.SetText(time.ToString());
        startTimer.SetActive(true);
    }

    private IEnumerator ClearStartTimer(){
        yield return new WaitForSeconds(1);
        startTimer.SetActive(false);

    }


}

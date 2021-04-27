using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingletonClass<AudioManager>
{


    #region Event References
    // These first three lines usually go inside the class definition—the same place where variables are declared
    //[FMODUnity.EventRef]
    public string backgroundTrack = "event:/Music/M - Carnial Riddumb (On Timer)";   // string reference to the FMOD-authored Event named "Intro"; name will appear in the Unity Inspector
    public FMOD.Studio.EventInstance backgroundTrackEv;      // Unity EventInstance name for Intro event that was created in FMOD

    //public string splashMusic = "event:/Music/Seperate Event Music/M - Comic Strip (Seperate)";
    //public FMOD.Studio.EventInstance splashMusicEv;

    public string shopTrack = "event:/Music/M - Shop Music";
    public FMOD.Studio.EventInstance shopTrackEv; 
    //Player SFX

    //[FMODUnity.EventRef]
    public string playerDeath = "event:/Player/P - Death";
    public FMOD.Studio.EventInstance playerDeathEv;

    //public string playerFootSteps = "event:/FX/Player - Footsteps";
    //public FMOD.Studio.EventInstance footStepsEv; 

    //[FMODUnity.EventRef]
    public string playerDamaged = "event:/Player/P - Damaged";
    public FMOD.Studio.EventInstance playerDamagedEv;

    //[FMODUnity.EventRef]
    public string playerAttack = "event:/Player/P - Attack";
    public FMOD.Studio.EventInstance playerAttackEv;


    public string playerCheer = "event:/Player/P - Woohoo";
    public FMOD.Studio.EventInstance playerCheerEv;

    //Weapons & Ability SFX
    //[FMODUnity.EventRef]
    public string hammerHit = "event:/Weapon Sounds/Hammer Sound/HH - Swing";
    public FMOD.Studio.EventInstance hammerHitEv;

    //[FMODUnity.EventRef]
    public string clownPop = "event:/Weapon Sounds/Clown Gun/CG - Shoot";
    public FMOD.Studio.EventInstance clownPopEv;
    //TODO fix event 
    //[FMODUnity.EventRef]
    public string babyRattleCountdown = "event:/Family Actions/FA - Baby Rattle";
    public FMOD.Studio.EventInstance babyRattleCountdownEv;

    public string babyRattle = "event:/Family Actions/Family Reset Timers/FR - Baby Rattle";
    public FMOD.Studio.EventInstance babyRattleEv; 

    //[FMODUnity.EventRef]
    public string dadJoke = "event:/Family Actions/FA - Dad Joke";
    public FMOD.Studio.EventInstance dadJokeEv;
    //TODO fix event 
    //[FMODUnity.EventRef]
    public string mothersGaze = "event:/Family Actions/FA - other's Gaze-r";
    public FMOD.Studio.EventInstance mothersGazeEv;

    //[FMODUnity.EventRef]
    public string dadJokeTimer = "event:/Family Actions/Family Reset Timers/FR - Dad Joke";
    public FMOD.Studio.EventInstance dadJokeTimeEv;

    //[FMODUnity.EventRef]
    public string mothersGazeTimer = "event:/Family Actions/Family Reset Timers/FR - Mother's Gazer";
    public FMOD.Studio.EventInstance motherGazeTimerEv;

    public string beetleBomb = "event:/Weapon Sounds/Beetle Bag/BB - Explosion";
    public FMOD.Studio.EventInstance beetleBombEv; 

    public string motherMumble = "event:/Family Actions/Family Mumbles/FM - Mum";
    public FMOD.Studio.EventInstance motherMumbleEv;

    public string sisterMumble = "event:/Family Actions/Family Mumbles/FM - Sister";
    public FMOD.Studio.EventInstance sisterMumbleEv; 



    //Enemy SFX 
    //[FMODUnity.EventRef]
    public string enemyAttck = "event:/Enemy Sounds/Enemy Attack/EA - Duck";
    public FMOD.Studio.EventInstance enemyAttackEv;

    //[FMODUnity.EventRef]
    public string enemyDeath = "event:/Enemy Sounds/Enemy Deaths/ED - Duck";
    public FMOD.Studio.EventInstance enemyDeathkEv;

    public string duckQuack = "event:/Enemy Sounds/Enemy Attack/EA - Duck";
    public FMOD.Studio.EventInstance duckQuackEv;
    //[FMODUnity.EventRef]
    public string elephantCharge = "event:/Enemy Sounds/Enemy Attack/EA - Elephant";
    public FMOD.Studio.EventInstance elephantChargeEv;

    //[FMODUnity.EventRef]
    public string elephantDeath = "event:/Enemy Sounds/Enemy Deaths/ED - Elephant";
    public FMOD.Studio.EventInstance elephantDeathEV;

    public string CannonFire = "event:/Enemy Sounds/EG - Mime Cannon";
    public FMOD.Studio.EventInstance cannonFireEv;

    public string mimeThud = "event:/Enemy Sounds/EG - Mime Thud";
    public FMOD.Studio.EventInstance mimeThudEv;

    public string mimeDeath = "event:/Enemy Sounds/Enemy Deaths/ED - Mime";
    public FMOD.Studio.EventInstance mimeDeathEv;

    public string mimeShoot = "event:/Enemy Sounds/Enemy Attack/EA - Mime";
    public FMOD.Studio.EventInstance mimeShootEv; 



    //Environment SFX
    //[FMODUnity.EventRef]
    public string corkPop = "event:/FX/FX - Barrel Pop";
    public FMOD.Studio.EventInstance corkPopEv;

    //[FMODUnity.EventRef]
    public string doorOpen = "event:/FX/FX - Door Open";
    public FMOD.Studio.EventInstance doorOpenEv;

    //[FMODUnity.EventRef]
    public string springs = "event:/Player/P - Springs";
    public FMOD.Studio.EventInstance springsEv;

    //[FMODUnity.EventRef]
    public string barrelExplode = "event:/FX/FX - Barrel Explode (Big)";
    public FMOD.Studio.EventInstance barrelExplodeEv;

    public string barrelShatter = "event:/FX/FX - Barrel Explode (General)";
    public FMOD.Studio.EventInstance barrelShatterEv;


    //[FMODUnity.EventRef]
    public string cheering = "event:/FX/FX - Cheering";
    public FMOD.Studio.EventInstance cheeringEv;

    //Pick Ups
    //[FMODUnity.EventRef]
    public string bubbleShield = "event:/Weapon Sounds/Bubble Shield/BS - Shield Equip";
    public FMOD.Studio.EventInstance bubbleShieldEv;

    //[FMODUnity.EventRef]
    public string bubblePop = "event:/Weapon Sounds/Bubble Shield/BS - Bubble Pop";
    public FMOD.Studio.EventInstance bubblePopEv;

    public string dragonCannonShoot = "event:/Weapon Sounds/Dragon Gun/DG - Shoot";
    public FMOD.Studio.EventInstance dragonShootEv;

    public string dragonCannonBullet = "event:/Weapon Sounds/Dragon Gun/DG - Impact";
    public FMOD.Studio.EventInstance dragonCannonBulletEv; 

    //Timer SFX
    public string countDownBuildUp = "event:/Misc/Game Timer/Timer - Build Up";
    public FMOD.Studio.EventInstance countDownBuildUpEv;

    public string singleSiren = "event:/Misc/Game Timer/Timer - Alarm Single";
    public FMOD.Studio.EventInstance singleSirenEv;

    public string finalSiren = "event:/Misc/Game Timer/Timer - Alarm Final";
    public FMOD.Studio.EventInstance finalSirenEv;

    //UI SFX
    public string purchaseDenied = "event:/Shop/Purchase Denied";
    public FMOD.Studio.EventInstance purchaseDeniedEv;

    public string purchaseSucces = "event:/Shop/Purchase Made";
    public FMOD.Studio.EventInstance purchaseSuccessEv;

    public string swapWeapon = "event:/Misc/UI - Feedback/Cards/Card Cycle";
    public FMOD.Studio.EventInstance swapWeaponEv;

    

    #endregion



    public override void Awake()
    {

        isPersistent = true;

        base.Awake();

        //These lines should be pasted into the section where you want to cue / play the sound
        // EventInstance is linked to the Event 


        ////player sfx
        playerDeathEv = FMODUnity.RuntimeManager.CreateInstance(playerDeath); //in its spot in the code 
        playerDamagedEv = FMODUnity.RuntimeManager.CreateInstance(playerDamaged); //in its spot in the code 
        playerAttackEv = FMODUnity.RuntimeManager.CreateInstance(playerAttack); //in its spot in the code 
        playerCheerEv = FMODUnity.RuntimeManager.CreateInstance(playerCheer);
       // footStepsEv = FMODUnity.RuntimeManager.CreateInstance(playerFootSteps); 

        ////weapons & ability sfx 
        corkPopEv = FMODUnity.RuntimeManager.CreateInstance(corkPop); //in its spot in the code 
        clownPopEv = FMODUnity.RuntimeManager.CreateInstance(clownPop); //in its spot in the code 
        hammerHitEv = FMODUnity.RuntimeManager.CreateInstance(hammerHit); //in its spot in the code 
        babyRattleEv = FMODUnity.RuntimeManager.CreateInstance(babyRattle); // in its spot in the code
        babyRattleCountdownEv = FMODUnity.RuntimeManager.CreateInstance(babyRattleCountdown); //in its spot in the code 
        mothersGazeEv = FMODUnity.RuntimeManager.CreateInstance(mothersGaze); //in its spot in the code 
        motherGazeTimerEv = FMODUnity.RuntimeManager.CreateInstance(mothersGazeTimer); //in its spot in the code 
        dadJokeEv = FMODUnity.RuntimeManager.CreateInstance(dadJoke); //in its spot in the code 
        dadJokeTimeEv = FMODUnity.RuntimeManager.CreateInstance(dadJokeTimer); //in its spot in the code 
        dragonShootEv = FMODUnity.RuntimeManager.CreateInstance(dragonCannonShoot); //in its spot in the code
        dragonCannonBulletEv = FMODUnity.RuntimeManager.CreateInstance(dragonCannonBullet);
        //beetleBombEv = FMODUnity.RuntimeManager.CreateInstance(beetleBomb); //in its spot in the code
        motherMumbleEv = FMODUnity.RuntimeManager.CreateInstance(motherMumble);
        sisterMumbleEv = FMODUnity.RuntimeManager.CreateInstance(sisterMumble);



        ////enemy sfx
        enemyDeathkEv = FMODUnity.RuntimeManager.CreateInstance(enemyDeath); //in its spot in the code 
        enemyAttackEv = FMODUnity.RuntimeManager.CreateInstance(enemyAttck); //in its spot in the code 
        elephantChargeEv = FMODUnity.RuntimeManager.CreateInstance(elephantCharge); //in its spot in the code 
        elephantDeathEV = FMODUnity.RuntimeManager.CreateInstance(elephantDeath); //in its spot in the code 
        mimeShootEv = FMODUnity.RuntimeManager.CreateInstance(mimeShoot);
        cannonFireEv = FMODUnity.RuntimeManager.CreateInstance(CannonFire);
        mimeThudEv = FMODUnity.RuntimeManager.CreateInstance(mimeThud);
        mimeDeathEv = FMODUnity.RuntimeManager.CreateInstance(mimeDeath);
        duckQuackEv = FMODUnity.RuntimeManager.CreateInstance(duckQuack);

  

        ////environment sfx
        springsEv = FMODUnity.RuntimeManager.CreateInstance(springs); //in its spot in the code 
        doorOpenEv = FMODUnity.RuntimeManager.CreateInstance(doorOpen); //in its spot in the code 
        backgroundTrackEv = FMODUnity.RuntimeManager.CreateInstance(backgroundTrack); //in its spot in the code 
        barrelExplodeEv = FMODUnity.RuntimeManager.CreateInstance(barrelExplode); //in its spot in the code 
        cheeringEv = FMODUnity.RuntimeManager.CreateInstance(cheering); //in its spot in the code 
        shopTrackEv = FMODUnity.RuntimeManager.CreateInstance(shopTrack);
        barrelShatterEv = FMODUnity.RuntimeManager.CreateInstance(barrelShatter);
        //splashMusicEv = FMODUnity.RuntimeManager.CreateInstance(splashMusic); 




        ////Pick ups sfx
        bubblePopEv = FMODUnity.RuntimeManager.CreateInstance(bubblePop); //in its spot in the code 
        bubbleShieldEv = FMODUnity.RuntimeManager.CreateInstance(bubbleShield); //in its spot in the code 

        //Timer SFX
        countDownBuildUpEv = FMODUnity.RuntimeManager.CreateInstance(countDownBuildUp);//in its spot in the code
        singleSirenEv = FMODUnity.RuntimeManager.CreateInstance(singleSiren); // in its spot in the code
        finalSirenEv = FMODUnity.RuntimeManager.CreateInstance(finalSiren); // in its spot in the code

        //UI SFX
        purchaseDeniedEv = FMODUnity.RuntimeManager.CreateInstance(purchaseDenied); //in its spot in the code
        purchaseSuccessEv = FMODUnity.RuntimeManager.CreateInstance(purchaseSucces); // in its spot in the code
        swapWeaponEv = FMODUnity.RuntimeManager.CreateInstance(swapWeapon);




        //backgroundTrackEv.getParameter("Level Restart", out lvlRestart);

        //backgroundTrackEv.getParameter("Level End", out endRoundParam);
        //backgroundTrackEv.getParameter("Level Start", out levelStartParam);
        //backgroundTrackEv.getParameter("Splash Screen Loop", out splashStartParam);
        //backgroundTrackEv.getParameter("Pause Screen ON-OFF", out pauseParam);

        //shopTrackEv.getParameter("Pause Screen ON-OFF", out shopPauseParam);


    }


    public void UpdatePitch(float _val)
    {

        backgroundTrackEv.setPitch(_val);

    }


}

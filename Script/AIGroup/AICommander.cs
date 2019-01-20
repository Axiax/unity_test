using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.ActionControl;
using Assets.Script.Avater;
using Assets.Script.weapon;
using UnityEngine;

namespace Assets.Script.AIGroup
{
    class AICommander:MonoBehaviour
    {
        private GameObject[] allMyAi;
        private GameObject target;

        private ActionStatusDictionary actionStatusDictionary = new ActionStatusDictionary();
        private ActionBasicBuilder actionConstructer = new ActionBasicBuilder();
        private AIConstructer aiConstructer = new AIConstructer();
        private WeaponFactory weaponFactory = new WeaponFactory();

        void Awake()
        {
            //初期化
            allMyAi = GameObject.FindGameObjectsWithTag("AI");
            //target = GameObject.Find("Player");
            weaponFactory.Init();
            foreach (GameObject ai in allMyAi)
            {
                var aimain = ai.gameObject.GetComponent<AINodeBase>();
                /*
                aimain.actionStatusDictionary.Init("Shotgunguy");
                //初始化，這裡還會更改
                aimain.ActionBasic = actionConstructer.GetActionBaseByName("Shotgunguy");
                aimain.ActionBasic.Init(ai,target);
                */
                aimain.AiBase = aiConstructer.GetAI();
                aimain.AiBase.Init(ai, target);

                aimain.Init_Avater();
                /*
                var a = weaponFactory.AllWeaponDictionary;
                ai.GetComponent<Gun>().AddWeapon(a["basicgun"]);
                ai.GetComponent<Gun>().AddWeapon(a["katana"]);
                ai.GetComponent<Gun>().AddWeapon(a["bazooka"]);
                */
                /*
                aimain.meleerange = 3;
                aimain.shootrange = 7;
                */
            }
        }

        void Start()
        {
            //是否有巡邏路線
        }

        void Update()
        {
            //調整時間後再重新思考
            //StartCoroutine(ChangeTactic(2f));
        }

        IEnumerator ChangeTactic(float waittime)
        {
            
            foreach (var gameObject in allMyAi)
            {
                //下達移動方針
                gameObject.SendMessage("GetCommand", "WakeUp!");
            }
            yield return new WaitForSeconds(waittime);
        }
    }
}

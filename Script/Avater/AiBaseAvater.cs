using System;
using Assets.Script.ActionControl;
using Assets.Script.AIGroup;
using Assets.Script.Test;
using Assets.Script.weapon;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.Avater
{
    class AiBaseAvater:AvaterMain
    {
        /*
        public ActionStatusDictionary actionStatusDictionary = new ActionStatusDictionary();
        public ActionBasic ActionBasic;

        public Animator Animator;
        */
        public ActionStatus NowStatus;
        public AIBase AiBase;
        public GameObject target;

        public string UpperCommand;
        public string NowCommand;
        public bool IsAwake = true;
        public bool canDecidedNextAction = true;
        public float meleerange;
        public float shootrange;
        public int smallhit;
        public int bighit;


        void Start()
        {
            actionBasic.ChangeTarget(GameObject.Find("Player"));
            //gameObject.GetComponent<Gun>().ChangeWeapon("bazooka");

            AiBase.target = GameObject.Find("Player");
            animator = this.gameObject.GetComponent<Animator>();
            NowStatus = actionStatusDictionary.MoveDictionary.WeightedRandom(x => (int)Convert.ToDouble(x.Value.Chance)).Value;
        }

        void OnCollisionEnter(Collision collision)
        {
        }

        void Update()
        {


            //異常狀態
            //分成有效和沒效，由主檔回報，
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("OnHit"))
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }
            
            //正常運轉中
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(NowStatus.ActionName) && canDecidedNextAction && !animator.IsInTransition(0)||!IsEndNormal)
            {
                NowCommand = AiBase.DistanceBasicAI(meleerange, shootrange);
                NowStatus = FindNextMoveInDictionary(NowCommand);
                animator.SetTrigger(NowStatus.ActionName);
                canDecidedNextAction = false;
                //得到掩體的位置，並且開始計時該動作的時間
                actionBasic.SetupBeforeAction();
            }
            else
            {
                canDecidedNextAction = true;
            }
            IsEndNormal = actionBasic.CustomAction(NowStatus);
        }

        public ActionStatus FindNextMoveInDictionary(string Command)
        {
            ActionStatus nowstatus;
            //抽選動作種類中的動作
            switch (Command)
            {
                default:
                    nowstatus = actionStatusDictionary.AllActionStatusDictionary[Command];
                    break;
                case "move":
                    nowstatus = actionStatusDictionary.MoveDictionary.WeightedRandom(x => (int)Convert.ToDouble(x.Value.Chance)).Value;
                    break;
                case "close":
                    nowstatus = actionStatusDictionary.CloseRangeDictionary.WeightedRandom(x => (int)Convert.ToDouble(x.Value.Chance)).Value;
                    break;
                case "long":
                    nowstatus = actionStatusDictionary.LongRangeDictionary.WeightedRandom(x => (int)Convert.ToDouble(x.Value.Chance)).Value;
                    break;
            }
            return nowstatus;
        }

        public void GetCommand(string msg)
        {
            //接收AI指揮官的命令
            UpperCommand = msg;
            Debug.Log(msg + " OK! I Woke!");
        }
    }
}

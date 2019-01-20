using Assets.Script.ActionControl;
using Assets.Script.weapon;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Script.Avater
{
    class PlayerAvater : AvaterMain
    {
        private float alarm;
        public Dictionary<int,string> PlayerWeaponDictionary;
        void Start()
        {
            //暫時，初始化到時會交出去
            Init_Avater();
            GetAnimaterParameter();

            actionBasic.ChangeTarget(GameObject.Find("CommandCube").transform.Find("Imp").gameObject);
            WeaponFactory weaponFactory = new WeaponFactory();
            weaponFactory.Init();
            var GunDic = weaponFactory.AllWeaponDictionary;

            gameObject.GetComponent<Gun>().AddWeapon(GunDic["basicgun"]);
            gameObject.GetComponent<Gun>().AddWeapon(GunDic["MG"]);
            gameObject.GetComponent<Gun>().AddWeapon(GunDic["bazooka"]);
            gameObject.GetComponent<Gun>().AddWeapon(GunDic["katana"]);
            gameObject.GetComponent<Gun>().CreateWeaponByList();
            //gameObject.GetComponent<Gun>().ChangeWeapon(PlayerWeaponDictionary[0]);
            //gameObject.GetComponent<Gun>().ChangeWeapon("MG");
        }
        void Update()
        {
            foreach (var actionStatuse in actionStatusDictionary.AllActionStatusDictionary)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag(actionStatuse.Key))
                {
                    NowActionStatus = actionStatuse.Value;
                }
            }
            if (OldActionStatus != null && OldActionStatus != NowActionStatus)
            {
                //動作變了
                actionBasic.BeforeCustomAction(NowActionStatus);
                actionBasic.SetupBeforeAction();
                RefreshAnimaterParameter();
                if (NowActionStatus.ignorelist != null)
                {
                    foreach (var cando in NowActionStatus.ignorelist)
                    {
                        animator.SetBool("avater_can_" + cando, false);
                    }
                }
            }

            OldActionStatus = NowActionStatus;
            IsEndNormal = actionBasic.CustomAction(NowActionStatus);
            animator.SetBool("avater_IsEndNormal", IsEndNormal);

            if (GetComponent<Rigidbody>().velocity.y != 0)
            {
                animator.SetFloat("avater_yspeed", GetComponent<Rigidbody>().velocity.y);
            }

            /*
            if (!animator.IsInTransition(0))
            {
                foreach (var actionStatuse in actionStatusDictionary.AllActionStatusDictionary)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsTag(actionStatuse.Key))
                    {
                        actionBasic.CustomAction(actionStatuse.Value);
                        NowActionStatus = actionStatuse.Value;
                    }
                }
            }
            */
        }

        void OnCollisionEnter(Collision collision)
        {   
            if (!animator.GetBool("avater_can_jump") && collision.gameObject.layer == 1)
            {
                animator.SetBool("avater_can_jump",true);                
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<NavMeshAgent>().enabled = true;                
            }            
        }
    }
}

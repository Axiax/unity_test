using System;
using Assets.Script.ActionControl;
using Assets.Script.Avater;
using Assets.Script.Config;
using Assets.Script.weapon;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.ActionList
{
    class PlayerAction : ActionBasic
    {
        public bool ChangeWeapon(ActionStatus actionStatus)
        {
            //input 得到現在按鈕的參照
            //將玩家武器Dictionary內部的索引值對應input的參照
            return true;
        }

        public bool Before_shoot(ActionStatus actionStatus)
        {
            gun.ChangeWeapon("MG");
            return true;
        }

        public bool shoot(ActionStatus actionStatus)
        {
            RotateTowardSlerp(target.transform.position);
            gun.fire();
            return true;
        }

        public bool Before_slash1(ActionStatus actionStatus)
        {
            gun.ChangeWeapon("katana");
            return true;
        }

        public bool slash1(ActionStatus actionStatus)
        {
            if (actionElapsedTime > actionStatus.Time1)
            {
                if (doOnlyOnce)
                {
                    gun.StartSlash(actionStatus.Time2);
                    doOnlyOnce = false;
                }
            }
            return true;
        }

        public bool Before_heavyslash(ActionStatus actionStatus)
        {
            gun.NowWeapon.charge = animator.GetFloat("charge");
            return true;
        }

        public bool heavyslash(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                Debug.Log("your power is "+gun.NowWeapon.charge);    
            }
            return true;
        }

        public bool idle(ActionStatus actionStatus)
        {
            me.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //me.GetComponent<Rigidbody>().velocity = Vector3.Lerp(me.GetComponent<Rigidbody>().velocity,Vector3.zero,0);
            //myAgent.velocity = Vector3.Lerp(myAgent.velocity, Vector3.zero, 1);
            return true;
        }
        public bool Before_move(ActionStatus actionStatus)
        {
            return true;
        }
        public bool move(ActionStatus actionStatus)
        {
            if (Input.anyKey)
            {
                RotateTowardlerp(me.transform.position + Vector3.forward * input.ws + Vector3.right * input.ad);
                myAgent.velocity = me.transform.TransformDirection(Vector3.forward).normalized * 5f;
            }
            //me.GetComponent<Rigidbody>().velocity = ;
            //me.GetComponent<NavMeshAgent>().Move(me.transform.TransformDirection(Vector3.forward).normalized* 0.1f);

            gun.fire();
            return true;
        }

        public bool jump(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                NowVecter = myAgent.velocity;
                myAgent.enabled = false;
                me.GetComponent<Rigidbody>().isKinematic = false;
                //me.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f);
                me.GetComponent<Rigidbody>().velocity = NowVecter+Vector3.up * 20f;                
                doOnlyOnce = false;
            }
            if (Input.anyKey)
            {
                RotateTowardlerp(me.transform.position + Vector3.forward * input.ws + Vector3.right * input.ad);
                me.GetComponent<Rigidbody>().AddForce(Vector3.right * input.ad);
            }

            return true;
        }

        public bool falling(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                myAgent.enabled = true;
                doOnlyOnce = false;
            }

            if (Input.anyKey)
            {
                RotateTowardlerp(me.transform.position + Vector3.forward * input.ws + Vector3.right * input.ad);
                me.GetComponent<Rigidbody>().AddForce(Vector3.right * input.ad);
            }
            return true;
        }

        public bool katana(ActionStatus actionStatus)
        {
            var katana = me.GetComponent<Gun>();
            katana.Bullet.GetComponent<Collider>().enabled = true;
            return true;
        }
        public bool stun()
        {
            return true;
        }
    }
}

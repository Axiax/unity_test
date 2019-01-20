using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.ActionControl;
using Assets.Script.Avater;
using Assets.Script.weapon;
using UnityEngine;

namespace Assets.Script.ActionList
{

    class ShotgunguyAction : ActionBasic
    {
        public bool OnHit()
        {
            return true;
        }

        public bool idle(ActionStatus actionStatus)
        {
            RotateTowardlerp(target.transform);
            return true;
        }

        public bool charge(ActionStatus actionStatus)
        {
            gun.ChangeWeapon("katana");

            if (doOnlyOnce)
            {
                myAgent.ResetPath();
                myAgent.SetDestination(target.transform.position);
                myAgent.autoBraking = false;
                doOnlyOnce = false;
            }


            return true;
        }

        public bool reload(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                myAgent.ResetPath();
                doOnlyOnce = false;
            }

            if (actionStartTime + actionStatus.Time1 < Time.time)
            {
                gun.reload();
            }
            return true;
        }

        public bool shoot(ActionStatus actionStatus)
        {
            gun.ChangeWeapon("basicgun");
            RotateTowardlerp(target.transform);
            if (doOnlyOnce)
            {
                myAgent.ResetPath();
                //myAgent.SetDestination(targetPos);
                doOnlyOnce = false;
            }           
            if (actionStartTime + actionStatus.Time1 < Time.time)
            {
                if (gun.NowWeapon.BulletInMag < 1)
                {
                    return false;
                }

                if (Vector3.Angle(me.transform.forward,target.transform.position-me.transform.position) < 5)
                    gun.fire();
            }
            
            return true;
        }

        public bool movingshoot(ActionStatus actionStatus)
        {
            gun.ChangeWeapon("bazooka");
            RotateTowardlerp(target.transform);
            if (doOnlyOnce)
            {
                myAgent.ResetPath();
                myAgent.SetDestination(targetPos);
                doOnlyOnce = false;
            }           
            if (actionStartTime+actionStatus.Time1 < Time.time)
            {
                if (gun.NowWeapon.BulletInMag < 1)
                {
                    return false;
                }

                if (Vector3.Angle(me.transform.forward, target.transform.position - me.transform.position) < 5)
                    gun.fire();
            }
            
            return true;
        }
        public bool runaway(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                var loc = targetPos - (targetPos - me.transform.position) * 2;
                myAgent.ResetPath();
                myAgent.SetDestination(loc);
                doOnlyOnce = false;
            }
            return true;
        }
    }
    class TofuAction : ActionBasic
    {
        public bool bounce(ActionStatus actionStatus)
        {
            myAgent.SetDestination(EarlycoverPos.position);
            Debug.Log("巡邏中");
            return true;
        }
        public bool hover(ActionStatus actionStatus)
        {
            myAgent.SetDestination(EarlycoverPos.position);
            Debug.Log("滑行?!");
            return true;
        }
        public bool fist(ActionStatus actionStatus)
        {
            myAgent.SetDestination(EarlycoverPos.position);
            Debug.Log("豆腐在 " + Time.time + " 時休息" + " 而長度是 " + actionStatus.Time1);
            return true;
        }
        public bool kick(ActionStatus actionStatus)
        {
            myAgent.SetDestination(EarlycoverPos.position);
            Debug.Log("豆腐lady!?!? " + Time.time);
            //將現在的動能加總為準度下降值
            //檢查準度下降值是否超過底線
            return true;
        }
        public bool laser(ActionStatus actionStatus)
        {
            myAgent.SetDestination(targetPos);
            Debug.Log("豆腐在 " + Time.time + " 射出了雷射?!" + " 而長度是 " + actionStatus.Time1);
            return true;
        }
        public bool gun(ActionStatus actionStatus)
        {
            myAgent.SetDestination(targetPos);
            Debug.Log("豆腐ready!?!? " + Time.time);
            //將現在的動能加總為準度下降值
            //檢查準度下降值是否超過底線
            return true;
        }
    }
}

using Assets.Script.ActionControl;
using Assets.Script.Avater;
using UnityEngine;

namespace Assets.Script.ActionList
{
    class ImpAction: ActionBasic
    {
        public bool idle(ActionStatus actionStatus)
        {
            RotateTowardSlerp(targetPos);
            return true;
        }

        public bool Before_walk(ActionStatus actionStatus)
        {
            myAgent.SetDestination(target.transform.position);
            return true;
        }

        public bool walk(ActionStatus actionStatus)
        {
            return true;
        }

        public bool Before_fireball(ActionStatus actionStatus)
        {
            myAgent.ResetPath();
            gun.ChangeWeapon("MG");
            return true;
        }

        public bool fireball(ActionStatus actionStatus)
        {
            /*
            if (doOnlyOnce)
            {
                //myAgent.SetDestination(target.transform.position);
                myAgent.ResetPath();
                gun.ChangeWeapon("MG");
                doOnlyOnce = false;
            }
            */
            if (gun.NowWeapon.BulletInMag > 0)
            {
                if (Vector3.Angle(me.transform.TransformDirection(Vector3.forward),
                        target.transform.position - me.transform.position) < 5)
                {
                    gun.fire();
                }
            }
            else
            {
                return false;
            }
            RotateTowardlerp(target.transform);
            return true;
        }

        public bool reload(ActionStatus actionStatus)
        {
            if (actionElapsedTime > actionStatus.Time1)
            {
                if (doOnlyOnce)
                {
                    gun.reload();
                    doOnlyOnce = false;
                }
            }
            return true;
        }

        public bool jumpslash(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                myAgent.SetDestination(target.transform.position);
                gun.ChangeWeapon("katana");
                gun.NowWeapon.BulletInMag = 1;
                doOnlyOnce = false;
            }
            if (actionElapsedTime > actionStatus.Time1)
            {
                gun.StartSlash(actionStatus.Time2);
            }
            //RotateTowardlerp(target.transform);
            return true;
        }
    }
}

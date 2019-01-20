using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Script.AIGroup;
using Assets.Script.Avater;
using Assets.Script.Config;
using Assets.Script.weapon;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.ActionControl
{
    class ActionBasicBuilder
    {
        public ActionBasic GetActionBaseByName(string ActionName)
        {
            Type t = Type.GetType("Assets.Script.ActionList." + ActionName+"Action");
            ActionBasic actionBase = new ActionBasic();
            actionBase = (ActionBasic)Activator.CreateInstance(t);
            return actionBase;
        }
    }
    /// <summary>
    /// 這裡負責關於人物的面向，移動，動作
    /// </summary>
    public class ActionBasic
    {
        public Animator animator;

        public GameObject me;
        public GameObject target;

        protected NavMeshAgent myAgent;
        protected NavMeshAgent targetAgent;
        protected NavMeshHit coverPos;
        protected NavMeshHit EarlycoverPos;
        protected Vector3 targetPos;
        protected Vector3 NowVecter;

        protected Vector3 CamFront;
        protected InputManager input;

        protected Camera camera;

        protected Gun gun;

        protected float actionStartTime;
        protected float actionElapsedTime;

        public bool doOnlyOnce;

        public void Init(GameObject me)
        {
            this.me = me;
            this.myAgent = this.me.GetComponent<NavMeshAgent>();
            this.gun = me.GetComponent<Gun>();
            this.animator = me.GetComponent<Animator>();

            if (me.GetComponent<InputManager>())
            {
                input = me.GetComponent<InputManager>();
            }

            if(me.transform.Find("Camera"))
            {
                camera = me.transform.Find("Camera").GetComponent<Camera>();
                CamFront = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane));
            }

            actionElapsedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        }

        public bool BeforeCustomAction(ActionStatus actionStatus)
        {
            Type actionbaseType = GetType();
            MethodInfo methodInfo = actionbaseType.GetMethod("Before_"+actionStatus.ActionName);
            if (methodInfo != null)
                return (bool)methodInfo.Invoke(this, new object[] { actionStatus }/*放入actionStatus*/);
            return false;
        }

        public bool CustomAction(ActionStatus actionStatus)
        {
            Type actionbaseType = GetType();
            MethodInfo methodInfo = actionbaseType.GetMethod(actionStatus.ActionName);
            return (bool)methodInfo.Invoke(this, new object[] { actionStatus }/*放入actionStatus*/);
        }

        public bool AfterCustomAction(ActionStatus actionStatus)
        {
            Type actionbaseType = GetType();
            MethodInfo methodInfo = actionbaseType.GetMethod("After_"+actionStatus.ActionName);
            if (methodInfo != null)
                return (bool)methodInfo.Invoke(this, new object[] { actionStatus }/*放入actionStatus*/);
            return false;
        }

        public bool CustomAction(string actionName)
        {
            //var actionStatus = FindByNameDic(action, type);
            Type actionbaseType = GetType();
            MethodInfo methodInfo = actionbaseType.GetMethod(actionName);
            return (bool)methodInfo.Invoke(this, null/*放入actionStatus*/);
        }

        public void SetupBeforeAction()
        {
            //targetAgent.FindClosestEdge(out coverPos);
            doOnlyOnce = true;
            targetPos = target.transform.position;
            actionStartTime = Time.time;
        }

        protected void RotateTowardSlerp(Transform target)
        {
            Vector3 direction = (target.position - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Slerp(me.transform.rotation, lookRotation, Time.deltaTime * 1.7f/*rotationSpeed*/);
        }

        protected void RotateTowardSlerp(Vector3 target)
        {
            Vector3 direction = (target - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Slerp(me.transform.rotation, lookRotation, Time.deltaTime * 1.7f/*rotationSpeed*/);
        }

        protected void RotateTowardSlerp(Vector3 target,float speed)
        {
            Vector3 direction = (target - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Slerp(me.transform.rotation, lookRotation, Time.deltaTime * speed/*rotationSpeed*/);
        }

        protected void RotateTowardlerp(Transform target)
        {
            Vector3 direction = (target.position - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, Time.deltaTime * 1.7f/*rotationSpeed*/);
        }

        protected void RotateTowardlerp(Vector3 target)
        {
            Vector3 direction = (target - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, Time.deltaTime * 3f/*rotationSpeed*/);
        }

        protected void RotateTowardlerp(Vector3 target, float speed)
        {
            Vector3 direction = (target - me.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
            me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, Time.deltaTime * speed/*rotationSpeed*/);
        }

        public void ChangeTarget(GameObject targetGameObject)
        {
            this.target = targetGameObject;
        }

        public virtual bool stun(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                myAgent.enabled = false;
                doOnlyOnce = false;
                gun.NowWeapon.weapon.GetComponent<Collider>().enabled = false;
            }
            return true;
        }
        public virtual bool recover(ActionStatus actionStatus)
        {
            if (doOnlyOnce)
            {
                myAgent.enabled = true;
                doOnlyOnce = false;
            }
            return true;
        }
    }


}

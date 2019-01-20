using Assets.Script.weapon;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Script.AIGroup
{
    class AIConstructer
    {
        public AIBase GetAI()
        {
            return new ZombieAI();
        }
    }

    class AIBase
    {
        public GameObject me;
        public GameObject target;

        public void Init(GameObject me, GameObject target)
        {
            this.me = me;
            this.target = target;
        }
        /// <summary>
        /// 以距離為主的AI，返回簡單的策略，如果返回並非遠中近就會執行指定字串
        /// </summary>
        /// <param name="meleerange">肉搏最遠距離</param>
        /// <returns></returns>
        public string DistanceBasicAI(float meleerange,float shootrange)
        {
            RaycastHit hit;
            float distance = GetDistanceVector3();
            if (distance < meleerange)
            {
                return "close";
            }
            //如果在距離內，又看的到目標，衝刺攻擊也寫在這。
            if (Physics.Raycast(me.transform.position, target.transform.position-me.transform.position,out hit, shootrange))
            {
                //如果沒有彈藥就優先換彈
                
                if (me.GetComponent<Gun>().NowWeapon.BulletInMag <= 0)
                {
                    return "reload";
                }
                
                if (hit.rigidbody != null)
                {
                    return "long";
                }
            }
            //雖說是move卻指的是在射擊距離外
            return "move";
        }
        //在得到和目標的距離
        public float GetDistanceVector3()
        {
            return Vector3.Distance(me.transform.position, target.transform.position);
        }
    }

    class ZombieAI:AIBase
    {
    }
}

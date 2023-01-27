using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CountDownManager : MonoBehaviour
    {
        [SerializeField] protected int countDownTimeInSecond = 1;
        protected List<CountDown> countDowns = new List<CountDown>();
        protected float timer = 0;

        void Awake()
        {
            timer = countDownTimeInSecond;
        }

        public void AddCountDown(CountDown c)
        {
            countDowns.Add(c);
        }

        public void CheckIsActive()
        {
            countDowns.RemoveAll(item => item.MyComponent.CheckIsActive() == false);
        }

        public void DoCountDown()
        {
            foreach (CountDown c in countDowns) c.DoCountdown();
        }

        void FixedUpdate()
        {
            if (countDowns.Count == 0) return;
            CheckIsActive();
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = countDownTimeInSecond;
                DoCountDown();
            }
        }
    }
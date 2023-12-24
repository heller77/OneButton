using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Enemys.Boids
{
    public class BoidsManager : MonoBehaviour
    {
        private struct Boid : IJob
        {
            public NativeArray<float> numbersToAdd;
            public NativeArray<float> result;

            public Boid(NativeArray<float> numbersToAdd, float result)
            {
                this.numbersToAdd = numbersToAdd;
                this.result = new NativeArray<float>(1, Allocator.TempJob);
            }

            public void Execute()
            {
                float sumvalue = 0.0f;
                for (int i = 0; i < numbersToAdd.Length; i++)
                {
                    sumvalue += numbersToAdd[i];
                }
                numbersToAdd[0] = sumvalue;
                result[0] = sumvalue;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                execute();
            }
        }

        private void execute()
        {
            int length = 1000;
            NativeArray<float> numbers = new NativeArray<float>(length, Allocator.TempJob);
            ArrayList temparray = new ArrayList();
            for (int i = 0; i < length; i++)
            {
                temparray.Add(i);
            }

            for (int i = 0; i < length; i++)
            {
                numbers[i] = temparray.IndexOf(i);
            }

            Boid boid = new Boid(numbers, 0.0f);
            JobHandle handle = boid.Schedule();
            handle.Complete();

            //処理結果（）
            Debug.Log(boid.numbersToAdd[0]); //55
            Debug.Log(boid.result[0]); //0(処理結果入ってなさそう)

            numbers.Dispose();
        }

    }
}
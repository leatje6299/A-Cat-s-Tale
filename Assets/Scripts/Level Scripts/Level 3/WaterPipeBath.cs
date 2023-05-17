using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipeBath : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private float delay = 5f;
    [SerializeField] private float reset = 10f;
    [SerializeField] private List<Transform> pipes;

    private Dictionary<ParticleSystem, float> particleCreationTimes = new Dictionary<ParticleSystem, float>();
    private List<ParticleSystem> particlesToRemove = new List<ParticleSystem>();


    private void Start()
    {
        StartCoroutine(SpawnWaterPipes(delay, reset));
    }

    private void Update()
    {
        foreach (var kvp in particleCreationTimes)
        {
            ParticleSystem particle = kvp.Key;
            float creationTime = kvp.Value;
            if (Time.time - creationTime > (reset + delay * 4))
            {
                particlesToRemove.Add(particle);
            }
        }

        for(int i = 0; i < particlesToRemove.Count; i++)
        {
            ParticleSystem particle = particlesToRemove[i];
            particleCreationTimes.Remove(particle);
            if (particle != null)
            {
                particleCreationTimes.Remove(particle);
                Destroy(particle.gameObject);
            }
        }

    }

    private IEnumerator SpawnWaterPipes(float delayTime, float resetTime)
    {
        var delay = new WaitForSeconds(delayTime);
        var reset = new WaitForSeconds(resetTime);

        for (int i = 0; i < pipes.Count; i++)
        {
            Transform pipe = pipes[i];
            int randomSide = Random.Range(0, 2);
            Transform faucet = pipe.GetChild(randomSide);
            Vector3 faucetPos = faucet.GetChild(0).position;
            Quaternion faucetRot = faucet.GetChild(0).rotation;
            ParticleSystem particle = Instantiate(particlePrefab, faucetPos, faucetRot, faucet);
            particleCreationTimes[particle] = Time.time;
            yield return delay;
        }
        yield return reset;
        StartCoroutine(SpawnWaterPipes(delayTime, resetTime));
    }
}

using System;
using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace KubernetesPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Kubernetes(KubernetesClientConfiguration.InClusterConfig());

            var pods = client.ListNamespacedPod("default");

            foreach (var p in pods.Items)
                Console.WriteLine($"Pod = {p.Metadata.Name}");

            //Create Pod
            var pod = new V1Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new V1ObjectMeta
                {
                    Name = "my-test-pod"
                },
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>()
                }
            };

            pod.Spec.Containers.Add(new V1Container
            {
                Name = "my-test-container",
                 Image = "ngnix",
                 
            });


            //Create pod
            var result = client.CreateNamespacedPod(pod, "default");

            foreach (var status in result.Status.ContainerStatuses)
                Console.WriteLine($"Image = {status.Image}, StartedAt = {status.State.Running.StartedAt}");


            pods = client.ListNamespacedPod("default");
            foreach (var p in pods.Items)
                Console.WriteLine($"Pod = {p.Metadata.Name}");

            Console.ReadLine();
        }
    }
}
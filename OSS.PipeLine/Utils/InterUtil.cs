﻿using System.Threading.Tasks;

namespace OSS.Pipeline.InterImpls
{
    internal static class InterUtil
    {
        //public static readonly Task<TrafficResult> GreenTrafficResultTask =Task.FromResult(TrafficResult.GreenResult);

        public static readonly Task<TrafficSignal> GreenTrafficSignalTask = Task.FromResult(TrafficSignal.GreenSignal);
    }
}

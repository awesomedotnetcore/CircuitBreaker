﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Steeltoe.CircuitBreaker.Hystrix.Util;
using System.Collections.Concurrent;

namespace Steeltoe.CircuitBreaker.Hystrix.CircuitBreaker
{
    public static class HystrixCircuitBreakerFactory
    {
        private static ConcurrentDictionary<string, IHystrixCircuitBreaker> circuitBreakersByCommand = new ConcurrentDictionary<string, IHystrixCircuitBreaker>();

        public static IHystrixCircuitBreaker GetInstance(IHystrixCommandKey key, IHystrixCommandGroupKey group, IHystrixCommandOptions options, HystrixCommandMetrics metrics)
        {
            return circuitBreakersByCommand.GetOrAddEx(key.Name, (k) => new HystrixCircuitBreakerImpl(key, group, options, metrics));
        }

        public static IHystrixCircuitBreaker GetInstance(IHystrixCommandKey key)
        {
            circuitBreakersByCommand.TryGetValue(key.Name, out IHystrixCircuitBreaker previouslyCached);
            return previouslyCached;
        }

        internal static void Reset()
        {
            circuitBreakersByCommand.Clear();
        }
    }
}

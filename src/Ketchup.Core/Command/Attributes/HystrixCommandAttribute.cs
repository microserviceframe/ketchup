﻿using System;
using System.Collections.Generic;
using System.Text;
using Ketchup.Core.Cache;

namespace Ketchup.Core.Command.Attributes
{
    public class HystrixCommandAttribute : Attribute
    {
        public string MethodName { get; set; }

        /// <summary>
        /// 执行超时时间
        /// </summary>
        public int ExcuteTimeoutInMilliseconds { get; set; } = 10000;

        /// <summary>
        /// 最大信号量
        /// </summary>
        public int MaxRequests { get; set; } = 0;

        /// <summary>
        /// 最大信号量的限定时间
        /// 默认1s
        /// </summary>
        public int MaxRequestsTime { get; set; } = 1000;

        /// <summary>
        /// 至少多少请求失败，熔断器才发挥起作用
        /// </summary>
        public int BreakerRequestCircuitBreaker { get; set; } = 10;

        /// <summary>
        /// 是否开启服务降级
        /// </summary>
        public bool EnableServiceDegradation { get; set; }

        /// <summary>
        /// 降级缓存类型
        /// </summary>
        public CacheModel Cache { get; set; } = CacheModel.Memory;

        /// <summary>
        /// 降级缓存时间
        /// 单位 秒
        /// </summary>
        public int ServiceDegradationTimeSpan { get; set; } = 10;
    }
}

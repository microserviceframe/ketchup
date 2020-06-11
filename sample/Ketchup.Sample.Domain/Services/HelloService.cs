﻿using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Domain;
using Ketchup.Core.Attributes;
using Ketchup.Core.Cache;
using Ketchup.Core.EventBus;
using Ketchup.Core.Kong.Attribute;
using Ketchup.Core.Utilities;
using Ketchup.Sample.Domain.Services.Events;
using Newtonsoft.Json;

namespace Ketchup.Sample.Domain.Services
{
    [Service(Name = "grpc.domain.RpcTest")]
    public class HelloService : RpcTest.RpcTestBase
    {
        private readonly ICacheProvider _cache;

        public HelloService()
        {
            _cache = ServiceLocator.GetService<ICacheProvider>(CacheModel.Redis.ToString());
        }

        //[HystrixCommand(MethodName = nameof(SayHello), ExcuteTimeoutInMilliseconds = 3000)]
        [KongRoute(Name = nameof(SayHello), Paths = new[] { "/sample/SayHello" })]
        public override async Task<HelloReponse> SayHello(HelloRequest request, ServerCallContext context)
        {

            //throw new Exception("报错了");
            //Thread.Sleep(1500);
            var result = await _cache.GetAsync<string>("a");

            return new HelloReponse()
            {
                Code = 1,
                Msg = "hello simple",
                Result = result
            };
        }

        //[HystrixCommand(MethodName = nameof(SayHelloEvent), Timeout = 2000)]
        public override Task<HelloReponse> SayHelloEvent(HelloRequest request, ServerCallContext context)
        {
            ServiceLocator.GetService<IEventBus>().Publish(new UserEvent()
            {
                Name = "simple",
                Job = "it"
            });
            return Task.FromResult(new HelloReponse());
        }
    }
}

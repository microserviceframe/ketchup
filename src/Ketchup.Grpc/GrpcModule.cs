﻿using System;
using Autofac;
using Ketchup.Core;
using Ketchup.Core.Configurations;
using Ketchup.Core.Modules;
using Ketchup.Grpc.Internal.Channel;
using Ketchup.Grpc.Internal.Channel.Implementation;
using Ketchup.Grpc.Internal.Client;
using Ketchup.Grpc.Internal.Client.Implementation;

namespace Ketchup.Grpc
{
    public class GrpcModule : KernelModule
    {
        public override void Initialize(KetchupPlatformContainer builder)
        {
        }

        protected override void RegisterModule(ContainerBuilderWrapper builder)
        {
            if (AppConfig.ServerOptions.EnableHttp)
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            builder.ContainerBuilder.RegisterType<DefaultGrpcClientProvider>().As<IGrpcClientProvider>()
                .SingleInstance();
            builder.ContainerBuilder.RegisterType<DefaultChannelPool>().As<IChannelPool>().SingleInstance();
        }
    }
}
﻿using Autofac;
using Ketchup.Core;
using Ketchup.Core.Modules;
using Ketchup.Profession.AutoMapper;
using Ketchup.Profession.AutoMapper.ObjectMapper;
using Ketchup.Profession.ORM.EntityFramworkCore.Repository;
using Ketchup.Profession.ORM.EntityFramworkCore.Repository.Implementation;

namespace Ketchup.Profession
{
    public class ProfessionModule : KernelModule
    {
        public override void Initialize(KetchupPlatformContainer builder)
        {
        }

        protected override void RegisterModule(ContainerBuilderWrapper builder)
        {
            builder.ContainerBuilder.RegisterGeneric(typeof(EfCoreRepository<,>)).As(typeof(IEfCoreRepository<,>)).InstancePerLifetimeScope();
            builder.ContainerBuilder.RegisterType<AutoMapperObjectMapper>().As<IObjectMapper>().SingleInstance();
        }
    }
}

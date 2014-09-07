using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Redis;

namespace RedisBugs
{
    [TestFixture]
    public class RedisAutoWireProof
    {
        private ServiceHost _host;

        [TestFixtureSetUp] public void FixtureSetup()
        {
            _host = new ServiceHost();
        }

        [TestFixtureTearDown] public void FixtureTeardown()
        {
            _host.Dispose();
        }

        [Test]
        public void ShouldBeAbleToAutoWireIRedisClient()
        {
            _host.Init();
            var redis = _host.Resolve<IRedisClient>();
            Assert.IsNotNull(redis);
        }
    }

    public class TestService : Service
    { }

    public class ServiceHost : AppHostBase
    {
        public ServiceHost() : base("Test Service", typeof(TestService).Assembly)
        { }

        public override void Configure(Container container)
        {
            container.RegisterAutoWiredAs<RedisClient, IRedisClient>();
        }
    }
}

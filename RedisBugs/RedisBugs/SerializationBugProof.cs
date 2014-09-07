using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ServiceStack.Redis;

namespace RedisBugs
{
    [TestFixture]
    public class SerializationBugProof
    {
        [Test]
        public static void ShouldSerializeDateEntitiesInListsWithTimeZoneOffset()
        {
            var date = DateTime.Now;
            using (var redis = new RedisClient())
            {
                redis.FlushDb();

                var list = redis.As<DateTime>().Lists["listId"];
                list.Add(date);             // silently drops timezone offset (0400)
                var dateFromList = list[0]; // fetch the incorrectly stored date
                Assert.AreEqual(date.ToLocalTime(), dateFromList.ToLocalTime());

            }
        }
    }
}

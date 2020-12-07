using System;
using FluentAssertions;
using NUnit.Framework;
using StudyHttpClient;

namespace Tests
{
    public class KeyValueRepositoryTest
    {
        private IKeyValueRepository keyValueRepository;

        [SetUp]
        public void Setup()
        {
            keyValueRepository = new KeyValueRepository();
        }
        [Test]
        public void TestPing()
        {
            var client = new KeyValueRepository();
            Assert.AreEqual(true, client.Ping());
        }

        [Test]
        public void TestFindByRandomKey()
        {
            var randomKey = Guid.NewGuid().ToString();

            var actual = keyValueRepository.Find(randomKey);
            actual.Should().BeNull();
        }

        [Test]
        public void TestUpdateByRandomKey()
        {
            var randomKey = Guid.NewGuid().ToString();

            var exception = Assert.Catch<KeyValueRepositoryException>(() => keyValueRepository.Update(randomKey, "value"));
            
            exception.Should().NotBeNull();
        }

        [Test]
        public void TestFindBySpecialKey()
        {
            var key = Guid.NewGuid().ToString();
            var expectedValue = DateTime.UtcNow.ToString();

            var keyValue = new KeyValue
            {
                Key = key,
                Value = expectedValue
            };

            keyValueRepository.Create(keyValue);

            var actual = keyValueRepository.Find(key);
            actual.Should().BeEquivalentTo(keyValue);
        }

        [Test]
        public void TestFindByUpdateSpecialKey()
        {
            var key = Guid.NewGuid().ToString();
            var expectedValue = DateTime.UtcNow.ToString();

            var keyValue = new KeyValue
            {
                Key = key,
                Value = expectedValue
            };

            keyValueRepository.Create(keyValue);

            var actual = keyValueRepository.Find(key);
            actual.Should().BeEquivalentTo(keyValue);

            var updatedValue = Guid.NewGuid().ToString();

            keyValueRepository.Update(key, updatedValue);

            actual = keyValueRepository.Find(key);
            actual.Should().BeEquivalentTo(new KeyValue {Key = key, Value = updatedValue});
        }



    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnglishTutor.Common.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace EnglishTutor.Services.Tests
{
    [TestClass]
    public class FirebaseServiceTest
    {
        private IFirebaseService _service;

        [TestInitialize]
        public void Init()
        {
            //_service = new FirebaseService();
        }

        [TestMethod]
        public async Task GetWordShouldSucceed()
        {
            var words = await _service.GetWordsAsync("fuss", "glorious");

            Assert.IsNotNull(words);
        }

        [TestMethod]
        public async Task GetStatisticsShouldSucceed()
        {
            const int expectedLen = 3;
            var words = await _service.GetStatisticAsync("115787596179138188666", expectedLen);

            Assert.IsNotNull(words);
            Assert.AreEqual(expectedLen, words.Count());
        }
    }
}

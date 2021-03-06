﻿using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PoeApiClient.Models;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
using POEToolsTestsBase;
using System.Text;

namespace PoeRankingTrackerTests.Services
{
    [TestClass]
    public class CharacterServiceTest : BaseUnitTest
    {
        private ILadder ladder;
        private Ladder ladder96;
        private ICharacterService characterService;

        [TestInitialize]
        public void TestSetup()
        {
            using (IWindsorContainer container = new WindsorContainer())
            {
                container.Install(new ServicesInstaller());
                characterService = container.Resolve<ICharacterService>();
                string ladderJson = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Ladder);
                ladder = JsonConvert.DeserializeObject<Ladder>(ladderJson, GetJsonSettings());
                string ladder96Json = Encoding.UTF8.GetString(POEToolsTestsBase.Properties.Resources.Ladder96);
                ladder96 = JsonConvert.DeserializeObject<Ladder>(ladder96Json, GetJsonSettings());
            }
        }

        [TestMethod]
        public void GetRank()
        {
            IEntry entry1 = ladder.Entries[0];
            IEntry entry2 = ladder.Entries[3];
            IEntry entry3 = ladder.Entries[5];
            IEntry entry4 = ladder.Entries[7];
            IEntry entry5 = ladder.Entries[9];
            IEntry entry6 = ladder.Entries[11];
            IEntry entry7 = ladder.Entries[14];
            IEntry entry8 = ladder.Entries[16];
            IEntry entry9 = ladder.Entries[19];

            int rank1 = characterService.GetRank(ladder, entry1.Character.Name);
            int rank2 = characterService.GetRank(ladder, entry2.Character.Name);
            int rank3 = characterService.GetRank(ladder, entry3.Character.Name);
            int rank4 = characterService.GetRank(ladder, entry4.Character.Name);
            int rank5 = characterService.GetRank(ladder, entry5.Character.Name);
            int rank6 = characterService.GetRank(ladder, entry6.Character.Name);
            int rank7 = characterService.GetRank(ladder, entry7.Character.Name);
            int rank8 = characterService.GetRank(ladder, entry8.Character.Name);
            int rank9 = characterService.GetRank(ladder, entry9.Character.Name);

            Assert.AreEqual(entry1.Rank, rank1);
            Assert.AreEqual(entry2.Rank, rank2);
            Assert.AreEqual(entry3.Rank, rank3);
            Assert.AreEqual(entry4.Rank, rank4);
            Assert.AreEqual(entry5.Rank, rank5);
            Assert.AreEqual(entry6.Rank, rank6);
            Assert.AreEqual(entry7.Rank, rank7);
            Assert.AreEqual(entry8.Rank, rank8);
            Assert.AreEqual(entry9.Rank, rank9);
        }

        [TestMethod]
        public void GetRankByClass()
        {
            IEntry entry1 = ladder.Entries[0];
            IEntry entry2 = ladder.Entries[3];
            IEntry entry3 = ladder.Entries[5];
            IEntry entry4 = ladder.Entries[7];
            IEntry entry5 = ladder.Entries[9];
            IEntry entry6 = ladder.Entries[11];
            IEntry entry7 = ladder.Entries[14];
            IEntry entry8 = ladder.Entries[16];
            IEntry entry9 = ladder.Entries[19];
            
            int rank1 = characterService.GetRankByClass(ladder.Entries, entry1);
            int rank2 = characterService.GetRankByClass(ladder.Entries, entry2);
            int rank3 = characterService.GetRankByClass(ladder.Entries, entry3);
            int rank4 = characterService.GetRankByClass(ladder.Entries, entry4);
            int rank5 = characterService.GetRankByClass(ladder.Entries, entry5);
            int rank6 = characterService.GetRankByClass(ladder.Entries, entry6);
            int rank7 = characterService.GetRankByClass(ladder.Entries, entry7);
            int rank8 = characterService.GetRankByClass(ladder.Entries, entry8);
            int rank9 = characterService.GetRankByClass(ladder.Entries, entry9);

            Assert.AreEqual(1, rank1);
            Assert.AreEqual(1, rank2);
            Assert.AreEqual(1, rank3);
            Assert.AreEqual(2, rank4);
            Assert.AreEqual(1, rank5);
            Assert.AreEqual(3, rank6);
            Assert.AreEqual(2, rank7);
            Assert.AreEqual(1, rank8);
            Assert.AreEqual(1, rank9);
        }

        [TestMethod]
        public void GetNumbersOfDeadsAhead()
        {
            IEntry entry1 = ladder.Entries[0];
            IEntry entry2 = ladder.Entries[3];
            IEntry entry3 = ladder.Entries[5];
            IEntry entry4 = ladder.Entries[7];
            IEntry entry5 = ladder.Entries[9];
            IEntry entry6 = ladder.Entries[11];
            IEntry entry7 = ladder.Entries[14];
            IEntry entry8 = ladder.Entries[16];
            IEntry entry9 = ladder.Entries[19];

            int n1 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry1);
            int n2 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry2);
            int n3 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry3);
            int n4 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry4);
            int n5 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry5);
            int n6 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry6);
            int n7 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry7);
            int n8 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry8);
            int n9 = characterService.GetNumbersOfDeadsAhead(ladder.Entries, entry9);

            Assert.AreEqual(0, n1);
            Assert.AreEqual(0, n2);
            Assert.AreEqual(1, n3);
            Assert.AreEqual(1, n4);
            Assert.AreEqual(1, n5);
            Assert.AreEqual(1, n6);
            Assert.AreEqual(2, n7);
            Assert.AreEqual(2, n8);
            Assert.AreEqual(3, n9);
        }

        [TestMethod]
        public void IsEntryInvalid()
        {
            IEntry entry1 = ladder.Entries[0];
            IEntry entry2 = ladder.Entries[3];
            IEntry entry3 = ladder.Entries[5];
            IEntry entry4 = ladder.Entries[7];
            IEntry entry5 = ladder.Entries[9];
            IEntry entry6 = ladder.Entries[11];
            IEntry entry7 = ladder.Entries[14];
            IEntry entry8 = ladder.Entries[16];
            IEntry entry9 = ladder.Entries[19];

            bool b1 = characterService.IsEntryInvalid(entry1);
            bool b2 = characterService.IsEntryInvalid(entry2);
            bool b3 = characterService.IsEntryInvalid(entry3);
            bool b4 = characterService.IsEntryInvalid(entry4);
            bool b5 = characterService.IsEntryInvalid(entry5);
            bool b6 = characterService.IsEntryInvalid(entry6);
            bool b7 = characterService.IsEntryInvalid(entry7);
            bool b8 = characterService.IsEntryInvalid(entry8);
            bool b9 = characterService.IsEntryInvalid(entry9);

            Assert.IsFalse(b1);
            Assert.IsTrue(b2);
            Assert.IsFalse(b3);
            Assert.IsFalse(b4);
            Assert.IsFalse(b5);
            Assert.IsTrue(b6);
            Assert.IsFalse(b7);
            Assert.IsFalse(b8);
            Assert.IsFalse(b9);
        }

        [TestMethod]
        public void GetExperienceDifference()
        {
            IEntry entry1 = ladder96.Entries[0];
            IEntry entry2 = ladder96.Entries[3];
            IEntry entry3 = ladder96.Entries[5];
            IEntry entry4 = ladder96.Entries[7];
            IEntry entry5 = ladder96.Entries[9];
            IEntry entry6 = ladder96.Entries[11];
            IEntry entry7 = ladder96.Entries[14];
            IEntry entry8 = ladder96.Entries[16];
            IEntry entry9 = ladder96.Entries[19];

            long n1 = entry1.Character.Experience - entry2.Character.Experience;
            long n2 = entry2.Character.Experience - entry3.Character.Experience;
            long n3 = entry3.Character.Experience - entry4.Character.Experience;
            long n4 = entry4.Character.Experience - entry5.Character.Experience;
            long n5 = entry5.Character.Experience - entry6.Character.Experience;
            long n6 = entry6.Character.Experience - entry7.Character.Experience;
            long n7 = entry7.Character.Experience - entry8.Character.Experience;
            long n8 = entry8.Character.Experience - entry9.Character.Experience;
            long n9 = entry1.Character.Experience - entry9.Character.Experience;

            Assert.AreEqual(181216, n1);
            Assert.AreEqual(263218, n2);
            Assert.AreEqual(179917, n3);
            Assert.AreEqual(267850, n4);
            Assert.AreEqual(292640, n5);
            Assert.AreEqual(666277, n6);
            Assert.AreEqual(146918, n7);
            Assert.AreEqual(90240, n8);
            Assert.AreEqual(2088276, n9);
        }

        [TestMethod]
        public void GetExperienceAhead()
        {
            IEntry entry1 = ladder96.Entries[0];
            IEntry entry2 = ladder96.Entries[3];
            IEntry entry3 = ladder96.Entries[5];
            IEntry entry4 = ladder96.Entries[7];
            IEntry entry5 = ladder96.Entries[9];
            IEntry entry6 = ladder96.Entries[11];
            IEntry entry7 = ladder96.Entries[14];
            IEntry entry8 = ladder96.Entries[16];
            IEntry entry9 = ladder96.Entries[19];

            long n1 = characterService.GetExperienceAhead(ladder96.Entries, entry1);
            long n2 = characterService.GetExperienceAhead(ladder96.Entries, entry2);
            long n3 = characterService.GetExperienceAhead(ladder96.Entries, entry3);
            long n4 = characterService.GetExperienceAhead(ladder96.Entries, entry4);
            long n5 = characterService.GetExperienceAhead(ladder96.Entries, entry5);
            long n6 = characterService.GetExperienceAhead(ladder96.Entries, entry6);
            long n7 = characterService.GetExperienceAhead(ladder96.Entries, entry7);
            long n8 = characterService.GetExperienceAhead(ladder96.Entries, entry8);
            long n9 = characterService.GetExperienceAhead(ladder96.Entries, entry9);

            Assert.AreEqual(0, n1);
            Assert.AreEqual(44130, n2);
            Assert.AreEqual(90433, n3);
            Assert.AreEqual(85437, n4);
            Assert.AreEqual(187833, n5);
            Assert.AreEqual(92180, n6);
            Assert.AreEqual(395300, n7);
            Assert.AreEqual(111808, n8);
            Assert.AreEqual(27268, n9);
        }

        [TestMethod]
        public void GetExperienceBehind()
        {
            IEntry entry1 = ladder96.Entries[0];
            IEntry entry2 = ladder96.Entries[3];
            IEntry entry3 = ladder96.Entries[5];
            IEntry entry4 = ladder96.Entries[7];
            IEntry entry5 = ladder96.Entries[9];
            IEntry entry6 = ladder96.Entries[11];
            IEntry entry7 = ladder96.Entries[14];
            IEntry entry8 = ladder96.Entries[16];
            IEntry entry9 = ladder96.Entries[19];

            long n1 = characterService.GetExperienceBehind(ladder96.Entries, entry1);
            long n2 = characterService.GetExperienceBehind(ladder96.Entries, entry2);
            long n3 = characterService.GetExperienceBehind(ladder96.Entries, entry3);
            long n4 = characterService.GetExperienceBehind(ladder96.Entries, entry4);
            long n5 = characterService.GetExperienceBehind(ladder96.Entries, entry5);
            long n6 = characterService.GetExperienceBehind(ladder96.Entries, entry6);
            long n7 = characterService.GetExperienceBehind(ladder96.Entries, entry7);
            long n8 = characterService.GetExperienceBehind(ladder96.Entries, entry8);
            long n9 = characterService.GetExperienceBehind(ladder96.Entries, entry9);

            Assert.AreEqual(0, n1);
            Assert.AreEqual(172785, n2);
            Assert.AreEqual(94480, n3);
            Assert.AreEqual(80017, n4);
            Assert.AreEqual(200460, n5);
            Assert.AreEqual(248658, n6);
            Assert.AreEqual(35110, n7);
            Assert.AreEqual(10226, n8);
            Assert.AreEqual(0, n9);
        }
    }
}

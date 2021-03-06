﻿using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoeRankingTracker;
using PoeRankingTracker.Installers;
using PoeRankingTracker.Services;
using POEToolsTestsBase;
using System;
using System.Linq;

namespace PoeRankingTrackerTests.Installers
{
    [TestClass]
    public class ServicesInstallerTest : BaseUnitTest, IDisposable
    {
        private IWindsorContainer container;

        [TestInitialize]
        public void Setup()
        {
#pragma warning disable CA2000
            container = new WindsorContainer()
                .Install(new ServicesInstaller());
#pragma warning restore CA2000
        }

        [TestMethod]
        public void AllServicesImplementsIService()
        {
            var all = GetAllHandlers(container);
            var handlers = GetHandlers();

            Assert.AreNotEqual(0, all.Length);
            CollectionAssert.AreEquivalent(all, handlers);
        }
        
        [TestMethod]
        public void AllServicesAreRegistered()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(
                c => c.Is<ICharacterService>() ||
                     c.Is<IFormatterService>() ||
                     c.Is<IFormService>() ||
                     c.Is<IHtmlService>()
            );
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesHaveServicesSuffix()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Name.EndsWith("Service", StringComparison.InvariantCulture));
            var registered = GetImplementationTypes();

            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllAndOnlyServicesLiveInServicesNamespace()
        {
            var all = GetPublicClassesFromApplicationAssembly<RankingTrackerContext>(c => c.Namespace.Contains("Services"));
            var registered = GetImplementationTypes();
            CollectionAssert.AreEquivalent(all, registered);
        }

        [TestMethod]
        public void AllServicesAreSingleton()
        {
            var nonSingletonServices = GetHandlers()
                .Where(service => service.ComponentModel.LifestyleType != LifestyleType.Singleton)
                .ToArray();

            Assert.AreEqual(0, nonSingletonServices.Length);
        }

        private IHandler[] GetHandlers()
        {
            var handlers = new IHandler[4];

            handlers[0] = GetHandlersFor(typeof(ICharacterService), container)[0];
            handlers[1] = GetHandlersFor(typeof(IFormatterService), container)[0];
            handlers[2] = GetHandlersFor(typeof(IFormService), container)[0];
            handlers[3] = GetHandlersFor(typeof(IHtmlService), container)[0];

            return handlers;
        }

        private Type[] GetImplementationTypes()
        {
            var registered = new Type[4];

            registered[0] = GetImplementationTypesFor(typeof(ICharacterService), container)[0];
            registered[1] = GetImplementationTypesFor(typeof(IFormatterService), container)[0];
            registered[2] = GetImplementationTypesFor(typeof(IFormService), container)[0];
            registered[3] = GetImplementationTypesFor(typeof(IHtmlService), container)[0];

            return registered;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                container?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

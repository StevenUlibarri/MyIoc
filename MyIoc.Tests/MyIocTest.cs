using NUnit.Framework;
using MyIoc.Tests.TestDataStructures;
using MyIoc.Core;

namespace MyIoc.Tests
{
    [TestFixture]
    public class MyIocTest
    {
        [Test]
        public void MyIocContainerShouldRegisterADependency()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>();
        }

        [Test]
        public void MyIocContainerShouldResolveADependency()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>();
            var worker = ioc.Resolve(typeof(ISimpleService));
            Assert.AreEqual(typeof(SimpleWorker), worker.GetType());
        }

        [Test]
        public void MyIocContainerShouldNotRegisterTheSameInterfaceTwice()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>();
            Assert.Throws<System.Exception>(() => ioc.Register<ISimpleService, SimpleWorker>());
        }

        [Test]
        public void MyIocContainerShouldReturnNewInstancesOfTransientTypes()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>();
            var worker0 = ioc.Resolve(typeof(ISimpleService));
            var worker1 = ioc.Resolve(typeof(ISimpleService));
            Assert.AreNotEqual(worker0, worker1);
        }

        [Test]
        public void MyIocContainerShouldReturnTheSameInstanceOfSingletonTypes()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>(LifeCycleTypes.SINGLETON);
            var worker0 = ioc.Resolve(typeof(ISimpleService));
            var worker1 = ioc.Resolve(typeof(ISimpleService));
            Assert.AreEqual(worker0, worker1);
        }

        [Test]
        public void MyIocContainerShouldResolveNestedDependencies()
        {
            MyIocContainer ioc = new MyIocContainer();
            ioc.Register<ISimpleService, SimpleWorker>();
            ioc.Register<IComplexService, ComplexWorker>();
            var complexWorker = ioc.Resolve(typeof(IComplexService));
            Assert.AreEqual(typeof(ComplexWorker), complexWorker.GetType());
            Assert.AreEqual(typeof(SimpleWorker), (complexWorker as ComplexWorker).NestedSimpleService.GetType());
        }

    }
}

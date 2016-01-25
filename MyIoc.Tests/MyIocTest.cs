using NUnit.Framework;
using MyIoc.Tests.TestDataStructures;

namespace MyIoc.Tests
{
    [TestFixture]
    public class MyIocTest
    {
        [Test]
        public void MyIocShouldRegisterADependency()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>();
        }

        [Test]
        public void MyIocShouldResolveADependency()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>();
            var worker = ioc.Resolve<ISimpleService>();
            Assert.AreEqual(typeof(SimpleWorker), worker.GetType());
        }

        [Test]
        public void MyIocShouldNotRegisterTheSameInterfaceTwice()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>();
            Assert.Throws<System.Exception>(() => ioc.Register<ISimpleService, SimpleWorker>());
        }

        [Test]
        public void MyIocShouldReturnNewInstancesOfTransientTypes()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>();
            var worker0 = ioc.Resolve<ISimpleService>();
            var worker1 = ioc.Resolve<ISimpleService>();
            Assert.AreNotEqual(worker0, worker1);
        }

        [Test]
        public void MyIocShouldReturnTheSameInstanceOfSingletonTypes()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>(LifeCycleTypes.SINGLETON);
            var worker0 = ioc.Resolve<ISimpleService>();
            var worker1 = ioc.Resolve<ISimpleService>();
            Assert.AreEqual(worker0, worker1);
        }

        [Test]
        public void MyIocShouldResolveTypesWithNestedDependencies()
        {
            MyIoc ioc = new MyIoc();
            ioc.Register<ISimpleService, SimpleWorker>();
            ioc.Register<IComplexService, ComplexWorker>();
            var complexWorker = ioc.Resolve<IComplexService>();
            Assert.AreEqual(typeof(ComplexWorker), complexWorker.GetType());
            Assert.AreEqual(typeof(SimpleWorker), (complexWorker as ComplexWorker).NestedSimpleService.GetType());
        }

    }
}

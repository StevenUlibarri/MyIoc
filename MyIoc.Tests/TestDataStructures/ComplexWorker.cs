using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoc.Tests.TestDataStructures
{
    internal class ComplexWorker : IComplexService
    {

        public ComplexWorker(ISimpleService service)
        {
            NestedSimpleService = service;
        }

        public ISimpleService NestedSimpleService { get; private set; }

        public void DoMoreComplexWork()
        {
            throw new NotImplementedException();
        }
    }
}

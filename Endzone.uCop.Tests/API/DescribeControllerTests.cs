using Endzone.UCop.API;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endzone.uCop.Tests.API
{
    [TestFixture]
    public class DescribeControllerTests
    {
        [Test]
        public void DocumentTypes_ShouldFetchPagedResults()
        {
            var controller = new DescribeController();

            var docTypes = controller.DocumentTypes();


        }
    }
}

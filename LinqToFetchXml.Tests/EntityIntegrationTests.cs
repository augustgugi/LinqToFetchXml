using FakeXrmEasy;
using LinqToFetchXml.Tests.Metadata;
using LinqToFetchXml.Tests.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using gugi.LinqToFetchXml;
using Microsoft.Xrm.Sdk.Query;

namespace LinqToFetchXml.Tests
{
    public class EntityIntegrationTests
    {
        
        private XrmFakedContext fakedContext;
        private IOrganizationService organizationService;
        private TestEntityContext testContext;

        public EntityIntegrationTests()
        {

            fakedContext = new XrmFakedContext();
            //fakedContext.SetEntityMetadata(BusinessUnitMetadata.GetMetadata());

            organizationService = fakedContext.GetFakedOrganizationService();
            testContext = new TestEntityContext(organizationService);

            Entity bu = new Entity("bu");
            bu["name"] = "GUGI";
            bu["users"] = 5;

            bu.Id = testContext.Create(bu);

            Entity buFilter = new Entity("bu");
            buFilter["name"] = "Filter";
            buFilter["users_filter"] = 5;

            buFilter.Id = testContext.Create(buFilter);

            Entity user = new Entity("systemuser");
            user["name"] = "August";
            user["bu_id"] = new EntityReference("bu", buFilter.Id);

        }

        [Fact]
        public void Retrieve_All_Attributes_By_Default()
        {
            var bus = testContext.BusinessUnits.ToList();

            Assert.NotEmpty(bus);
            Assert.Equal("GUGI", bus[0].GetAttributeValue<string>("name"));
            Assert.Equal(5, bus[0].GetAttributeValue<int>("users"));
        }

        [Fact]
        public void Retrieve_Specific_Attributes_By_Default()
        {
            var bus = testContext.BusinessUnits
                .SelectAttributes(bu => new FetchXmlAttributes(bu.GetAttributeValue<int>("users"))) //
                .ToList();

            Assert.NotEmpty(bus);
            Assert.Null(bus.First().GetAttributeValue<string>("name"));
            Assert.Equal(5, bus.First().GetAttributeValue<int>("users"));
        }

        [Fact]
        public void Count_Records()
        {
            var bus = testContext.BusinessUnits.Count();

            Assert.NotEqual(0, bus);
        }

        [Fact]
        public void Filter_Records_Equal()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<string>("name") == "Filter")
                .ToList();

            Assert.NotEmpty(bus);
            Assert.Single(bus);
        }

        [Fact]
        public void Filter_Records_Not_Equal()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<string>("name") != "Filter")
                .ToList();

            Assert.NotEmpty(bus);
            Assert.Equal(testContext.BusinessUnits.Count() -1, bus.Count);
        }

        [Fact]
        public void Filter_Records_Less_Than()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<int>("users_filter") < 5)
                .ToList();

            Assert.Empty(bus);
        }

        [Fact]
        public void Filter_Records_Less_Or_Equal_Than()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<int>("users_filter") <= 5)
                .ToList();

            Assert.Single(bus);
        }

        [Fact]
        public void Filter_Records_Greater_Than()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<int>("users_filter") > 5)
                .ToList();

            Assert.Empty(bus);
        }

        [Fact]
        public void Filter_Records_Greater_Or_Equal_Than()
        {
            var bus = testContext.BusinessUnits
                .Where(bu => bu.GetAttributeValue<int>("users_filter") >= 5)
                .ToList();

            Assert.Single(bus);
        }

        [Fact]
        public void Inner_Join()
        {
            var users = from u in testContext.Users
                        join bu in testContext.BusinessUnits
                        on u.GetAttributeValue<object>("bu_id") equals bu.GetAttributeValue<object>("buid")
                        select u;

            

            Assert.Single(users);
        }
    }
}

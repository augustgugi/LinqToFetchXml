using FakeXrmEasy;
using gugi.LinqToFetchXml;
using gugi.LinqToFetchXml.Extensions;
using gugi.LinqToFetchXml.FetchXml;
using LinqToFetchXml.Tests.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace LinqToFetchXml.Tests
{
    public class IntegrationTests
    {

        private XrmFakedContext fakedContext;
        private IOrganizationService organizationService;
        private TestContext testContext;


        BusinessUnit bu = null;
        Team t = null;
        User Agust = null;
        User Andrea = null;

        public IntegrationTests()
        {
            fakedContext = new XrmFakedContext();
            //fakedContext.ProxyTypesAssembly = Assembly.GetExecutingAssembly();

            organizationService = fakedContext.GetFakedOrganizationService();
            testContext = new TestContext(organizationService);

            bu = new BusinessUnit()
            {
                _name = "GG"
            };
            bu.buid = Create(bu);

            t = new Team()
            {
                _name = "GUGI"
            };
            t.teamid = Create(t);

            Agust = new User()
            {
                _name = "Agust",
                _bu = bu.Id,
                _teamid = t.Id
            };

            Agust.systemuserid = Create(Agust);

            Andrea = new User()
            {
                _name = "Andrea",
                _bu = bu.Id,
                _teamid = t.Id
            };

            Andrea.systemuserid = Create(Andrea);
        }

        [Fact]
        public void SelectTest()
        {
            var p = from k in testContext.Users
                    select k;

            var r = p.ToList();

            Assert.Equal(2, r.Count);
        }

        [Fact]
        public void SelectWhereTest()
        {
            var p = from u in testContext.Users
                    where u._name == "Agust"
                    select u;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void SelectWhereMultipleTest()
        {
            var p = from u in testContext.Users
                    where u._name == "Agust" || u._name == "David"
                    select u;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void SelectWhereMultiple2Test()
        {
            var p = from u in testContext.Users
                    where (u._name == "Agust" || u._name == "David") && u.systemuserid == Agust.systemuserid
                    select u;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void SelectWhere_Guid_Initialized_In_PredicateTest()
        {
            Guid gg = new Guid(Agust.systemuserid.ToString());
            var p = from u in testContext.Users
                    where u.systemuserid == new Guid(Agust.systemuserid.ToString())
                    select u;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void SelectWhere_Guid_Pre_Initialized_In_PreicateTest()
        {
            Guid gg = new Guid(Agust.systemuserid.ToString());

            var p = from u in testContext.Users
                    where u.systemuserid == gg
                    select u;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void SelectWhere_Extensions_PreicateTest()
        {

            var p = testContext.Users
                    .SetFilterType(FilterType.or)
                    .Where(u => u.systemuserid == new Guid(Agust.systemuserid.ToString()));

            var expr = p.Expression;

            var users = p.ToList();

            Assert.Single(users);
        }

        [Fact]
        public void Select_Order_Extensions_PredicateTest()
        {
            Guid gg = new Guid("2B8896AD-11B1-4FE6-879C-0B4FCFDC0540");

            var p = testContext.Users
                    .OrderByDescending(u => "name")
                    .ThenByDescending(u => u.systemuserid);

            var expr = p.Expression;

            var users = p.ToList();

            Assert.NotEmpty(users);
        }

        [Fact]
        public void Select_Take_Test()
        {
            var p = testContext.Users
                .Take(1);

            var expr = p.Expression;

            var users = p.ToList();

            Assert.NotEmpty(users);
        }

        [Fact]
        public void Select_Select_Test()
        {

            var p = testContext.Users
                .Take(1);

            var expr = p.Expression;

            var users = p.ToList();

            Assert.NotEmpty(users);
        }

        [Fact]
        public void Join_Linq_Test()
        {
            Guid gg = new Guid("2B8896AD-11B1-4FE6-879C-0B4FCFDC0540");

            var p = testContext.Users
                .Join(testContext.Teams, 
                u => u._teamid, 
                t => t.teamid, 
                (u, t) => u);

            var expr = p.Expression;

            var users = p.ToList();

            Assert.NotEmpty(users);
        }

        [Fact]
        public void Join_Test()
        {
            Guid gg = new Guid("2B8896AD-11B1-4FE6-879C-0B4FCFDC0540");

            var p = from u in testContext.Users
                    join t in testContext.Teams
                    on u._teamid equals t.teamid
                    select u;

            var expr = p.Expression;

            var users = p.ToList();

            Assert.NotEmpty(users);
        }


        private Guid Create(Entity record)
        {
            Entity ent = new Entity(record.LogicalName, record.Id);
            foreach(var attr in record.Attributes)
            {
                ent.Attributes[attr.Key] = attr.Value;
            }

            return organizationService.Create(ent);
        }
    }
}
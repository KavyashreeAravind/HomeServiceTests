using HomeService.Models;
using HomeService.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeServiceTests
{
    public class Tests
    {
        List<Audit> auditdetails = new List<Audit>();

        IQueryable<Audit> auditdetailsData;

        Mock<DbSet<Audit>> mockSet;

        Mock<AMSContext> auditManagementSystemContext;

        [SetUp]
        public void Setup()
        {
            auditdetails = new List<Audit>()
           {
               new Audit{Auditid=101,ProjectName="Face Detection",ProjectManagerName="Manali",ApplicationOwnerName="Python Labs",AuditType="Internal",AuditDate=DateTime.Now,ProjectExecutionStatus="Green",RemedialActionDuration="No action needed",Userid=1}
           };
            auditdetailsData = auditdetails.AsQueryable();

            mockSet = new Mock<DbSet<Audit>>();

            mockSet.As<IQueryable<Audit>>().Setup(m => m.Provider).Returns(auditdetailsData.Provider);
            mockSet.As<IQueryable<Audit>>().Setup(m => m.Expression).Returns(auditdetailsData.Expression);
            mockSet.As<IQueryable<Audit>>().Setup(m => m.ElementType).Returns(auditdetailsData.ElementType);
            mockSet.As<IQueryable<Audit>>().Setup(m => m.GetEnumerator()).Returns(auditdetailsData.GetEnumerator());

            var p = new DbContextOptions<AMSContext>();
            auditManagementSystemContext = new Mock<AMSContext>(p);
            auditManagementSystemContext.Setup(x => x.Audit).Returns(mockSet.Object);
        }

        [Test]
        public void GetAuditByUserIdPass()
        {
            var rmock = new HomeRepo(auditManagementSystemContext.Object);
            var data = rmock.AuditByUserId(1);
            Assert.IsNotNull(data);
        }

        [Test]
        public void GetAuditByUserIdFail()
        {
            var rmock = new HomeRepo(auditManagementSystemContext.Object);
            Assert.Throws<AggregateException>(() => rmock.AuditByUserId(50).Wait());
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Repositories;
using StructureMap;

namespace Oxagile.Internal.Api.Test
{
    [TestFixture]
    public class CompanyRepositoryTests
    {
        private ICompanyRepository repository;

        public CompanyRepositoryTests()
        {
            var container = new Container();
            container.Configure(config => 
            {
                config.For<ICompanyRepository>().Use<CompanyRepository>();
                
                var optionsBuilder = new DbContextOptionsBuilder<UserCompanyContext>();
                optionsBuilder.UseInMemoryDatabase("CompanyRepositoryTestDb");
                var context = new UserCompanyContext(optionsBuilder.Options);
                config.For<UserCompanyContext>().Use(context);
            });

            repository  = container.GetInstance<ICompanyRepository>();
        }

        [Test]
        public async Task Should_Get_Companies()
        {
            await Task.WhenAll(new [] 
            {
                repository.Create(new Company
                {
                    Name = "company1",
                }),
                repository.Create(new Company
                {
                    Name = "company2",
                }),
                repository.Create(new Company
                {
                    Name = "company3",
                })
            });
            var companies = await repository.Get();

            Assert.IsNotEmpty(companies);
        }

        [Test]
        public async Task Should_Create_Company()
        {
            var company = await repository.Create(new Company
            {
                Name = "company6",
            });

            Assert.NotNull(company);
            Assert.AreEqual("company6", company.Name);
        }

        [Test]
        public async Task Should_Get_Company()
        {
            var company = await repository.Create(new Company
            {
                Name = "test company",
            });

            var getCompanyResult = await repository.Get(company.Id);

            Assert.NotNull(getCompanyResult);
            Assert.AreEqual("test company", getCompanyResult.Name);
        }

        [Test]
        public async Task Should_Delete_Company()
        {
            var company = await repository.Create(new Company
            {
                Name = "test company for delete",
            });

            var createdCompanyId = company.Id;
            var getCompanyResult = await repository.Delete(company.Id);

            Assert.ThrowsAsync(typeof(ArgumentException)
                , async () => await repository.Get(createdCompanyId));
        }
    }
}

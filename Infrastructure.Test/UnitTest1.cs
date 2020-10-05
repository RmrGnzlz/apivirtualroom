using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Infrastructure.Test
{
    public class Tests
    {
        UnitOfWork unitOfWork;
        IDbContext context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SolumaticaContext>().UseNpgsql("Host=ec2-52-0-155-79.compute-1.amazonaws.com;Port=5432;Database=darscg1heeg51g;Username=mgkshjrkovllrc;Password=2ee5ea5cf4c5c50565a2f1815cd591f109cce67f15c15c0def594b79d424b501;SSL=true;SslMode=Require").Options;

            context = new SolumaticaContext(options);
            unitOfWork = new UnitOfWork(context);
        }

        [Test]
        public void Test1()
        {
            
        }
    }
}
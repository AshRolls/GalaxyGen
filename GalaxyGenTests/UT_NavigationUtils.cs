using FluentAssertions;
using GCEngine.Framework;
using NUnit.Framework;

namespace GalaxyGenTests
{
    [TestFixture]
    public class UT_NavigationUtils
    {
        [Test]
        public void Check_Destination()
        {
            double aX = 1;
            double aY = -1;
            double bX = 2;
            double bY = 4;
            double distance = 4;

            PointD res = NavigationUtils.GetNewPointForShip(distance, aX, aY, bX, bY);
            res.X.Should().BeApproximately(1.79, 0.01);
            res.Y.Should().BeApproximately(2.92, 0.01);
        }

    }
}

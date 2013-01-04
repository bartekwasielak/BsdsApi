using WebMuses.PME.Core;

namespace WebMuses.PME.SampleConsole
{
    class Program
    {
        static void Main()
        {
            var bsdsAccess = new BsdsAccess("Av1Pxhxmw1q2Pa8yYeRoO6nRSQttINrDGcmvmPfHzAfokdT0alyVHecHDPNC0oAO");
            var results = bsdsAccess.FindByAreaRadius(47.63674, -122.30413, 3);
        }
    }
}

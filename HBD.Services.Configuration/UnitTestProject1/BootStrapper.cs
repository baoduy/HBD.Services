using HBD.Mef.Hosting;

namespace HBD.Services.Configuration.StTests
{
    class BootStrapper : Mef.StandardBootstrapper
    {
        static BootStrapper _b;

        public static BootStrapper Default
        {
            get
            {
                if (_b != null) return _b;
                _b = new BootStrapper();
                _b.Run();
                return _b;
            }
        }

        protected override ExtendedContainerConfiguration CreateContainerConfiguration()
        {
            var config =  base.CreateContainerConfiguration();
            config.WithAssembly(typeof(BootStrapper).Assembly);
            return config;
        }
    }
}

using System.ComponentModel.Composition.Hosting;

namespace HBD.Services.Configuration._4xTests
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

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(BootStrapper).Assembly));
        }
    }
}

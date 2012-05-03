using System.Diagnostics;

namespace MyShows.UI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using Caliburn.Micro;

    public class SimpleLog : ILog
    {
        readonly Type type;

        public SimpleLog(Type type)
        {
            this.type = type;
        }

        public void Info(string format, params object[] args)
        {
        //    Debug.WriteLine("INFO: {0} : {1}", type.Name, string.Format(format, args));
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine("WARN: {0} : {1}", type.Name, string.Format(format, args));
        }

        public void Error(Exception exception)
        {
            Debug.WriteLine("ERROR: {0}\n{1}", type.Name, exception);
        }
    }

    public class AppBootstrapper : Bootstrapper<IShell>
    {
        CompositionContainer container;

        protected override void StartRuntime()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            LogManager.GetLog = type => new SimpleLog(type);
            base.StartRuntime();
        }
        /// <summary>
        /// By default, we are configured to use MEF
        /// </summary>
        protected override void Configure()
        {
            var catalog = new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                );

            container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);
            batch.AddExportedValue(catalog);

            container.Compose(batch);

//            ViewLocator.NameTransformer.AddRule
//(
//   @"(?<namespace>(.*\.)*)Views\.(?<basename>[A-Za-z_]\w*)(?<suffix>View$)",
//   new[] {
//            @"${namespace}ViewModels.${basename}ViewModel",
//            @"${namespace}ViewModels.${basename}",
//            @"${namespace}ViewModels.I${basename}ViewModel",
//            @"${namespace}ViewModels.I${basename}"
//        },
//   @"(.*\.)*Views\.[A-Za-z_]\w*View$"
//);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }
    }
}

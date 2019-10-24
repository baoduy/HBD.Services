using System;

namespace HBD.Services.Transformation
{
    internal class TransformSection : IDisposable
    {
        private readonly TransformerService service;

        public TransformSection(TransformerService service) => this.service = service;

        public void Dispose() => service.ClearCache();
    }
}

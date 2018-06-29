using Castle.DynamicProxy;

namespace HBD.Services.Polly
{
    public interface IInterceptorBuilder
    {
        IInterceptor CreateInterceptor();
    }
}
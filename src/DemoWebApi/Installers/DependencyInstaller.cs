using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DemoWebApi.Managers;
using DemoWebApi.Providers;
using FluentValidation;

namespace DemoWebApi.Installers
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IUserManager>()
                .ImplementedBy<UserManager>()
                .LifestyleSingleton());

            container.Register(Component
                .For<IValidationProvider>()
                .ImplementedBy<ValidationProvider>()
                .LifestyleSingleton()
                .CrossWired());

            container.Register(Classes
                .FromAssembly(typeof(DependencyInstaller).Assembly)
                .BasedOn<IValidator>()
                .WithServiceAllInterfaces()
                .LifestyleSingleton());
        }
    }
}

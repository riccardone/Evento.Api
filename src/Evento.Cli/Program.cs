using Evento.Cli;
using Evento.Cli.Contracts;
using Evento.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using static Evento.Cli.HostingPlaygroundLogEvents;

class Program
{
    static async Task Main(string[] args) => await BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton<IGreeter, Greeter>();
                        services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
                    });
                })
            .UseDefaults()
            .Build()
            .InvokeAsync(args);

    private static CommandLineBuilder BuildCommandLine()
    {
        var ciccio = new RootCommand("dotnet run create organization --name 'ciccio inc.'");
        var create = new Command("create");
        var organization = new Command("organization")
        {
            new Option<string>("--name"){
                IsRequired = true
            }
        };
        create.AddCommand(organization);
        ciccio.AddCommand(create);
        ciccio.Handler = CommandHandler.Create<OrganizationOptions, IHost>(Run);
        return new CommandLineBuilder(ciccio);
        //var root = new RootCommand(@"$ dotnet run --name 'Joe'"){
        //        new Option<string>("--name"){
        //            IsRequired = true
        //        }
        //    };
        //root.Handler = CommandHandler.Create<GreeterOptions, IHost>(Run);
        //return new CommandLineBuilder(root);
    }

    private static void Run(OrganizationOptions options, IHost host)
    {
        var serviceProvider = host.Services;
        var organizationRepository = serviceProvider.GetRequiredService<IOrganizationRepository>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(Program));

        var name = options.Name;
        logger.LogInformation(CreateOrganizationEvent, "Create organization was requested for: {name}", name);
        organizationRepository.Create(name);
    }

    //private static void Run(GreeterOptions options, IHost host)
    //{
    //    var serviceProvider = host.Services;
    //    var greeter = serviceProvider.GetRequiredService<IGreeter>();
    //    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    //    var logger = loggerFactory.CreateLogger(typeof(Program));

    //    var name = options.Name;
    //    logger.LogInformation(GreetEvent, "Greeting was requested for: {name}", name);
    //    greeter.Greet(name);
    //}
}
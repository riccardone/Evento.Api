namespace Evento.Cli.Contracts
{
    interface IOrganizationRepository
    {
        void Create(string name) => Console.WriteLine($"Organization '{name}' created");
    }
}

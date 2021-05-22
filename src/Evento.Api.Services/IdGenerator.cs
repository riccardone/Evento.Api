using System;

namespace Evento.Api.Services
{
    public class IdGenerator : IIdGenerator
    {
        public string GenerateId(string prefix)
        {
            return $"{prefix}{Guid.NewGuid()}";
        }
    }
}

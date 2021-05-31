using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Api.Model;
using Finbuckle.MultiTenant;

namespace Evento.Api.Tests.Fakes
{
    public class FakeMultitenantStore : IMultiTenantStore<EventoTenantInfo>
    {
        private readonly bool _ingestInvalidPayloads;
        private const string ValidationErrorField = "ValidationError";

        public FakeMultitenantStore()
        {

        }

        public FakeMultitenantStore(bool ingestInvalidPayloads)
        {
            _ingestInvalidPayloads = ingestInvalidPayloads;
        }
        public async Task<bool> TryAddAsync(EventoTenantInfo tenantInfo)
        {
            return Task.Run(async () =>
            {
                return true;
            }).GetAwaiter().GetResult();
        }

        public async Task<bool> TryUpdateAsync(EventoTenantInfo tenantInfo)
        {
            return Task.Run(async () =>
            {
                return true;
            }).GetAwaiter().GetResult();
        }

        public async Task<bool> TryRemoveAsync(string identifier)
        {
            return Task.Run(async () =>
            {
                return true;
            }).GetAwaiter().GetResult();
        }

        public async Task<EventoTenantInfo> TryGetByIdentifierAsync(string identifier)
        {
            return Task.Run(async () =>
            {
                return BuildTenant(Guid.NewGuid().ToString(), identifier);
            }).GetAwaiter().GetResult();
        }

        private EventoTenantInfo BuildTenant(string id, string identifier)
        {
            return new()
            {
                Identifier = identifier,
                CryptoKey = "3x+bfONnKwILntoe1hBELOpD1u+1cp2ZceRVnbW2fAs=",
                IngestInvalidPayloads = _ingestInvalidPayloads,
                ValidationErrorField = ValidationErrorField
            };
        }

        public async Task<EventoTenantInfo> TryGetAsync(string id)
        {
            return Task.Run(async () =>
            {
                return BuildTenant(id, "test");
            }).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<EventoTenantInfo>> GetAllAsync()
        {
            return Task.Run(async () =>
            {
                return new List<EventoTenantInfo>
                    {BuildTenant(Guid.NewGuid().ToString(), "test")};
            }).GetAwaiter().GetResult();
        }
    }
}

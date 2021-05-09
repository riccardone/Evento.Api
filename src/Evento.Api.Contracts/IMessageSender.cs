﻿using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace Evento.Api.Contracts
{
    /// <summary>
    /// Contract to implement a message sender
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Send a batch of CloudEvents
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        Task SendAsync(CloudEvent[] requests);
        /// <summary>
        /// Send a CloudEvent message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task SendAsync(CloudEvent request);
    }
}

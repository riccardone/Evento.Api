﻿using System.Threading.Tasks;
using CloudEventData;

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
        Task SendAsync(CloudEventRequest[] requests);
        /// <summary>
        /// Send a CloudEvent message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task SendAsync(CloudEventRequest request);
    }
}

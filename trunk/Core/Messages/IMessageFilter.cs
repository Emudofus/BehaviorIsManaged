using System;

namespace BiM.Core.Messages
{
    public interface IMessageFilter
    {
        /// <summary>
        /// Method called to allow or not a message handling.
        /// </summary>
        /// <param name="message">The handled message</param>
        /// <param name="sender">The message sender</param>
        /// <param name="handlerContainer">The container type of the handler</param>
        /// <param name="container">The container of the handler (null if static)</param>
        /// <returns>True to allow the message handling</returns>
        bool Handle(Message message, object sender, Type handlerContainer, object container);
    }
}
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.CQRS
{
    /// <summary>
    /// This will write data
    /// </summary>
    public interface ICommand : ICommand<Unit>
    {

    }
    /// <summary>
    /// This Produce Response
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}

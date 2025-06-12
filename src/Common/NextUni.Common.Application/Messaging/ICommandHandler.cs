using MediatR;
using NextUni.Common.Domain;

namespace NextUni.Common.Application.Messaging;

public interface ICommandHandler : IRequestHandler<ICommand>;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
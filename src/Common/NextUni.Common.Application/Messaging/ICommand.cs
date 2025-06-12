using MediatR;
using NextUni.Common.Domain;

namespace NextUni.Common.Application.Messaging;

public interface ICommand : IRequest, IBaseCommand;

public interface ICommand<TResponse> : IBaseCommand, IRequest<Result<TResponse>>;

public interface IBaseCommand;
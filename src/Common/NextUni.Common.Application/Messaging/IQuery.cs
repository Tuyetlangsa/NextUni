using MediatR;
using NextUni.Common.Domain;

namespace NextUni.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
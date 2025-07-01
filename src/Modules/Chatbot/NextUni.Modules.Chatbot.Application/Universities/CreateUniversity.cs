using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using NextUni.Modules.Chatbot.Application.Abstractions.EmbeddingGenerator;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace NextUni.Modules.Chatbot.Application.Universities;

public abstract class CreateUniversity
{
    public record Command(
        Guid Id,
        string FormattedText
    ) : ICommand<Guid>;
    
    internal sealed class Handler(IEmbeddingGenerator generator, QdrantClient client) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var collections = await client.ListCollectionsAsync(cancellationToken);
            if (!collections.Contains("semantic_embeddings"))
            {
                await client.CreateCollectionAsync("semantic_embeddings", new VectorParams()
                {
                    Size = 768,
                    Distance = Distance.Cosine 
                }, cancellationToken: cancellationToken);
            }
            
            //upsert the embedding cho toi
            
            var embedding = await generator.GenerateAsync(command.FormattedText);
            var point = new PointStruct()
            {
                Id = command.Id,
                Vectors = embedding,
                Payload = 
                {
                    ["entity_type"] = "university",
                    ["origin_text"] = command.FormattedText
                }
            };

            var updateResult = await client.UpsertAsync("semantic_embeddings", [point], cancellationToken: cancellationToken);
            if (updateResult.Status == UpdateStatus.Completed)
            {
                return Result.Success(command.Id);    
            }
            else {
                return Result.Failure<Guid>(new Error("Failed to upsert university embedding", "UpsertError", ErrorType.Problem));
            }
            
        }
    }
}
using Pgvector;

namespace NextUni.Modules.Chatbot.Application.Abstractions.EmbeddingGenerator;

public interface IEmbeddingGenerator
{
    Task<float[]> GenerateAsync(string input);
}
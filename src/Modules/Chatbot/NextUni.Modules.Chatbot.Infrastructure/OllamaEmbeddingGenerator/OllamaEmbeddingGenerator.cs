using Microsoft.Extensions.AI;
using Pgvector;
using IEmbeddingGenerator = NextUni.Modules.Chatbot.Application.Abstractions.EmbeddingGenerator.IEmbeddingGenerator;

namespace NextUni.Modules.Chatbot.Infrastructure.OllamaEmbeddingGenerator;

public class MyEmbeddingGenerator(IEmbeddingGenerator<string, Embedding<float>> generator) : IEmbeddingGenerator
{
    public async Task<float[]> GenerateAsync(string input)
    {
        var embedding = await generator.GenerateAsync(input);

        return embedding.Vector.ToArray();
    }
}
using System.Text.Json;
using Microsoft.Extensions.AI;
using NextUni.Common.Application.Messaging;
using NextUni.Common.Domain;
using Qdrant.Client;
using IEmbeddingGenerator = NextUni.Modules.Chatbot.Application.Abstractions.EmbeddingGenerator.IEmbeddingGenerator;

namespace NextUni.Modules.Chatbot.Application.Chatbot;

public abstract class Chatbot
{
    public record Query(string Prompt) : IQuery<string>;
    
    public enum QueryKey
    {
        University,
        MajorsOfUniversity,
        SubjectGroupOfMajor,
        AdmissionScoreOfUniversityByYear
    }
    
    internal sealed class Handler(
        IEmbeddingGenerator embeddingGenerator,
        QdrantClient qdrantClient,
        IChatClient chatClient) : IQueryHandler<Query, string>
    {
        private const string CLASSIFICATION_SYSTEM_PROMPT = @"
                            You are a query classifier for an educational information system. Your task is to analyze user questions and return the appropriate QueryKey classification.

                            Available QueryKey Categories:
                            1. University - Questions about universities in general, specific university information, university details, rankings, locations, etc.
                            2. MajorsOfUniversity - Questions about majors/programs offered by a specific university or universities
                            3. SubjectGroupOfMajor - Questions about subject groups or courses within a specific major/program
                            4. AdmissionScoreOfUniversityByYear - Questions about admission scores, entrance requirements, cutoff scores by year for universities
        
                            Instructions: Analyze the user's question and respond with ONLY the appropriate QueryKey enum value. Do not provide explanations or additional text.";
        
        public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            ChatResponse queryKey = await chatClient.GetResponseAsync<QueryKey>(
                [
                    new ChatMessage(ChatRole.System, CLASSIFICATION_SYSTEM_PROMPT),
                    new ChatMessage(ChatRole.User, request.Prompt)]);
            
            var queryVector = await embeddingGenerator.GenerateAsync(request.Prompt);
            var json = JsonDocument.Parse(queryKey.Text);
            var collectionName = json.RootElement.GetProperty("data").GetString()?.ToLowerInvariant();
            
            var points = await qdrantClient.SearchAsync(
                collectionName,
                queryVector,
                limit: 10);
            var contextTexts = points
                .Select(p => p.Payload != null 
                             && p.Payload.TryGetValue("origin_text", out var textObj) ? textObj?.ToString() : null)
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList();
            var context = string.Join("\n", contextTexts);
        
            ChatResponse response = await chatClient.GetResponseAsync(
                [
                new(ChatRole.System, $"""
                                      Bạn là một trợ lý tư vấn đại học, nhiệm vụ của bạn là trả lời câu hỏi của người dùng dựa trên các thông tin được cung cấp bên dưới.

                                      Chỉ sử dụng thông tin trong phần "Ngữ cảnh" để đưa ra câu trả lời. Nếu không đủ thông tin, hãy trả lời rằng bạn không có dữ liệu phù hợp để trả lời câu hỏi.
                                      
                                      Không phân tích quá sâu về câu hỏi, chỉ cần trả lời rõ ràng và chính xác nhất có thể.
                                      ### Ngữ cảnh:
                                      {context}

                                      ### Hướng dẫn:
                                      - Trả lời ngắn gọn, chính xác và đúng trọng tâm.
                                      - Tránh suy đoán hoặc thêm thông tin ngoài ngữ cảnh.
                                      - Trả lời bằng tiếng việt.
                                      - Nếu câu hỏi không liên quan đến thông tin trong ngữ cảnh, hãy trả lời rằng bạn không có dữ liệu phù hợp để trả lời câu hỏi.
                                      - Trả lời theo đúng định dạng của câu hỏi.
                                      - Đừng suy luận quá nhiều, chỉ cần trả lời dựa trên thông tin đã cho.
                                      - Không cần tìm kiếm data ngoài ngữ cảnh đã cung cấp.
                                      """),
                new (ChatRole.User, request.Prompt)]);
        
            return response.Text;
        }
    }
}
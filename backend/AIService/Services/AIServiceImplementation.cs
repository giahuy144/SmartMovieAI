using AIService.Contracts;

namespace AIService.Services
{
    public class AIServiceImplementation : IAIService
    {
        public SentimentResponse AnalyzeSentiment(SentimentRequest request)
        {
            string textLower = (request?.Text ?? "").ToLower();
            string sentiment = "Neutral";
            double confidence = 0.85;

            if (textLower.Contains("hay") || textLower.Contains("xuất sắc") || textLower.Contains("tuyệt vời") || textLower.Contains("thích") || textLower.Contains("good") || textLower.Contains("awesome") || textLower.Contains("đẹp"))
            {
                sentiment = "Positive";
                confidence = 0.95;
            }
            else if (textLower.Contains("dở") || textLower.Contains("tệ") || textLower.Contains("chán") || textLower.Contains("bad") || textLower.Contains("khó hiểu") || textLower.Contains("tốn thời gian"))
            {
                sentiment = "Negative";
                confidence = 0.91;
            }

            return new SentimentResponse
            {
                Text = request?.Text ?? string.Empty,
                Sentiment = sentiment,
                Confidence = confidence,
                AnalyzedAt = DateTime.UtcNow
            };
        }

        public SummaryResponse SummarizeReview(SummaryRequest request)
        {
            if (request?.Reviews == null || request.Reviews.Count == 0)
            {
                return new SummaryResponse
                {
                    MovieId = request?.MovieId ?? 0,
                    Summary = "Chưa có đủ nhận xét để tóm tắt.",
                    Pros = new List<string>(),
                    Cons = new List<string>()
                };
            }

            return new SummaryResponse
            {
                MovieId = request.MovieId,
                TotalReviews = request.Reviews.Count,
                Summary = "Phim nhận được phản hồi tích cực từ khán giả về hình ảnh sống động và kỹ xảo sắc nét, tuy nhiên thời lượng có phần hơi kéo dài.",
                Pros = new List<string>
                {
                    "Nội dung hấp dẫn và giàu cảm xúc",
                    "Kỹ xảo hình ảnh và âm thanh đỉnh cao",
                    "Diễn xuất của dàn nhân vật chính ấn tượng"
                },
                Cons = new List<string>
                {
                    "Thời lượng phim tương đối dài (~3 tiếng)",
                    "Một số đoạn tình tiết diễn biến hơi chậm"
                }
            };
        }

        public RecommendationResponse RecommendMovies(RecommendationRequest request)
        {
            var recommendedMovies = new List<MovieRecommendationItem>
            {
                new MovieRecommendationItem { Id = 1, Title = "Interstellar", Genre = "Sci-Fi", MatchScore = 0.98, Reason = "Phù hợp với sở thích phim Khoa học viễn tưởng & khám phá vũ trụ của bạn" },
                new MovieRecommendationItem { Id = 2, Title = "Inception", Genre = "Sci-Fi / Action", MatchScore = 0.94, Reason = "Có cùng đạo diễn Christopher Nolan và cốt truyện giật gân" },
                new MovieRecommendationItem { Id = 3, Title = "The Dark Knight", Genre = "Action", MatchScore = 0.91, Reason = "Phim có điểm rating cực cao từ cộng đồng người dùng tương đồng" }
            };

            return new RecommendationResponse
            {
                UserId = request?.UserId ?? 1,
                FavoriteGenre = request?.FavoriteGenre ?? "Sci-Fi",
                Recommendations = recommendedMovies
            };
        }

        public ChatResponse Chat(ChatRequest request)
        {
            string msg = (request?.Message ?? "").ToLower();
            string reply;

            if (msg.Contains("interstellar") || msg.Contains("khoa học viễn tưởng") || msg.Contains("sci-fi"))
            {
                reply = "Nếu bạn thích thể loại Khoa học viễn tưởng như Interstellar, tôi đề xuất các phim: Inception, Tenet, The Martian, và Dune!";
            }
            else if (msg.Contains("hành động") || msg.Contains("action"))
            {
                reply = "Dưới đây là một số phim Hành động đỉnh cao: John Wick 4, Mad Max: Fury Road, và The Dark Knight!";
            }
            else if (msg.Contains("dưới 2 tiếng") || msg.Contains("ngắn"))
            {
                reply = "Các phim hay có thời lượng dưới 2 tiếng: Whiplash (1h 47m), Spider-Man: Into the Spider-Verse (1h 57m), A Quiet Place (1h 30m).";
            }
            else
            {
                reply = "Chào bạn! Smart Movie AI RAG Assistant rất vui được hỗ trợ. Bạn có thể hỏi tôi về các thể loại phim, phim tương tự hoặc yêu cầu gợi ý phim theo thời lượng!";
            }

            return new ChatResponse
            {
                UserMessage = request?.Message ?? string.Empty,
                BotReply = reply,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}

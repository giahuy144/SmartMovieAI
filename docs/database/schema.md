# 🗄️ Smart Movie AI - Database Schema Documentation

File SQL chính thức lưu tại: [`docs/database/schema.sql`](file:///D:/SOA/SmartMovieAI/docs/database/schema.sql)

Database được thiết kế đầy đủ cho toàn bộ 13 bảng thuộc hệ thống **Smart Movie AI (SOA)** bao gồm quản lý người dùng, phim, thể loại, đánh giá, bình luận và các bảng tính năng nâng cao của **AI Service**.

---

## 📋 Danh sách 13 Bảng & Quan hệ

### 1. Nóm Người dùng & Phân quyền (Auth Service)
* **`Users`**: Lưu tài khoản (`UserId`, `Username`, `Email`, `PasswordHash`, `FullName`, `Role` [User/Admin], `IsActive`, `CreatedAt`).

### 2. Nhóm Phim & Thể loại (Movie Service)
* **`Movies`**: Thông tin phim (`MovieId`, `Title`, `Description`, `ReleaseDate`, `Duration`, `Director`, `PosterUrl`, `TrailerUrl`, `AverageRating`, `TotalReviews`, `CreatedAt`).
* **`Genres`**: Danh mục thể loại phim (`GenreId`, `Name`).
* **`MovieGenres`**: Bảng trung gian n-n giữa Phim & Thể loại (`MovieId`, `GenreId`).

### 3. Nhóm Đánh giá, Review & Bình luận (Review & Rating Service)
* **`Ratings`**: Lưu điểm đánh giá 0.0 - 10.0 sao (`RatingId`, `UserId`, `MovieId`, `Score`, `CreatedAt`).
* **`Reviews`**: Bài viết đánh giá & kết quả AI Sentiment (`ReviewId`, `UserId`, `MovieId`, `Content`, `Sentiment` [Positive/Negative/Neutral], `SentimentScore`, `CreatedAt`, `UpdatedAt`).
* **`Comments`**: Bình luận dưới từng Review (`CommentId`, `UserId`, `ReviewId`, `Content`, `CreatedAt`).
* **`Favorites`**: Danh sách phim yêu thích của User (`FavoriteId`, `UserId`, `MovieId`, `CreatedAt`).
* **`WatchHistory`**: Lịch sử xem phim (`WatchHistoryId`, `UserId`, `MovieId`, `WatchedAt`).

### 4. Nhóm AI Features (AI Service)
* **`AIReviewSummaries`**: Bảng tổng hợp ưu/nhược điểm review theo phim (`SummaryId`, `MovieId`, `Summary`, `PositivePercent`, `NegativePercent`, `NeutralPercent`, `TotalReviews`, `GeneratedAt`).
* **`AIRecommendations`**: Lưu kết quả gợi ý phim theo thuật toán AI (`RecommendationId`, `UserId`, `MovieId`, `Score`, `Reason`, `CreatedAt`).
* **`AIChatConversations`**: Các phiên trò chuyện giữa User và AI Chatbot (`ConversationId`, `UserId`, `Title`, `CreatedAt`).
* **`AIChatMessages`**: Tin nhắn trong từng phiên chat (`MessageId`, `ConversationId`, `Sender` [User/AI], `Message`, `CreatedAt`).

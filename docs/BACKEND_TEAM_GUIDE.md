# 👥 Kế hoạch phân công & Hướng dẫn Backend SmartMovieAI (Dành cho nhóm 4 người)

> **Kiến trúc:** Service-Oriented Architecture (SOA) bằng **C# / ASP.NET Core (.NET 9)**.  
> **Nguyên tắc:** Mỗi người làm trên **1 Service riêng**, độc lập cổng mạng (Port), độc lập Swagger, độc lập Model và Logic xử lý. Tuyệt đối không viết dồn tính năng vào chung một chỗ.

---

## 🏗️ Cấu trúc chuẩn cho từng Service (Clean Feature Architecture)

Tất cả các service đã được refactor và chuẩn hóa theo kiến trúc sau:
```text
backend/<Tên_Service>/
├── Controllers/       # Nhận HTTP Request, validate cơ bản, gọi Service và trả HTTP Response
├── Contracts/         # DTOs (Data Transfer Objects: Request / Response models)
├── Models/            # Entity models đại diện cho thực thể dữ liệu
├── Services/          # Interfaces & Implementations chứa toàn bộ Business Logic nghiệp vụ
├── Data/              # DbContext / Repository kết nối cơ sở dữ liệu (SQL Server EF Core)
├── appsettings.json   # Cấu hình Connection String, Port, JWT keys
└── Program.cs         # Cấu hình Dependency Injection (DI) & Swagger, CORS
```

---

## 🎯 Phân chia chi tiết cho 4 thành viên

### 👤 Thành viên 1: Quản lý Xác thực & Người dùng (`AuthService`)
* **Thư mục làm việc:** [`backend/AuthService`](file:///D:/SOA/SmartMovieAI/backend/AuthService)
* **Cổng (Port):** `http://localhost:5001` (Swagger: `http://localhost:5001/swagger`)
* **Nhiệm vụ & Tính năng:**
  1. Đăng ký tài khoản (`POST /api/auth/register`).
  2. Đăng nhập & cấp phát JWT Token (`POST /api/auth/login`).
  3. Xác thực & kiểm tra Token (`GET /auth`, `POST /auth`).
  4. Phân quyền vai trò người dùng (Role: `User`, `Admin`).
  5. Quản lý thông tin cá nhân và mật khẩu (băm mật khẩu bảo mật BCrypt/PBKDF2).
  6. Kết nối bảng `Users` trong SQL Server qua Entity Framework Core.
* **Git Branch đề xuất:** `feature/auth-service`

---

### 👤 Thành viên 2: Quản lý Phim & Danh mục (`MovieService`)
* **Thư mục làm việc:** [`backend/MovieService`](file:///D:/SOA/SmartMovieAI/backend/MovieService)
* **Cổng (Port):** `http://localhost:5002` (Swagger: `http://localhost:5002/swagger`)
* **Nhiệm vụ & Tính năng:**
  1. Xem danh sách phim & chi tiết phim (`GET /api/movies`, `GET /api/movies/{id}`).
  2. Tìm kiếm và bộ lọc nâng cao (`GET /api/movies/search?title=...&genre=...`).
  3. Thêm mới, chỉnh sửa, xóa phim (`POST`, `PUT`, `DELETE /api/movies/{id}`).
  4. Quản lý thể loại phim (`Genres`) và liên kết bảng trung gian `MovieGenres`.
  5. Cập nhật kết nối SQL Server (bảng `Movies`, `Genres`, `MovieGenres`) qua EF Core (xem schema tại `docs/database/schema.md`).
* **Git Branch đề xuất:** `feature/movie-service`

---

### 👤 Thành viên 3: Đánh giá & Xếp hạng (`ReviewService` & `RatingService`)
* **Thư mục làm việc:** [`backend/ReviewService`](file:///D:/SOA/SmartMovieAI/backend/ReviewService) và [`backend/RatingService`](file:///D:/SOA/SmartMovieAI/backend/RatingService)
* **Cổng (Port):** 
  - `ReviewService`: `http://localhost:5003` (Swagger: `http://localhost:5003/swagger`)
  - `RatingService`: `http://localhost:5004` (Swagger: `http://localhost:5004/swagger`)
* **Nhiệm vụ & Tính năng:**
  1. **RatingService:**
     - Người dùng chấm điểm 1–5 sao hoặc 0–10 điểm (`POST /api/ratings`).
     - Xem điểm đánh giá trung bình theo phim (`GET /api/ratings/movie/{movieId}`).
     - Thống kê tỷ lệ phân bổ các mức sao (`GET /api/ratings/movie/{movieId}/statistics`).
  2. **ReviewService:**
     - Viết bài đánh giá, nhận xét phim (`POST /api/reviews`).
     - Xem danh sách review theo từng phim hoặc theo người dùng (`GET /api/reviews/movie/{id}`, `GET /api/reviews/user/{id}`).
     - Xóa bài đánh giá (`DELETE /api/reviews/{id}`).
     - Gắn kết quả phân tích cảm xúc (Sentiment) vào review.
     - Tính năng phụ: Bình luận dưới review (`Comments`), Phim yêu thích (`Favorites`).
* **Git Branch đề xuất:** `feature/review-rating-service`

---

### 👤 Thành viên 4: Trí tuệ nhân tạo AI & Gợi ý (`AIService`)
* **Thư mục làm việc:** [`backend/AIService`](file:///D:/SOA/SmartMovieAI/backend/AIService)
* **Cổng (Port):** `http://localhost:5005` (Swagger: `http://localhost:5005/swagger`)
* **Nhiệm vụ & Tính năng:**
  1. Phân tích cảm xúc văn bản (`POST /api/ai/analyze-sentiment`) - Nhận diện Positive / Negative / Neutral và độ tin cậy.
  2. Tóm tắt nội dung các review của phim (`POST /api/ai/summarize-review`) - Tự động rút ra Pros (Ưu điểm) và Cons (Nhược điểm).
  3. Gợi ý phim thông minh (`POST /api/ai/recommend-movies`) - Thuật toán gợi ý dựa trên sở thích và thể loại phù hợp.
  4. Trợ lý ảo Chatbot (`POST /api/ai/chat`) - Trả lời câu hỏi, tư vấn phim theo yêu cầu của khán giả.
  5. Tích hợp AI nâng cao: Kết nối OpenAI / Gemini API hoặc Ollama nếu muốn thay thế luật từ khóa mặc định.
* **Git Branch đề xuất:** `feature/ai-service`

---

## 🛠️ Quy trình làm việc nhóm bằng Git

1. Mỗi thành viên clone repository:
   ```bash
   git clone <URL_REPO>
   cd SmartMovieAI
   ```
2. Tạo nhánh riêng của mình:
   ```bash
   git checkout -b feature/<ten-service-cua-ban>
   ```
3. Mở Visual Studio và chạy service của mình:
   - Chỉ cần chuột phải vào Project của mình chọn `Set as Startup Project` và bấm `F5` hoặc chạy lệnh:
     ```bash
     dotnet run --project backend/<Tên_Service>/<Tên_Service>.csproj
     ```
4. Khi hoàn thành tính năng:
   - Commit & push lên nhánh cá nhân, sau đó tạo Pull Request (PR) để review và merge vào `main`.

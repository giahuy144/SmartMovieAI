# 🎬 Smart Movie AI

> Hệ thống đánh giá, phân tích và đề xuất phim thông minh sử dụng **Service-Oriented Architecture (SOA)** và **Artificial Intelligence (AI)** bằng ngôn ngữ C# / ASP.NET Core.

## 📌 Giới thiệu

**Smart Movie AI** là ứng dụng web xem và khám phá phim theo kiến trúc SOA. Người dùng đăng ký, đăng nhập, duyệt phim, rồi dùng các công cụ AI trên cùng một trang chủ. Mỗi nghiệp vụ nằm trong một ASP.NET Core API riêng. Giao diện là Angular.

---

## Tính năng

Trên giao diện (`http://localhost:4200`):

| Màn hình | Việc người dùng làm |
|---|---|
| Đăng nhập / đăng ký | Tạo tài khoản, đăng nhập, xem độ mạnh mật khẩu, dùng tài khoản mẫu `use1` / `123` |
| Trang chủ `/home` | Xem danh sách phim, tìm theo tên / đạo diễn / thể loại, lọc thể loại, mở chi tiết, xem trailer, đánh dấu yêu thích trên trình duyệt |
| AI Sentiment | Dán một đoạn review và nhận nhãn Tích cực / Tiêu cực / Trung lập kèm độ tin cậy |
| Gợi ý | Chọn thể loại yêu thích và nhận danh sách phim kèm điểm khớp và lý do |
| Chatbot | Hỏi gợi ý theo thể loại, phim ngắn, hoặc phim giống một tác phẩm |
| Đăng xuất | Xóa phiên và quay về trang đăng nhập |

Yêu thích chỉ lưu trong phiên trang (bộ nhớ trình duyệt), chưa ghi xuống server.

---

## Dịch vụ và nghiệp vụ

Mỗi service là một process riêng, có Swagger, và CORS mở cho frontend.

| Service | Cổng | Nghiệp vụ đang chạy | Dữ liệu |
|---|---|---|---|
| AuthService | 5001 | Đăng ký, đăng nhập, cấp JWT, kiểm tra token `GET/POST /auth` | SQL Server, bảng `Users` qua EF Core |
| MovieService | 5002 | Xem, tìm, thêm, sửa, xóa phim | Danh sách trong bộ nhớ |
| ReviewService | 5003 | Viết review, xem theo phim hoặc user, xóa review, gán sentiment bằng luật từ khóa | Danh sách trong bộ nhớ |
| RatingService | 5004 | Chấm 1–5 sao (mỗi user một điểm trên một phim), điểm trung bình, thống kê số sao | Danh sách trong bộ nhớ |
| AIService | 5005 | Phân tích cảm xúc, tóm tắt review, gợi ý phim, chatbot | Luật từ khóa và câu trả lời mẫu, không gọi mô hình ngôn ngữ bên ngoài |

API chính:

- Auth: `POST /api/auth/register`, `POST /api/auth/login`, `GET|POST /auth` (cần Bearer token)
- Movie: `GET /api/movies`, `GET /api/movies/{id}`, `GET /api/movies/search?title=&genre=`, `POST /api/movies`, `PUT /api/movies/{id}`, `DELETE /api/movies/{id}`
- Review: `POST /api/reviews`, `GET /api/reviews/movie/{movieId}`, `GET /api/reviews/user/{userId}`, `DELETE /api/reviews/{id}`
- Rating: `POST /api/ratings`, `GET /api/ratings/movie/{movieId}`, `GET /api/ratings/movie/{movieId}/statistics`
- AI: `POST /api/ai/analyze-sentiment`, `POST /api/ai/summarize-review`, `POST /api/ai/recommend-movies`, `POST /api/ai/chat`

Frontend đang gọi Auth, Movie và AI. Review và Rating có API, chưa có màn hình riêng.

Schema SQL đầy đủ hơn (13 bảng: user, phim, thể loại, rating, review, comment, yêu thích, lịch sử xem, tóm tắt AI, gợi ý, hội thoại chat) nằm ở `docs/database/schema.md`. Hiện chỉ AuthService ghi SQL Server. Các service còn lại chưa nối schema đó.

---

## Công nghệ

| Phần | Dùng |
|---|---|
| Backend | C#, ASP.NET Core 9, Swagger (Swashbuckle) |
| Xác thực | JWT Bearer, mật khẩu băm, mật khẩu đăng nhập từ frontend được mã hóa Base64 trước khi gửi |
| Cơ sở dữ liệu | SQL Server / LocalDB + Entity Framework Core (chỉ AuthService) |
| Frontend | Angular 17, TypeScript, RxJS, Angular Forms, HttpClient |
| Giao tiếp | REST JSON, mỗi service một cổng, không có API gateway |
| AI | Luật từ khóa trong AIService và bản dự phòng cùng kiểu trên Angular khi service tắt |

---

## 🏗️ Cấu trúc Repo & Solution C#

Repository bao gồm C# Visual Studio Solution (`SmartMovieAI.sln`):

```text
SmartMovieAI/
│
├── SmartMovieAI.sln
│
├── backend/
│   ├── AuthService/          # Service Đăng ký / Đăng nhập / JWT Token / Phân quyền Role (C# ASP.NET Core API)
│   ├── MovieService/         # Service Quản lý Phim / Thể loại / Diễn viên / Tìm kiếm (C# ASP.NET Core API)
│   ├── ReviewService/        # Service Đánh giá & Review tích hợp AI Sentiment (C# ASP.NET Core API)
│   ├── RatingService/        # Service Thống kê Điểm 1-5 sao & Phân tích rating (C# ASP.NET Core API)
│   └── AIService/            # AI Provider Service (Sentiment, Summary, Recommendation, Chatbot)
│
├── docs/
│   ├── database/
│   │   └── schema.md         # Database Schema DDL cho SQL Server
│   ├── architecture/         # Sơ đồ kiến trúc SOA
│   └── api/                  # Swagger API Docs Specs
│
├── frontend/                 # Angular 17: đăng nhập và trang chủ AI
└── README.md
```

---

## 🚀 Hướng dẫn Chạy C# Solution (Visual Studio / .NET CLI)

### Cách 1: Sử dụng Visual Studio 2022
1. Mở file `SmartMovieAI.sln` trong Visual Studio.
2. Chọn `Set Startup Projects...` -> Chọn Multiple Startup Projects (bật `AuthService`, `MovieService`, `ReviewService`, `RatingService`, `AIService`).
3. Nhấn `F5` hoặc nút `Start` để khởi chạy đồng thời tất cả các Service.
4. Truy cập giao diện Swagger UI của từng service để kiểm thử API:
   - Auth Service: `http://localhost:5001/swagger`
   - Movie Service: `http://localhost:5002/swagger`
   - Review Service: `http://localhost:5003/swagger`
   - Rating Service: `http://localhost:5004/swagger`
   - AI Service: `http://localhost:5005/swagger`

### Cách 2: Sử dụng .NET CLI Terminal
```bash
# Biên dịch toàn bộ C# Solution
dotnet build SmartMovieAI.sln

# Chạy riêng lẻ từng service
dotnet run --project backend/AuthService/AuthService.csproj
dotnet run --project backend/MovieService/MovieService.csproj
dotnet run --project backend/ReviewService/ReviewService.csproj
dotnet run --project backend/RatingService/RatingService.csproj
dotnet run --project backend/AIService/AIService.csproj
```

---

## Chạy AuthService và Frontend Angular

Mở hai terminal tại thư mục gốc của repository.

### 1. Chạy backend AuthService

AuthService dùng SQL Server LocalDB/SQL Server với database `SmartMovieAI`. Kiểm tra hoặc điều chỉnh connection string `DefaultConnection` trong `backend/AuthService/appsettings.json` trước khi chạy.

```powershell
dotnet run --project backend/AuthService/AuthService.csproj
```

Backend chạy tại `http://localhost:5001`. Có thể kiểm tra API và JWT trên Swagger:

```text
http://localhost:5001/swagger
```

### 2. Chạy frontend Angular

Yêu cầu Node.js 18 hoặc 20 LTS (Angular 17 chưa hỗ trợ Node.js 24).

```powershell
cd frontend
npm install
npm start
```

Mở URL Angular hiển thị trong terminal, thường là `http://localhost:4200`. Frontend gọi AuthService tại `http://localhost:5001`, vì vậy backend phải chạy trước hoặc đồng thời.

### 3. Thử luồng xác thực

1. Mở frontend và đăng nhập bằng `use1` / `123`.
2. Frontend gửi `userName` cùng mật khẩu Base64 tới `POST /api/auth/login`.
3. Backend trả JWT; frontend lưu token và chuyển đến `/home`.
4. Trang chủ gọi Movie Service (`:5002`) và AI Service (`:5005`) để hiện danh sách phim, phân tích cảm xúc, gợi ý và chatbot. Nếu backend chưa chạy, giao diện dùng dữ liệu dự phòng.

---

## 👨‍💻 Tác giả

| Họ tên | Vai trò | Công nghệ |
|---|---|---|
| **Phan Nguyễn Gia Huy** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Hồ Minh Chí** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Trần Bảo Việt** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Trần Thị Mỹ Thu** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Võ Thị Kiều Trang** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |

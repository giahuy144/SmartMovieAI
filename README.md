# 🎬 Smart Movie AI

> Hệ thống đánh giá, phân tích và đề xuất phim thông minh sử dụng **Service-Oriented Architecture (SOA)** và **Artificial Intelligence (AI)** bằng ngôn ngữ C# / ASP.NET Core.

## 📌 Giới thiệu

**Smart Movie AI** là giải pháp phần mềm toàn diện được thiết kế theo kiến trúc SOA. Hệ thống cung cấp dịch vụ quản lý phim, đánh giá review, tính điểm rating và tích hợp **AI Service** chuyên biệt (Sentiment Analysis, Tóm tắt review, Chatbot tư vấn, Đề xuất phim thông minh).

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
cd frontend/smart-movie-angular
npm install
npm start
```

Mở URL Angular hiển thị trong terminal, thường là `http://localhost:4200`. Frontend gọi AuthService tại `http://localhost:5001`, vì vậy backend phải chạy trước hoặc đồng thời.

### 3. Thử luồng xác thực

1. Mở frontend và đăng nhập bằng `use1` / `123`.
2. Frontend gửi `userName` cùng mật khẩu Base64 tới `POST /api/auth/login`.
3. Backend trả JWT; frontend lưu token và chuyển đến `/hello`.
4. Trang `/hello` gọi `GET /auth` với header `Authorization: Bearer <token>` và nhận `Hello World`.

---

## 🤖 5. Các Chức năng AI Service chính

1. **Phân tích Cảm xúc (Sentiment Analysis)**: `POST /api/ai/analyze-sentiment`
2. **AI Review Summary**: `POST /api/ai/summarize-review`
3. **Movie Recommendation**: `POST /api/ai/recommend-movies`
4. **AI Chatbot**: `POST /api/ai/chat`

---

## 👨‍💻 Tác giả

| Họ tên | Vai trò | Công nghệ |
|---|---|---|
| **Phan Nguyễn Gia Huy** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Hồ Minh Chí** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Trần Bảo Việt** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Trần Thị Mỹ Thu** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |
| **Võ Thị Kiều Trang** | Lead Developer | C#, ASP.NET Core, EF Core, SOA, AI API |

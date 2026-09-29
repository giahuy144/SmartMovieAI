CREATE DATABASE SmartMovieAI;
GO

USE SmartMovieAI;
GO

-- =============================================
-- USERS
-- =============================================

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    Role NVARCHAR(20) NOT NULL DEFAULT 'User',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('User', 'Admin'))
);

-- =============================================
-- MOVIES
-- =============================================

CREATE TABLE Movies (
    MovieId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    ReleaseDate DATE,
    Duration INT,
    Director NVARCHAR(150),
    PosterUrl NVARCHAR(500),
    TrailerUrl NVARCHAR(500),
    AverageRating DECIMAL(3,2) DEFAULT 0,
    TotalReviews INT DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Movies_AverageRating
        CHECK (AverageRating >= 0 AND AverageRating <= 10),

    CONSTRAINT CK_Movies_Duration
        CHECK (Duration IS NULL OR Duration > 0)
);

-- =============================================
-- GENRES
-- =============================================

CREATE TABLE Genres (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE
);

-- =============================================
-- MOVIE - GENRE
-- =============================================

CREATE TABLE MovieGenres (
    MovieId INT NOT NULL,
    GenreId INT NOT NULL,

    PRIMARY KEY (MovieId, GenreId),

    CONSTRAINT FK_MovieGenres_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE,

    CONSTRAINT FK_MovieGenres_Genre
        FOREIGN KEY (GenreId)
        REFERENCES Genres(GenreId)
        ON DELETE CASCADE
);

-- =============================================
-- RATINGS
-- =============================================

CREATE TABLE Ratings (
    RatingId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Score DECIMAL(3,1) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Ratings_Score
        CHECK (Score >= 0 AND Score <= 10),

    CONSTRAINT UQ_Ratings_User_Movie
        UNIQUE (UserId, MovieId),

    CONSTRAINT FK_Ratings_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Ratings_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE
);

-- =============================================
-- REVIEWS
-- =============================================

CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    Sentiment NVARCHAR(20),
    SentimentScore DECIMAL(5,4),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Reviews_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Reviews_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE,

    CONSTRAINT CK_Reviews_Sentiment
        CHECK (
            Sentiment IS NULL
            OR Sentiment IN ('Positive', 'Negative', 'Neutral')
        )
);

-- =============================================
-- COMMENTS
-- =============================================

CREATE TABLE Comments (
    CommentId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ReviewId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Comments_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Comments_Review
        FOREIGN KEY (ReviewId)
        REFERENCES Reviews(ReviewId)
        ON DELETE CASCADE
);

-- =============================================
-- FAVORITES
-- =============================================

CREATE TABLE Favorites (
    FavoriteId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_Favorites_User_Movie
        UNIQUE (UserId, MovieId),

    CONSTRAINT FK_Favorites_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Favorites_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE
);

-- =============================================
-- WATCH HISTORY
-- =============================================

CREATE TABLE WatchHistory (
    WatchHistoryId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    WatchedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_WatchHistory_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_WatchHistory_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE
);

-- =============================================
-- AI REVIEW SUMMARY
-- =============================================

CREATE TABLE AIReviewSummaries (
    SummaryId INT IDENTITY(1,1) PRIMARY KEY,
    MovieId INT NOT NULL,
    Summary NVARCHAR(MAX) NOT NULL,
    PositivePercent DECIMAL(5,2),
    NegativePercent DECIMAL(5,2),
    NeutralPercent DECIMAL(5,2),
    TotalReviews INT DEFAULT 0,
    GeneratedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AIReviewSummaries_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE
);

-- =============================================
-- AI RECOMMENDATIONS
-- =============================================

CREATE TABLE AIRecommendations (
    RecommendationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Score DECIMAL(5,4) NOT NULL,
    Reason NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AIRecommendations_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE,

    CONSTRAINT FK_AIRecommendations_Movie
        FOREIGN KEY (MovieId)
        REFERENCES Movies(MovieId)
        ON DELETE CASCADE
);

-- =============================================
-- AI CHAT CONVERSATIONS
-- =============================================

CREATE TABLE AIChatConversations (
    ConversationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AIChatConversations_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
        ON DELETE CASCADE
);

-- =============================================
-- AI CHAT MESSAGES
-- =============================================

CREATE TABLE AIChatMessages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    ConversationId INT NOT NULL,
    Sender NVARCHAR(20) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AIChatMessages_Conversation
        FOREIGN KEY (ConversationId)
        REFERENCES AIChatConversations(ConversationId)
        ON DELETE CASCADE,

    CONSTRAINT CK_AIChatMessages_Sender
        CHECK (Sender IN ('User', 'AI'))
);

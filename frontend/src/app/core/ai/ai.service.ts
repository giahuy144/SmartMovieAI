import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface SentimentResult {
  text: string;
  sentiment: 'Positive' | 'Negative' | 'Neutral';
  confidence: number;
  analyzedAt: string;
}

export interface RecommendationResult {
  userId: number;
  favoriteGenre: string;
  recommendations: Array<{
    id: number;
    title: string;
    genre: string;
    matchScore: number;
    reason: string;
  }>;
}

export interface ChatResponse {
  userMessage: string;
  botReply: string;
  timestamp: string;
}

@Injectable({ providedIn: 'root' })
export class AiApiService {
  private readonly apiBaseUrl = 'http://localhost:5005/api/ai';

  constructor(private readonly http: HttpClient) {}

  analyzeSentiment(text: string): Observable<SentimentResult> {
    return this.http.post<SentimentResult>(`${this.apiBaseUrl}/analyze-sentiment`, { text }).pipe(
      catchError(() => {
        const lower = text.toLowerCase();
        let sentiment: 'Positive' | 'Negative' | 'Neutral' = 'Neutral';
        let confidence = 0.85;

        if (lower.includes('hay') || lower.includes('tuyệt') || lower.includes('đẹp') || lower.includes('thích') || lower.includes('good') || lower.includes('xuất sắc')) {
          sentiment = 'Positive';
          confidence = 0.96;
        } else if (lower.includes('dở') || lower.includes('chán') || lower.includes('tệ') || lower.includes('thất vọng') || lower.includes('bad') || lower.includes('lãng phí')) {
          sentiment = 'Negative';
          confidence = 0.92;
        }

        return of({
          text,
          sentiment,
          confidence,
          analyzedAt: new Date().toISOString()
        });
      })
    );
  }

  getRecommendations(userId = 1, favoriteGenre = 'Sci-Fi'): Observable<RecommendationResult> {
    return this.http.post<RecommendationResult>(`${this.apiBaseUrl}/recommend-movies`, {
      userId,
      favoriteGenre
    }).pipe(
      catchError(() => of({
        userId,
        favoriteGenre,
        recommendations: [
          { id: 1, title: 'Interstellar', genre: 'Sci-Fi', matchScore: 0.98, reason: 'Phù hợp 98% với sở thích du hành vũ trụ & khoa học viễn tưởng của bạn' },
          { id: 2, title: 'Inception', genre: 'Sci-Fi / Action', matchScore: 0.95, reason: 'Cùng đạo diễn Christopher Nolan và kịch bản tầng thứ ấn tượng' },
          { id: 5, title: 'Dune: Part Two', genre: 'Sci-Fi / Adventure', matchScore: 0.93, reason: 'Sử thi điện ảnh nhận được sentiment đánh giá cao ngất ngưởng' }
        ]
      }))
    );
  }

  chat(message: string): Observable<ChatResponse> {
    return this.http.post<ChatResponse>(`${this.apiBaseUrl}/chat`, { message }).pipe(
      catchError(() => {
        const lower = message.toLowerCase();
        let botReply = 'Xin chào! Tôi là Smart Movie AI Assistant. Bạn muốn tôi gợi ý phim theo thể loại, tâm trạng hay thời lượng nào hôm nay?';

        if (lower.includes('hành động') || lower.includes('action')) {
          botReply = 'Gợi ý phim Hành động đỉnh cao: The Dark Knight, Cyber Runner 2077, John Wick và Mad Max: Fury Road!';
        } else if (lower.includes('khoa học') || lower.includes('viễn tưởng') || lower.includes('sci-fi') || lower.includes('vũ trụ')) {
          botReply = 'Nếu bạn mê khoa học viễn tưởng, tuyệt đối đừng bỏ qua: Interstellar, Inception, Dune: Part Two và Avatar!';
        } else if (lower.includes('ngắn') || lower.includes('dưới 2 tiếng')) {
          botReply = 'Các tác phẩm ngắn gọn xúc tích: A Quiet Place (90p), Whiplash (106p), Spider-Man: Spider-Verse (117p)!';
        }

        return of({
          userMessage: message,
          botReply,
          timestamp: new Date().toISOString()
        });
      })
    );
  }
}

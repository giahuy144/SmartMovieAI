import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface Movie {
  id: number;
  title: string;
  description: string;
  poster: string;
  trailer: string;
  releaseDate: string;
  duration: number;
  director: string;
  genre: string;
  averageRating?: number;
  matchScore?: number;
  aiReason?: string;
}

@Injectable({ providedIn: 'root' })
export class MovieApiService {
  private readonly apiBaseUrl = 'http://localhost:5002/api/movies';

  private readonly fallbackMovies: Movie[] = [
    {
      id: 1,
      title: 'Interstellar',
      description: 'Khi Trái Đất dần trở nên không thể sống được, một nhóm nhà hành tinh học du hành qua lỗ sâu ngoài không gian để tìm kiếm ngôi nhà mới cho nhân loại.',
      poster: 'https://images.unsplash.com/photo-1534447677768-be436bb09401?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=zSWdZVtXT7E',
      releaseDate: '2014-11-07',
      duration: 169,
      director: 'Christopher Nolan',
      genre: 'Sci-Fi',
      averageRating: 9.2,
      matchScore: 98,
      aiReason: 'Khớp 98% với sở thích phim Du hành vũ trụ & Khoa học viễn tưởng của bạn'
    },
    {
      id: 2,
      title: 'Inception',
      description: 'Kẻ trộm tài ba Dominic Cobb chuyên đột nhập vào tầng tiềm thức và giấc mơ của người khác để đánh cắp các bí mật kinh doanh quan trọng nhất.',
      poster: 'https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=YoHD9XEInc0',
      releaseDate: '2010-07-16',
      duration: 148,
      director: 'Christopher Nolan',
      genre: 'Sci-Fi / Action',
      averageRating: 8.9,
      matchScore: 95,
      aiReason: 'Cùng đạo diễn Christopher Nolan và phong cách cốt truyện twist đỉnh cao'
    },
    {
      id: 3,
      title: 'The Dark Knight',
      description: 'Batman phải đối đầu với Joker - kẻ phản diện điên loạn đại diện cho sự hỗn mang, đe dọa biến thành phố Gotham thành đống tro tàn.',
      poster: 'https://images.unsplash.com/photo-1509198397868-475647b2a1e5?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=EXeTwQWrcwY',
      releaseDate: '2008-07-18',
      duration: 152,
      director: 'Christopher Nolan',
      genre: 'Action',
      averageRating: 9.0,
      matchScore: 92,
      aiReason: 'Điểm đánh giá siêu cao từ cộng đồng người dùng có gu xem phim tương đồng'
    },
    {
      id: 4,
      title: 'Avatar: The Way of Water',
      description: 'Jake Sully cùng Neytiri và gia đình Na\'vi kiên cường bảo vệ hành tinh Pandora trước cuộc xâm lăng mới với những kỳ quan đại dương ngoạn mục.',
      poster: 'https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=d9MyW72ELq0',
      releaseDate: '2022-12-16',
      duration: 192,
      director: 'James Cameron',
      genre: 'Sci-Fi / Adventure',
      averageRating: 8.7,
      matchScore: 89,
      aiReason: 'Kỹ xảo hình ảnh 3D và thế giới quan sinh thái kỳ ảo'
    },
    {
      id: 5,
      title: 'Dune: Part Two',
      description: 'Paul Atreides liên minh cùng Chani và tộc Fremen để trả thù những kẻ đã hủy diệt gia tộc mình, trong một cuộc chiến sinh tử giữa sa mạc Arrakis.',
      poster: 'https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=Way9Dexny3w',
      releaseDate: '2024-03-01',
      duration: 166,
      director: 'Denis Villeneuve',
      genre: 'Sci-Fi / Adventure',
      averageRating: 9.1,
      matchScore: 96,
      aiReason: 'Tác phẩm sử thi viễn tưởng được AI phân tích có sentiment đánh giá cực kỳ tích cực'
    },
    {
      id: 6,
      title: 'Cyber Runner 2077',
      description: 'Thế giới ngầm công nghệ cao tại siêu đô thị tương lai, nơi một tay lính đánh thuê được cấy ghép chip AI tìm cách sống sót giữa các tập đoàn bóng tối.',
      poster: 'https://images.unsplash.com/photo-1578632767115-351597cf2477?w=800&auto=format&fit=crop',
      trailer: 'https://www.youtube.com/watch?v=qIcTM8WXFjk',
      releaseDate: '2025-06-12',
      duration: 135,
      director: 'Ridley Scott',
      genre: 'Sci-Fi / Action',
      averageRating: 8.8,
      matchScore: 93,
      aiReason: 'Vibe Cyberpunk phong cách neon đêm tương tự thị hiếu của bạn'
    }
  ];

  constructor(private readonly http: HttpClient) {}

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiBaseUrl).pipe(
      catchError(() => of(this.fallbackMovies))
    );
  }

  getMovieById(id: number): Observable<Movie | undefined> {
    return this.http.get<Movie>(`${this.apiBaseUrl}/${id}`).pipe(
      catchError(() => of(this.fallbackMovies.find(m => m.id === id)))
    );
  }

  searchMovies(title?: string, genre?: string): Observable<Movie[]> {
    let url = `${this.apiBaseUrl}/search?`;
    if (title) url += `title=${encodeURIComponent(title)}&`;
    if (genre) url += `genre=${encodeURIComponent(genre)}&`;
    return this.http.get<Movie[]>(url).pipe(
      catchError(() => {
        let list = this.fallbackMovies;
        if (title) {
          list = list.filter(m => m.title.toLowerCase().includes(title.toLowerCase()));
        }
        if (genre && genre !== 'all') {
          list = list.filter(m => m.genre.toLowerCase().includes(genre.toLowerCase()));
        }
        return of(list);
      })
    );
  }
}

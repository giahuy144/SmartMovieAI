import {
  Component,
  OnInit,
  OnDestroy,
  HostListener,
  ViewChild,
  ElementRef,
  AfterViewChecked
} from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AuthService } from '../../core/auth/auth.service';
import { MovieApiService, Movie } from '../../core/movie/movie.service';
import { AiApiService, SentimentResult } from '../../core/ai/ai.service';

interface ChatMessage {
  role: 'user' | 'bot';
  content: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('chatMessages') private chatMessagesEl!: ElementRef;

  private destroy$ = new Subject<void>();
  private searchSubject$ = new Subject<string>();
  private shouldScrollChat = false;

  // State
  isScrolled = false;
  currentUser: { idUser: number; userName: string; role?: string } | null = null;
  isAdmin = false;

  // Auth & Add Movie Modals
  showAuthModal = false;
  authModalTab: 'login' | 'register' = 'login';
  showAddMovieModal = false;

  // Movies
  movies: Movie[] = [];
  filteredMovies: Movie[] = [];
  loadingMovies = true;
  searchQuery = '';
  activeGenre = 'all';
  genres = ['Sci-Fi', 'Action', 'Adventure', 'Drama', 'Animation'];
  selectedMovie: Movie | null = null;
  favorites: Set<number> = new Set();

  // AI Sentiment
  reviewText = '';
  analyzingsentiment = false;
  sentimentResult: SentimentResult | null = null;

  // AI Recommendations
  favoriteGenre = 'Sci-Fi';
  loadingRec = false;
  recommendations: Array<{ id: number; title: string; genre: string; matchScore: number; reason: string }> = [];

  // AI Chat
  chatHistory: ChatMessage[] = [];
  chatInput = '';
  chatTyping = false;

  constructor(
    private readonly authService: AuthService,
    private readonly movieService: MovieApiService,
    private readonly aiService: AiApiService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    // Load user info
    this.refreshCurrentUser();

    // Load movies
    this.movieService.getMovies().pipe(takeUntil(this.destroy$)).subscribe(movies => {
      this.movies = movies;
      this.filteredMovies = movies;
      this.loadingMovies = false;
    });

    // Load AI recommendations
    this.loadRecommendations();

    // Welcome bot message
    this.chatHistory = [{
      role: 'bot',
      content: 'Xin chào! 👋 Tôi là Smart Movie AI Assistant. Tôi có thể gợi ý phim theo thể loại, tâm trạng, hoặc phim tương tự. Bạn muốn xem gì hôm nay?'
    }];

    // Search debounce
    this.searchSubject$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.applyFilter());
  }

  ngAfterViewChecked(): void {
    if (this.shouldScrollChat && this.chatMessagesEl) {
      const el = this.chatMessagesEl.nativeElement as HTMLElement;
      el.scrollTop = el.scrollHeight;
      this.shouldScrollChat = false;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  @HostListener('window:scroll')
  onWindowScroll(): void {
    this.isScrolled = window.scrollY > 20;
  }

  // ===================== Navigation =====================
  scrollTo(id: string): void {
    document.getElementById(id)?.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }

  refreshCurrentUser(): void {
    const stored = localStorage.getItem('user_info');
    if (stored) {
      try {
        this.currentUser = JSON.parse(stored);
      } catch {
        this.currentUser = null;
      }
    } else {
      this.currentUser = null;
    }
    // Phân role chuẩn do Backend trả về trong trường user.role
    const roleLower = (this.currentUser?.role || '').toLowerCase();
    this.isAdmin = roleLower === 'admin';
  }

  logout(): void {
    this.authService.logout();
    this.currentUser = null;
    this.isAdmin = false;
  }

  // ===================== Modal Handlers =====================
  openAuthModal(tab: 'login' | 'register' = 'login'): void {
    this.authModalTab = tab;
    this.showAuthModal = true;
  }

  onAuthSuccess(): void {
    this.refreshCurrentUser();
    this.showAuthModal = false;
  }

  openAddMovieModal(): void {
    this.showAddMovieModal = true;
  }

  onMovieCreated(created: Movie): void {
    if (!this.movies.some(m => m.id === created.id)) {
      this.movies.unshift(created);
    }
    this.applyFilter();
  }

  // ===================== Movie Actions =====================
  onSearch(): void {
    this.searchSubject$.next(this.searchQuery);
  }

  filterGenre(genre: string): void {
    this.activeGenre = genre;
    this.applyFilter();
  }

  private applyFilter(): void {
    let list = [...this.movies];
    if (this.searchQuery.trim()) {
      const q = this.searchQuery.toLowerCase();
      list = list.filter(m =>
        m.title.toLowerCase().includes(q) ||
        m.genre.toLowerCase().includes(q) ||
        m.director.toLowerCase().includes(q)
      );
    }
    if (this.activeGenre !== 'all') {
      list = list.filter(m => m.genre.toLowerCase().includes(this.activeGenre.toLowerCase()));
    }
    this.filteredMovies = list;
  }

  openMovieDetail(movie: Movie): void {
    this.selectedMovie = movie;
  }

  closeModal(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.selectedMovie = null;
    }
  }

  openTrailer(movie: Movie, event: MouseEvent): void {
    event.stopPropagation();
    window.open(movie.trailer, '_blank');
  }

  toggleFavorite(movie: Movie, event: MouseEvent): void {
    event.stopPropagation();
    if (this.favorites.has(movie.id)) {
      this.favorites.delete(movie.id);
    } else {
      this.favorites.add(movie.id);
    }
  }

  isFavorite(movie: Movie): boolean {
    return this.favorites.has(movie.id);
  }

  onImgError(event: Event): void {
    (event.target as HTMLImageElement).src =
      'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=600&auto=format&fit=crop';
  }

  analyzeMovieReview(movie: Movie): void {
    this.selectedMovie = null;
    this.reviewText = `Phim ${movie.title} thật tuyệt vời! Cốt truyện hấp dẫn, hình ảnh xuất sắc.`;
    this.scrollTo('ai-tools');
    setTimeout(() => this.analyzeSentiment(), 400);
  }

  // ===================== AI Sentiment =====================
  analyzeSentiment(): void {
    if (!this.reviewText.trim()) return;
    this.analyzingsentiment = true;
    this.sentimentResult = null;

    this.aiService.analyzeSentiment(this.reviewText)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.sentimentResult = result;
          this.analyzingsentiment = false;
        },
        error: () => { this.analyzingsentiment = false; }
      });
  }

  // ===================== AI Recommendations =====================
  loadRecommendations(): void {
    this.loadingRec = true;
    this.recommendations = [];

    this.aiService.getRecommendations(1, this.favoriteGenre)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          this.recommendations = res.recommendations;
          this.loadingRec = false;
        },
        error: () => { this.loadingRec = false; }
      });
  }

  // ===================== AI Chat =====================
  sendChat(): void {
    const msg = this.chatInput.trim();
    if (!msg || this.chatTyping) return;

    this.chatHistory.push({ role: 'user', content: msg });
    this.chatInput = '';
    this.chatTyping = true;
    this.shouldScrollChat = true;

    this.aiService.chat(msg).pipe(takeUntil(this.destroy$)).subscribe({
      next: res => {
        setTimeout(() => {
          this.chatHistory.push({ role: 'bot', content: res.botReply });
          this.chatTyping = false;
          this.shouldScrollChat = true;
        }, 600);
      },
      error: () => {
        this.chatHistory.push({ role: 'bot', content: 'Xin lỗi, kết nối AI Service gặp sự cố. Hãy thử lại sau!' });
        this.chatTyping = false;
        this.shouldScrollChat = true;
      }
    });
  }

  quickChat(prompt: string): void {
    this.chatInput = prompt;
    this.sendChat();
  }
}

import { Component, EventEmitter, Output } from '@angular/core';
import { MovieApiService, Movie } from '../../../core/movie/movie.service';

@Component({
  selector: 'app-add-movie-modal',
  templateUrl: './add-movie-modal.component.html'
})
export class AddMovieModalComponent {
  @Output() closeModal = new EventEmitter<void>();
  @Output() movieCreated = new EventEmitter<Movie>();

  submitting = false;
  message = '';
  messageType: 'error' | 'success' = 'error';

  newMovie = {
    title: '',
    description: '',
    genre: 'Sci-Fi',
    director: '',
    duration: 120,
    releaseDate: new Date().toISOString().substring(0, 10),
    poster: 'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=800&auto=format&fit=crop',
    trailer: 'https://www.youtube.com',
    aiReason: 'Phim mới được thêm bởi ban quản trị'
  };

  constructor(private readonly movieService: MovieApiService) {}

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeModal.emit();
    }
  }

  submit(): void {
    if (!this.newMovie.title.trim() || !this.newMovie.director.trim()) {
      this.message = 'Vui lòng nhập tên phim và đạo diễn.';
      this.messageType = 'error';
      return;
    }

    this.submitting = true;
    this.message = '';

    this.movieService.createMovie({
      title: this.newMovie.title.trim(),
      description: this.newMovie.description.trim() || 'Chưa có mô tả chi tiết.',
      genre: this.newMovie.genre,
      director: this.newMovie.director.trim(),
      duration: Number(this.newMovie.duration) || 120,
      releaseDate: this.newMovie.releaseDate || new Date().toISOString().substring(0, 10),
      poster: this.newMovie.poster.trim() || 'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=800&auto=format&fit=crop',
      trailer: this.newMovie.trailer.trim() || 'https://www.youtube.com',
      aiReason: this.newMovie.aiReason.trim() || 'Phim mới được thêm bởi quản trị viên'
    }).subscribe({
      next: created => {
        this.submitting = false;
        this.movieCreated.emit(created);
        this.closeModal.emit();
      },
      error: () => {
        this.submitting = false;
        this.messageType = 'error';
        this.message = 'Lỗi kết nối khi thêm phim. Vui lòng thử lại!';
      }
    });
  }
}

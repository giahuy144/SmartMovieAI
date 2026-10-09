import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Movie } from '../../../core/movie/movie.service';

@Component({
  selector: 'app-movie-detail-modal',
  templateUrl: './movie-detail-modal.component.html'
})
export class MovieDetailModalComponent {
  @Input() movie: Movie | null = null;
  @Input() isFavorite = false;

  @Output() closeModal = new EventEmitter<void>();
  @Output() toggleFavorite = new EventEmitter<Movie>();
  @Output() analyzeAi = new EventEmitter<Movie>();

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeModal.emit();
    }
  }

  onImgError(event: Event): void {
    (event.target as HTMLImageElement).src =
      'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=600&auto=format&fit=crop';
  }
}

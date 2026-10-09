import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html'
})
export class FooterComponent {
  @Input() isAdmin = false;
  @Input() totalMovies = 0;

  @Output() navigateSection = new EventEmitter<string>();
  @Output() filterByGenre = new EventEmitter<string>();
  @Output() openAddMovie = new EventEmitter<void>();

  scrollTo(sectionId: string): void {
    this.navigateSection.emit(sectionId);
  }

  onFilterGenre(genre: string): void {
    this.filterByGenre.emit(genre);
    this.scrollTo('movies');
  }
}

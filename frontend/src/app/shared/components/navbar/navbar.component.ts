import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {
  @Input() isScrolled = false;
  @Input() currentUser: { idUser: number; userName: string; role?: string } | null = null;
  @Input() isAdmin = false;
  @Input() searchQuery = '';

  @Output() searchQueryChange = new EventEmitter<string>();
  @Output() searchInput = new EventEmitter<void>();
  @Output() openAuth = new EventEmitter<'login' | 'register'>();
  @Output() openAddMovie = new EventEmitter<void>();
  @Output() logoutUser = new EventEmitter<void>();
  @Output() navigateSection = new EventEmitter<string>();

  onSearchChange(value: string): void {
    this.searchQuery = value;
    this.searchQueryChange.emit(this.searchQuery);
    this.searchInput.emit();
  }

  clearSearch(): void {
    this.onSearchChange('');
  }

  scrollTo(sectionId: string): void {
    this.navigateSection.emit(sectionId);
  }
}

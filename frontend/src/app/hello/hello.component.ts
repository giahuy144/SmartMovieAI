import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-hello',
  templateUrl: './hello.component.html',
  styleUrls: ['./hello.component.css'],
})
export class HelloComponent implements OnInit {
  message = '';

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit() {
    if (!this.auth.isAuthenticated) {
      this.router.navigate(['/']);
      return;
    }

    this.auth.checkAuth().subscribe({
      next: res => (this.message = res.message), // "Hello World"
      error: () => {
        this.auth.logout();
        this.router.navigate(['/']);
      },
    });
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/']);
  }
}

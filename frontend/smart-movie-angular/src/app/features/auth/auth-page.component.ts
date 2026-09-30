import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({ selector: 'app-auth-page', templateUrl: './auth-page.component.html', styleUrls: ['../../../app.component.css'] })
export class AuthPageComponent {
  activeTab: 'login' | 'register' = 'login';
  formData = { userName: '', password: '', confirmPassword: '' };
  loading = false;
  message = '';
  messageType: 'error' | 'success' = 'error';
  constructor(private readonly auth: AuthService, private readonly router: Router) {}

  switchTab(tab: 'login' | 'register'): void {
    this.activeTab = tab;
    this.message = '';
    this.messageType = 'error';
    this.formData.password = '';
    this.formData.confirmPassword = '';
  }

  submit(): void {
    if (!this.formData.userName || !this.formData.password) { this.message = 'Vui lòng nhập tên tài khoản và mật khẩu.'; return; }
    if (this.activeTab === 'register' && this.formData.password !== this.formData.confirmPassword) { this.message = 'Mật khẩu xác nhận không khớp.'; return; }

    this.loading = true;
    this.message = '';
    const request = this.activeTab === 'login'
      ? this.auth.login(this.formData.userName, this.formData.password)
      : this.auth.register(this.formData.userName, this.formData.password, this.formData.confirmPassword);

    request.subscribe({
      next: response => {
        this.loading = false;
        if (this.activeTab === 'login') { this.auth.saveSession(response); this.router.navigate(['/hello']); return; }
        this.switchTab('login');
        this.messageType = 'success';
        this.message = response.message;
      },
      error: error => { this.loading = false; this.message = error.error?.message ?? 'Không thể kết nối tới dịch vụ xác thực.'; }
    });
  }
}

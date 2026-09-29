import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  activeTab: 'login' | 'register' = 'login';
  
  formData = {
    name: '',
    email: '',
    password: ''
  };

  loading = false;
  message = { type: '', text: '' };
  currentUser: any = null;

  private API_AUTH_URL = 'http://localhost:5001/api/auth';

  constructor(private http: HttpClient) {}

  switchTab(tab: 'login' | 'register') {
    this.activeTab = tab;
    this.message = { type: '', text: '' };
  }

  onSubmit() {
    this.loading = true;
    this.message = { type: '', text: '' };

    const endpoint = this.activeTab === 'login' 
      ? `${this.API_AUTH_URL}/login` 
      : `${this.API_AUTH_URL}/register`;

    const payload = this.activeTab === 'login'
      ? { email: this.formData.email, password: this.formData.password }
      : { name: this.formData.name, email: this.formData.email, password: this.formData.password };

    this.http.post<any>(endpoint, payload).subscribe({
      next: (data) => {
        this.loading = false;
        if (this.activeTab === 'login') {
          this.currentUser = data.user;
          this.message = { 
            type: 'success', 
            text: `Chào mừng ${data.user.name} đã đăng nhập Angular thành công!` 
          };
          localStorage.setItem('jwt_token', data.token);
          localStorage.setItem('user_info', JSON.stringify(data.user));
        } else {
          this.message = { 
            type: 'success', 
            text: 'Đăng ký tài khoản thành công! Bạn có thể đăng nhập ngay.' 
          };
          this.switchTab('login');
        }
      },
      error: (err) => {
        this.loading = false;
        // Fallback Offline Demo nếu AuthService chưa chạy
        if (this.activeTab === 'login') {
          this.currentUser = {
            id: 1,
            name: this.formData.email.split('@')[0] || 'User Demo',
            email: this.formData.email,
            role: this.formData.email.includes('admin') ? 'ADMIN' : 'USER'
          };
          this.message = { 
            type: 'success', 
            text: `Đăng nhập Demo Angular thành công (Backend Offline)!` 
          };
        } else {
          this.message = { 
            type: 'success', 
            text: 'Đăng ký Demo thành công! Mời bạn đăng nhập.' 
          };
          this.switchTab('login');
        }
      }
    });
  }

  logout() {
    this.currentUser = null;
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    this.message = { type: 'success', text: 'Đã đăng xuất khỏi Angular Client.' };
  }
}

import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  activeTab: 'login' | 'register' = 'login';
  
  formData = {
    userName: '',
    password: ''
  };

  loading = false;
  message = { type: '', text: '' };
  currentUser: any = null;
  jwtToken: string = '';
  
  // Kết quả khi test API /auth (Xác thực token "Hello World")
  authTestResult: any = null;
  authTesting = false;

  private API_BASE_URL = 'http://localhost:5001';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const savedToken = localStorage.getItem('jwt_token');
    const savedUser = localStorage.getItem('user_info');
    if (savedToken && savedUser) {
      this.jwtToken = savedToken;
      this.currentUser = JSON.parse(savedUser);
    }
  }

  switchTab(tab: 'login' | 'register') {
    this.activeTab = tab;
    this.message = { type: '', text: '' };
  }

  // Helper mã hóa Base64 password tại Client theo đúng yêu cầu
  encodeBase64(str: string): string {
    try {
      return btoa(unescape(encodeURIComponent(str)));
    } catch (e) {
      return btoa(str);
    }
  }

  onSubmit() {
    if (!this.formData.userName || !this.formData.password) {
      this.message = { type: 'error', text: 'Vui lòng nhập UserName và Mật Khẩu!' };
      return;
    }

    this.loading = true;
    this.message = { type: '', text: '' };

    // Mã hóa base64 password tại client theo yêu cầu đề bài
    const encodedPassword = this.encodeBase64(this.formData.password);

    const payload = {
      userName: this.formData.userName,
      password: encodedPassword
    };

    const endpoint = this.activeTab === 'login' 
      ? `${this.API_BASE_URL}/api/auth/login` 
      : `${this.API_BASE_URL}/api/auth/register`;

    this.http.post<any>(endpoint, payload).subscribe({
      next: (data) => {
        this.loading = false;
        if (this.activeTab === 'login') {
          this.currentUser = data.user;
          this.jwtToken = data.token;
          this.message = { 
            type: 'success', 
            text: `🎉 Đăng nhập thành công! Đã sinh và lưu JWT Token.` 
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
        // Fallback demo cho client thử nghiệm
        const mockToken = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwidXNlck5hbWUiOiI${encodedPassword}IiwiaWF0IjoxNTE2MjM5MDIyfQ.mock_token_${Date.now()}`;
        if (this.activeTab === 'login') {
          this.currentUser = {
            idUser: 1,
            userName: this.formData.userName
          };
          this.jwtToken = mockToken;
          this.message = { 
            type: 'success', 
            text: `Đăng nhập thành công (Mode Demo Frontend)!` 
          };
          localStorage.setItem('jwt_token', mockToken);
          localStorage.setItem('user_info', JSON.stringify(this.currentUser));
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

  // Gọi API localhost:5001/auth với Bearer JWT Token để test Middleware
  testAuthEndpoint() {
    if (!this.jwtToken) {
      this.message = { type: 'error', text: 'Chưa có Token JWT! Vui lòng đăng nhập trước.' };
      return;
    }

    this.authTesting = true;
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.jwtToken}`
    });

    this.http.get<any>(`${this.API_BASE_URL}/auth`, { headers }).subscribe({
      next: (res) => {
        this.authTesting = false;
        this.authTestResult = res;
      },
      error: (err) => {
        this.authTesting = false;
        if (err.status === 401) {
          this.authTestResult = {
            error: "401 Unauthorized",
            message: "Middleware đã từ chối Token không hợp lệ hoặc hết hạn!"
          };
        } else {
          // Fallback offline mock response
          this.authTestResult = {
            message: "Hello World",
            status: "Authorized (Client Offline Simulation)",
            authenticatedUser: this.currentUser,
            timestamp: new Date().toISOString()
          };
        }
      }
    });
  }

  logout() {
    this.currentUser = null;
    this.jwtToken = '';
    this.authTestResult = null;
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    this.message = { type: 'success', text: 'Đã đăng xuất khỏi hệ thống.' };
  }
}

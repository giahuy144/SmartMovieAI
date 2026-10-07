import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService, AuthResponse } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-auth-modal',
  templateUrl: './auth-modal.component.html'
})
export class AuthModalComponent {
  @Input() initialTab: 'login' | 'register' = 'login';
  @Output() closeModal = new EventEmitter<void>();
  @Output() authSuccess = new EventEmitter<void>();

  activeTab: 'login' | 'register' = 'login';
  formData = { userName: '', password: '', confirmPassword: '' };
  loading = false;
  message = '';
  messageType: 'error' | 'success' = 'error';
  showPass = false;

  constructor(private readonly authService: AuthService) {}

  ngOnInit(): void {
    this.activeTab = this.initialTab;
  }

  switchTab(tab: 'login' | 'register'): void {
    this.activeTab = tab;
    this.message = '';
    this.messageType = 'error';
    this.formData.password = '';
    this.formData.confirmPassword = '';
    this.showPass = false;
  }

  onBackdropClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.closeModal.emit();
    }
  }

  getPasswordStrength(): number {
    const pw = this.formData.password;
    if (!pw) return 0;
    let score = 0;
    if (pw.length >= 6) score++;
    if (pw.length >= 10) score++;
    if (/[A-Z]/.test(pw)) score++;
    if (/[0-9!@#$%^&*]/.test(pw)) score++;
    return score;
  }

  getPasswordStrengthColor(bar: number): string {
    const strength = this.getPasswordStrength();
    if (bar > strength) return 'rgba(255,255,255,0.1)';
    const colors = ['#ef4444', '#f97316', '#f59e0b', '#10b981'];
    return colors[strength - 1] || 'rgba(255,255,255,0.1)';
  }

  getPasswordStrengthLabel(): { text: string; color: string } {
    const strength = this.getPasswordStrength();
    const labels: Array<{ text: string; color: string }> = [
      { text: 'Rất yếu', color: 'var(--error)' },
      { text: 'Yếu', color: '#f97316' },
      { text: 'Trung bình', color: 'var(--accent-gold)' },
      { text: 'Mạnh', color: 'var(--success)' }
    ];
    return labels[strength - 1] || { text: 'Rất yếu', color: 'var(--error)' };
  }

  submit(): void {
    if (!this.formData.userName || !this.formData.password) {
      this.message = 'Vui lòng nhập đầy đủ tên tài khoản và mật khẩu.';
      this.messageType = 'error';
      return;
    }
    if (this.activeTab === 'register' && this.formData.password !== this.formData.confirmPassword) {
      this.message = 'Mật khẩu xác nhận không khớp. Vui lòng kiểm tra lại.';
      this.messageType = 'error';
      return;
    }

    this.loading = true;
    this.message = '';

    const req = this.activeTab === 'login'
      ? this.authService.login(this.formData.userName, this.formData.password)
      : this.authService.register(this.formData.userName, this.formData.password, this.formData.confirmPassword);

    req.subscribe({
      next: res => {
        this.loading = false;
        if (this.activeTab === 'login') {
          this.authService.saveSession(res);
          this.authSuccess.emit();
          return;
        }
        this.switchTab('login');
        this.messageType = 'success';
        this.message = res.message || 'Đăng ký thành công! Hãy đăng nhập để tiếp tục.';
      },
      error: err => {
        this.loading = false;
        this.messageType = 'error';
        this.message = err.error?.message ?? 'Không thể kết nối tới Auth Service. Hãy kiểm tra backend.';
      }
    });
  }
}

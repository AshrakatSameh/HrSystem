export interface LoginDto {
  username: string;
  password: string;
}

export interface LoginResponseDto {
  token: string;
  userName: string;
  fullName: string;
  roles: string[];
}

export interface AuthState {
  token: string | null;
  user: LoginResponseDto | null;
  isAuthenticated: boolean;
  roles: string[];
}

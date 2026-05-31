export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  phoneNumber: string;
  username: string;
  password: string;
  birthDate: string;     // ISO date
  gender: 'M' | 'F' | 'O';
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  user: {
    id: string;
    fullName: string;
    email: string;
    username: string;
  };
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  id: number;
  fullName: string;
  email: string;
  accessToken: string;
  refreshToken: string;
  accessTokenExpTime: string;
  refreshTokenExpTime: string;
}

export interface RegisterRequest {
  email: string;
  firstName: string;
  lastName: string;
  password: string;
}

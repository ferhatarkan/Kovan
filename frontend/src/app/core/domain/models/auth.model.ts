/**
 * Backend'deki Kovan.Application.Features.Auth modellerinin karşılığıdır.
 */

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword?: string;
  tenantName?: string;
}

/**
 * Backend'deki LoginResponseDto karşılığıdır.
 */
export interface AuthResponse {
  token: string | null;
  refreshToken: string | null;
  is2faRequired: boolean;
  message: string | null;
}
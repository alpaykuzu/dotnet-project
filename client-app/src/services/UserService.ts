import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from "../types/user";

export class UserService {
  static async login(data: LoginRequest): Promise<LoginResponse> {
    const response = await fetch("https://localhost:7197/api/Users/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const err = await response.json();
      throw new Error(err.message || "Giriş başarısız");
    }

    const result = await response.json();
    return result.data;
  }

  static async register(data: RegisterRequest): Promise<void> {
    const response = await fetch("https://localhost:7197/api/Users/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const err = await response.json();
      throw new Error(err.message || "Kayıt başarısız");
    }
  }
}

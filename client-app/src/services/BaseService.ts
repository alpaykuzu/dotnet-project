import type { ApiFormat } from "../types/api";
import Cookies from "js-cookie";

export class BaseService {
  static async request<T>(
    url: string,
    options: RequestInit = {}
  ): Promise<ApiFormat<T>> {
    const accessToken = Cookies.get("accessToken");
    //const accessToken = localStorage.getItem("accessToken") ?? "";
    //const refreshToken = localStorage.getItem("refreshToken") ?? "";

    const headers = new Headers({
      ...(options.headers instanceof Headers
        ? Object.fromEntries(options.headers.entries())
        : options.headers),
      Authorization: `Bearer ${accessToken}`,
      "Content-Type": "application/json",
    });

    const response = await fetch(url, { ...options, headers });

    /*if (response.status === 401 && refreshToken) {
      const refreshResponse = await fetch(
        "https://localhost:7197/api/Users/refresh-token",
        {
          method: "POST",
          headers: new Headers({ "Content-Type": "application/json" }),
          body: JSON.stringify({ refreshToken }),
        }
      );

      if (!refreshResponse.ok) {
        localStorage.clear();
        window.location.href = "/";
        throw new Error("Oturum süresi doldu");
      }

      const refreshData = await refreshResponse.json();
      accessToken = refreshData.data.accessToken;
      console.log(refreshData);
      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", refreshData.data.refreshToken);

      headers.set("Authorization", `Bearer ${accessToken}`);
      response = await fetch(url, { ...options, headers });
    }*/

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Bir hata oluştu");
    }

    return response.json();
  }
}

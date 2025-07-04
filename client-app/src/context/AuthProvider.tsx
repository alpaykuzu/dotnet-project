import React, { createContext, useContext, useEffect, useState } from "react";
import type { LoginResponse } from "../types/user";
import Cookies from "js-cookie";

interface AuthContextType {
  accessToken: string | null;
  refreshToken: string | null;
  login: (data: LoginResponse) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [accessToken, setAccessToken] = useState(
    //localStorage.getItem("accessToken")
    Cookies.get("accessToken")
  );
  const [refreshToken, setRefreshToken] = useState(
    //localStorage.getItem("refreshToken")
    Cookies.get("refreshToken")
  );

  const login = (data: LoginResponse) => {
    setAccessToken(data.accessToken);
    setRefreshToken(data.refreshToken);
    //localStorage.setItem("accessToken", data.token);
    //localStorage.setItem("refreshToken", data.refreshToken);
    const accessExp = new Date(data.accessTokenExpTime);
    const refreshExp = new Date(data.refreshTokenExpTime);

    Cookies.set("accessToken", data.accessToken, {
      expires: accessExp,
    });
    Cookies.set("refreshToken", data.refreshToken, {
      expires: refreshExp,
    });
  };

  const logout = () => {
    setAccessToken("");
    setRefreshToken("");
    //localStorage.clear();
    Object.keys(Cookies.get()).forEach((cookie) => {
      Cookies.remove(cookie);
    });
    window.location.href = "/";
  };

  useEffect(() => {
    const id = setInterval(async () => {
      const access = Cookies.get("accessToken");
      const refresh = Cookies.get("refreshToken");
      const currentPath = window.location.pathname;
      if (!refresh) {
        if (!["/login", "/register"].includes(currentPath)) {
          logout();
        }
        return;
      }

      if (!access && ["/login", "/register"].includes(currentPath)) {
        return;
      }

      if (!access) {
        try {
          const response = await fetch(
            "https://localhost:7197/api/Users/refresh-token",
            {
              method: "POST",
              headers: { "Content-Type": "application/json" },
              body: JSON.stringify({ refreshToken: refresh }),
            }
          );

          if (!response.ok) {
            logout();
            return;
          }

          const result = await response.json();

          setAccessToken(result.data.accessToken);
          setRefreshToken(result.data.refreshToken);

          Cookies.set("accessToken", result.data.accessToken, {
            expires: new Date(result.data.accessTokenExpTime),
          });
          Cookies.set("refreshToken", result.data.refreshToken, {
            expires: new Date(result.data.refreshTokenExpTime),
          });
        } catch {
          logout();
        }
      }
    }, 500);

    return () => clearInterval(id);
  }, []);

  return (
    <AuthContext.Provider
      value={{
        accessToken: accessToken ?? null,
        refreshToken: refreshToken ?? null,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within an AuthProvider");
  return context;
};

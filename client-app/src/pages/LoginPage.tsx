import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserService } from "../services/UserService";
import { useAuth } from "../context/AuthProvider";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    try {
      const data = await UserService.login({ email, password });
      login(data);
      navigate("/products");
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message);
    }
  };

  const register = () => {
    navigate("/register");
  };

  return (
    <div>
      <form onSubmit={handleLogin}>
        <h2>Giriş Yap</h2>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <input
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          required
        />
        <input
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Şifre"
          type="password"
          required
        />
        <button type="submit">Giriş</button>
      </form>
      <button onClick={register}>Hesabın yoksa kayıt ol.</button>
    </div>
  );
}

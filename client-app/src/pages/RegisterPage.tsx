import { useState } from "react";
import { UserService } from "../services/UserService";
import { useNavigate } from "react-router-dom";

export function RegisterPage() {
  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const register = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    try {
      await UserService.register({
        email,
        firstName,
        lastName,
        password,
      });
      navigate("/login");

      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      setError(error.message);
    }
  };
  return (
    <div>
      <form onSubmit={register}>
        <h2>Kayıt Ol</h2>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <input
          value={email}
          type="email"
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Eposta"
          required
        />
        <input
          value={firstName}
          type="text"
          onChange={(e) => setFirstName(e.target.value)}
          placeholder="İsim"
          required
        />
        <input
          value={lastName}
          type="text"
          onChange={(e) => setLastName(e.target.value)}
          placeholder="Soyisim"
          required
        />
        <input
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Şifre"
          type="password"
          required
        />
        <button type="submit">Kayıt Ol</button>
      </form>
    </div>
  );
}

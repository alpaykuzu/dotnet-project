import { useState } from "react";
import { ProductService } from "../services/ProductService";
import { useAuth } from "../context/AuthProvider";
import type { ProductResponse } from "../types/product";

export function ProductPage() {
  const [product, setProduct] = useState<ProductResponse | null>(null);
  const [error, setError] = useState("");

  const [productName, setProductName] = useState("");
  const [productDescription, setProductDescription] = useState("");
  const [productPrice, setProductPrice] = useState(0);
  const [productQuantity, setProductQuantity] = useState(0);
  const [isProductAdded, setIsProductAdded] = useState("");

  const [productId, setProductId] = useState(0);
  const [isProductDeleted, setIsProductDeleted] = useState("");

  const [isProductUpdated, setIsProductUpdated] = useState("");

  const fetchProduct = async () => {
    setError("");
    try {
      const result = await ProductService.getProduct(4);
      setProduct(result);
      console.log(result);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message);
    }
  };

  const addProduct = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    try {
      const result = await ProductService.addProduct({
        productName,
        productDescription,
        productPrice,
        productQuantity,
      });
      setIsProductAdded(result);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message);
    }
  };

  const deleteProduct = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    try {
      const result = await ProductService.deleteProduct(productId);
      setIsProductDeleted(result);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message);
    }
  };

  const updateProduct = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    try {
      const result = await ProductService.updateProduct(productId, {
        productName,
        productDescription,
        productPrice,
        productQuantity,
      });
      setIsProductUpdated(result);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message);
    }
  };

  const { logout } = useAuth();

  return (
    <div>
      <button onClick={fetchProduct}>Ürünü Göster</button>
      {error && <p style={{ color: "red" }}>{error}</p>}
      {product && (
        <div>
          <h3>Ürün ismi: {product.productName}</h3>
          <p>Ürün açıklaması: {product.productDescription}</p>
          <p>Ürün Fiyatı: {product.productPrice}₺</p>
          <p>Ürün Miktarı: {product.productQuantity}</p>
        </div>
      )}
      <form onSubmit={addProduct}>
        <input
          type="text"
          placeholder="Ürün İsmi"
          //value={productName}
          required
          onChange={(e) => setProductName(e.target.value)}
        />
        <input
          type="text"
          placeholder="Ürün Açıklaması"
          //value={productDescription}
          required
          onChange={(e) => setProductDescription(e.target.value)}
        />
        <input
          type="number"
          placeholder="Ürün Fiyatı"
          //value={productPrice}
          required
          onChange={(e) => setProductPrice(parseInt(e.target.value))}
        />
        <input
          type="number"
          placeholder="Ürün Adedi"
          //value={productQuantity}
          required
          onChange={(e) => setProductQuantity(parseInt(e.target.value))}
        />
        <button type="submit">Ürün Ekle</button>
        {isProductAdded && <p style={{ color: "orange" }}>{isProductAdded}</p>}
      </form>
      <form onSubmit={deleteProduct}>
        <input
          type="number"
          placeholder="Ürün Id"
          required
          //value={productId}
          onChange={(e) => setProductId(parseInt(e.target.value))}
        />
        <button type="submit">Ürünü Sil</button>
        {isProductDeleted && (
          <p style={{ color: "orange" }}>{isProductDeleted}</p>
        )}
      </form>
      <form onSubmit={updateProduct}>
        <input
          type="text"
          placeholder="Ürün Id"
          //value={productId}
          required
          onChange={(e) => setProductId(parseInt(e.target.value))}
        />
        <input
          type="text"
          placeholder="Ürün İsmi"
          //value={productName}
          required
          onChange={(e) => setProductName(e.target.value)}
        />
        <input
          type="text"
          placeholder="Ürün Açıklaması"
          //value={productDescription}
          required
          onChange={(e) => setProductDescription(e.target.value)}
        />
        <input
          type="number"
          placeholder="Ürün Fiyatı"
          //value={productPrice}
          required
          onChange={(e) => setProductPrice(parseInt(e.target.value))}
        />
        <input
          type="number"
          placeholder="Ürün Adedi"
          //value={productQuantity}
          required
          onChange={(e) => setProductQuantity(parseInt(e.target.value))}
        />
        <button type="submit">Ürünü Güncelle</button>
        {isProductUpdated && (
          <p style={{ color: "orange" }}>{isProductUpdated}</p>
        )}
      </form>
      <button onClick={logout}>Çıkış Yap</button>
    </div>
  );
}

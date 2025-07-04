import type { ApiFormat } from "../types/api";
import type { ProductResponse } from "../types/product";
import { BaseService } from "./BaseService";

export class ProductService {
  static async getProduct(id: number): Promise<ProductResponse> {
    const res: ApiFormat<ProductResponse> =
      await BaseService.request<ProductResponse>(
        `https://localhost:7197/api/Products/get-product?productId=${id}`
      );
    return res.data;
  }

  static async addProduct(payload: {
    productName: string;
    productDescription: string;
    productPrice: number;
    productQuantity: number;
  }): Promise<string> {
    const res: ApiFormat<void> = await BaseService.request<void>(
      "https://localhost:7197/api/Products/add-product",
      {
        method: "POST",
        body: JSON.stringify(payload),
      }
    );
    return res.message;
  }

  static async deleteProduct(id: number): Promise<string> {
    const res: ApiFormat<void> = await BaseService.request<void>(
      `https://localhost:7197/api/Products/delete-product?productId=${id}`,
      {
        method: "DELETE",
      }
    );
    return res.message;
  }

  static async updateProduct(
    id: number,
    payload: {
      productName: string;
      productDescription: string;
      productPrice: number;
      productQuantity: number;
    }
  ): Promise<string> {
    const res: ApiFormat<void> = await BaseService.request<void>(
      `https://localhost:7197/api/Products/update-product?productId=${id}`,
      {
        method: "PUT",
        body: JSON.stringify(payload),
      }
    );
    return res.message;
  }
}

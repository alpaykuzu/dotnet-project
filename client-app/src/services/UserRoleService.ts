import type { UserRoleRequest } from "./../types/userRole";
import type { ApiFormat } from "../types/api";
import { BaseService } from "./BaseService";

export class UserRoleService {
  static async updateUserRole(
    userRoleRequest: UserRoleRequest
  ): Promise<string> {
    const res: ApiFormat<void> = await BaseService.request<void>(
      `https://localhost:7197/api/UserRole/update-user-role`,
      {
        method: "PUT",
        body: JSON.stringify(userRoleRequest),
      }
    );
    return res.message;
  }
}

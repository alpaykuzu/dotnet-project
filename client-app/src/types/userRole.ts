export interface UserRoleRequest {
  role: string;
  userId: number;
}

export interface UserRoleResponse {
  id: number;
  role: string;
  userId: number;
}

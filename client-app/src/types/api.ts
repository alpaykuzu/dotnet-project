export interface ApiFormat<T> {
  data: T;
  statusCode: number;
  isSuccessful: boolean;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  error: any;
  message: string;
}

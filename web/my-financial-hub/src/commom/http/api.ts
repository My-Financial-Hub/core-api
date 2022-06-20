import { ServiceResult } from '../interfaces/service-result';

export default class Api<T>
{
  private readonly _baseUrl: string;
  private readonly _baseEndpoint: string;

  private get _apiUrl(): string {
    return `${this._baseUrl}/${this._baseEndpoint}`;
  }

  constructor(baseUrl: string, baseEndpoint: string) {
    this._baseUrl = baseUrl;
    this._baseEndpoint = baseEndpoint;
  }

  async GetAllAsync(): Promise<ServiceResult<T[]>> {
    const result = await fetch(this._apiUrl);
    const json = await result.json();

    if (!result.ok) {
      throw json;//TODO:
    }

    return json as ServiceResult<T[]>;
  }

  async PostAsync(body: T): Promise<ServiceResult<T>> {
    const result = await fetch(this._apiUrl,
      {
        method: 'POST',
        headers: {
          'content-type': 'application/json'
        },
        body: JSON.stringify(body)
      }
    );
    const json = await result.json();

    return json as ServiceResult<T>;
  }

  async PutAsync(id: string, body: T): Promise<ServiceResult<T>> {
    try {
      const result = await fetch(`${this._apiUrl}/${id}`,
        {
          method: 'PUT',
          headers: {
            'content-type': 'application/json'
          },
          body: JSON.stringify(body)
        }
      );
      const json = await result.json();

      return json as ServiceResult<T>;
    } catch (e) {
      return {
        hasError: true,
        error: {
          message: 'Internal Error'
        }
      } as ServiceResult<T>;
    }
  }

  async DeleteAsync(id: string): Promise<boolean> {
    const result = await fetch(`${this._apiUrl}/${id}`, { method: 'DELETE' });

    if (!result.ok) {
      throw false;
    }

    return true;
  }
}
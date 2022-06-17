export default class Api<T>
{
  private readonly _baseUrl: string;
  private readonly _baseEndpoint: string;

  private get _apiUrl() : string {
    return `${this._baseUrl}/${this._baseEndpoint}`;
  }  

  constructor(baseUrl: string,baseEndpoint: string) {
    this._baseUrl       = baseUrl;
    this._baseEndpoint  = baseEndpoint;
  }

  async GetAllAsync(): Promise<T[]> {
    const result = await fetch(this._apiUrl);
    const json = await result.json();

    if (!result.ok) {
      throw json;//TODO:
    }

    return json as T[];
  }

  async PostAsync(body: T): Promise<T> {
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

    if (!result.ok) {
      throw json;//TODO:
    }

    return json as T;
  }

  async PutAsync(id: string,body: T): Promise<T> {
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

    if (!result.ok) {
      throw json;//TODO:
    }

    console.log(json);

    return json as T;
  }

  async DeleteAsync(id: string): Promise<boolean>{
    const result = await fetch(`${this._apiUrl}/${id}`,{ method: 'DELETE' });

    if (!result.ok) {
      throw false;
    }

    return true;
  }
}
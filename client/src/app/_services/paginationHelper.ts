import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";

export function getPaginationResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  export function getPaginationHeaders(pageNumber: Number, itemsPerPage: Number) {
    let params = new HttpParams();

    if (pageNumber !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return params;
  }
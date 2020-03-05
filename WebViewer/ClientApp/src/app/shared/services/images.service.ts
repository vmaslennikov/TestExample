import { ImageFile } from './../models/image-file';
import { Folder } from './../models/folder';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
  })
};

@Injectable({
  providedIn: 'root'
})
export class ImagesService {
  config = {
    foldersUrl: '/api/images/folders',
    filesUrl: '/api/images/files'
  };

  constructor(private http: HttpClient) {}

  getFolders(f: string): Observable<Folder[]> {
    if (f) {
      return this.http.get<Folder[]>(
        [this.config.foldersUrl, '?folder=', f].join(''),
        httpOptions
      );
    }
    return this.http.get<Folder[]>(this.config.foldersUrl, httpOptions);
  }

  getFiles(f: string): Observable<ImageFile[]> {
    if (f) {
      return this.http.get<ImageFile[]>(
        [this.config.filesUrl, '?folder=', f].join(''),
        httpOptions
      );
    }
    return this.http.get<ImageFile[]>(this.config.filesUrl, httpOptions);
  }
}

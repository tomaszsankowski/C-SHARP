import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { UserLogin } from './user-login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isLoggedIn: boolean = false;

  // This is the URL of the API that we want to call
  url:string = environment.apiBaseUrl + '/Users';

  formData: UserLogin = new UserLogin();

  constructor(private http: HttpClient) { }

  login(){
    return this.http.post(this.url + '/Login', this.formData)
  }
}
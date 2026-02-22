import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http"
import { environment } from '../../environments/environment.development';
import { UserDetails } from './user-details.model';
import { NgForm } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {

  // This is the URL of the API that we want to call
  url:string = environment.apiBaseUrl + '/Users';

  // List of Users
  list: UserDetails[] = [];

  // Data from form
  formData: UserDetails = new UserDetails();
  formSubmitted: boolean = false;

  // Constructor
  constructor(private http: HttpClient) {}

  // Function that displays all the users
  refreshList(){
    this.http.get(this.url)
    .subscribe({
      next: res => {
        this.list = res as UserDetails[];
      },
      error: err => {
        console.log(err);
      }
    })
  }

  // Function that creates new user
  postUser(){
    return this.http.post(this.url, this.formData)
  }

  putUser(){
    return this.http.put(this.url + '/' + this.formData.id, this.formData)
  }

  deleteUser(id:number){
    return this.http.delete(this.url + '/' + id)
  }

  resetForm(form:NgForm){
    form.form.reset()
    this.formData = new UserDetails()
    this.formSubmitted = true;
  }
}

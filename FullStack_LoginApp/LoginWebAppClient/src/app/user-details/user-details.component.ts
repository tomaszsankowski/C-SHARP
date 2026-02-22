import { Component, OnInit } from '@angular/core';
import { UserDetailsFormComponent } from './user-details-form/user-details-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { UserDetailsService } from '../shared/user-details.service';
import { AuthService } from '../shared/auth.service';
import { CommonModule } from '@angular/common';
import { UserDetails } from '../shared/user-details.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [UserDetailsFormComponent, LoginFormComponent, CommonModule],
  templateUrl: './user-details.component.html',
  styles: ``
})
export class UserDetailsComponent implements OnInit{

  constructor(public service: UserDetailsService, public authService: AuthService, private toastr:ToastrService) {

  }
  ngOnInit(): void {
    this.service.refreshList();
  }

  populateForm(selectedRecord: UserDetails){
    this.service.formData = Object.assign({}, selectedRecord);
  }

  onDelete(id:number){
    if(confirm('Are you sure to delete this record?'))
    {
      this.service.deleteUser(id)
      .subscribe({
        next: res => {
          this.service.refreshList();
          this.toastr.error('Deleted successfully', 'User Register')
        },
        error: err => {
          console.log(err);
        }
      })
    }
  }
}
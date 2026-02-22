import { Component } from '@angular/core';
import { UserDetailsService } from '../../shared/user-details.service';
import { FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { UserDetails } from '../../shared/user-details.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-details-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './user-details-form.component.html',
  styles: ``
})
export class UserDetailsFormComponent {
  constructor(public service: UserDetailsService, private toastr:ToastrService) {

  }

  onSubmit(form:NgForm) {
    this.service.formSubmitted = true;

    // Check if the form is valid
    if(form.valid == false)
    {
      return
    }

    if(this.service.formData.id == 0)
    {
      this.insertRecord(form)
    }
    else
    {
      this.updateRecord(form)
    }
  }

  insertRecord(form:NgForm) {
    // Call the postUser function from the service if form is valid
    this.service.postUser()
      .subscribe({
        next: res => {
          this.service.list = res as UserDetails[];
          this.service.resetForm(form);
          this.toastr.success('Inserted successfullly', 'User Register')
        },
        error: err => {
          console.log(err);
        }
      })
  }

  updateRecord(form:NgForm) {
    this.service.putUser()
      .subscribe({
        next: res => {
          this.service.list = res as UserDetails[];
          this.service.resetForm(form);
          this.toastr.info('Updated successfullly', 'User Register')
        },
        error: err => {
          console.log(err);
        }
      })
  }

}

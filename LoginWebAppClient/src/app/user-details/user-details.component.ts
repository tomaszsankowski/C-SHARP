import { Component } from '@angular/core';
import { UserDetailsFormComponent } from './user-details-form/user-details-form.component';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [UserDetailsFormComponent],
  templateUrl: './user-details.component.html',
  styles: ``
})
export class UserDetailsComponent {

}
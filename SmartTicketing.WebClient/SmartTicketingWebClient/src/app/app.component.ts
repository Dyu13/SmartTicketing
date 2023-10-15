import { Component, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  IsLoginComponent: boolean = false;

  componentAdded($event: EventEmitter<any>) {
    this.IsLoginComponent = $event.constructor.name == "LoginComponent";
  }
}

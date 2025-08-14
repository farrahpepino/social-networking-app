import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';

@Component({
  selector: 'app-home',
  imports: [NavigationComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  showPost = false;

  viewPost(){
    this.showPost = true;
  }
  hidePost(){
    this.showPost = false;
  }

  



}

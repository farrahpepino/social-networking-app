import { Routes } from '@angular/router';
import { HeroComponent } from './components/main/hero/hero.component';
import { HomeComponent } from './components/main/home/home.component';

export const routes: Routes = [
    {
        path: 'auth',
        component: HomeComponent
    },
    {
        path: '',
        component: HeroComponent
    }
];

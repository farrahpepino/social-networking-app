import { Routes } from '@angular/router';
import { HeroComponent } from './components/main/hero/hero.component';
import { HomeComponent } from './components/main/home/home.component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    },
    {
        path: 'auth',
        component: HeroComponent
    }
];

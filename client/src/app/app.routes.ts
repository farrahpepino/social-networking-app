import { Routes } from '@angular/router';
import { HeroComponent } from './components/main/hero/hero.component';
import { HomeComponent } from './components/main/home/home.component';
import { authGuard } from './guards/auth.guard';
import { guestGuard } from './guards/guest.guard';
export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
        canActivate: [authGuard]
    },
    {
        path: '',
        component: HeroComponent,
        canActivate: [guestGuard]
    },
];

import { Routes } from '@angular/router';
import { HeroComponent } from './components/main/hero/hero.component';
import { HomeComponent } from './components/main/home/home.component';
import { ProfileComponent } from './components/main/profile/profile.component';
import { authGuard } from './guards/auth.guard';
import { guestGuard } from './guards/guest.guard';
import { ExternalProfileComponent } from './components/main/external-profile/external-profile.component';

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
    {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [authGuard]
    },
    {
        path: 'profile/:username',
        component: ExternalProfileComponent,
        canActivate: [authGuard]
      }

];

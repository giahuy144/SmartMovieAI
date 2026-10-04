import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthPageComponent } from './features/auth/auth-page.component';
import { HomeComponent } from './features/home/home.component';

const routes: Routes = [
  { path: '', component: AuthPageComponent, pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

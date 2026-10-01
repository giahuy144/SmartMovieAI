import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HelloComponent } from './hello/hello.component';
import { AuthPageComponent } from './features/auth/auth-page.component';

const routes: Routes = [
  { path: '', component: AuthPageComponent, pathMatch: 'full' },
  { path: 'hello', component: HelloComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

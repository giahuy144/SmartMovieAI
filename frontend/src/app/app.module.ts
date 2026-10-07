import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthPageComponent } from './features/auth/auth-page.component';
import { HomeComponent } from './features/home/home.component';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { AuthModalComponent } from './shared/components/auth-modal/auth-modal.component';
import { AddMovieModalComponent } from './shared/components/add-movie-modal/add-movie-modal.component';
import { MovieDetailModalComponent } from './shared/components/movie-detail-modal/movie-detail-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthPageComponent,
    HomeComponent,
    NavbarComponent,
    FooterComponent,
    AuthModalComponent,
    AddMovieModalComponent,
    MovieDetailModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

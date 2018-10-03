import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AlaskaUiCacheModule } from 'alaska-ui';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AlaskaUiCacheModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

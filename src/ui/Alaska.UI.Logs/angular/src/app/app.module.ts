import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AlaskaUiLogsModule } from 'alaska-ui';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
	AlaskaUiLogsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

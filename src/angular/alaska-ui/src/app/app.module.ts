import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import {
  AlaskaUiCacheModule,
  CACHE_API_BASE_URL,
  AlaskaUiLogsModule,
  DEFAULT_LOGS_HUB_URL
} from 'alaska-ui';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AlaskaUiCacheModule,
    AlaskaUiLogsModule
  ],
  providers: [
    [{provide: CACHE_API_BASE_URL, useValue: 'https://localhost:44345'}],
    [{provide: DEFAULT_LOGS_HUB_URL, useValue: 'http://localhost:5000/hub/logs'}],
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}

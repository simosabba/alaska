import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { AlaskaUiCoreModule } from '../alaska-ui-core/alaska-ui-core.module';
import { LogsDashboardComponent } from './components/logs-dashboard/logs-dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    InfiniteScrollModule,
    AlaskaUiCoreModule
  ],
  declarations: [
    LogsDashboardComponent
  ],
  exports: [
    InfiniteScrollModule,
    LogsDashboardComponent
  ],
  entryComponents: [
  ]
})
export class AlaskaUiLogsModule { }

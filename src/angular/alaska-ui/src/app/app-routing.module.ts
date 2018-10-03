import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CacheAdminComponent, LogsDashboardComponent } from 'alaska-ui';

const routes: Routes = [
  {
    path: 'cache',
    component: CacheAdminComponent
  },
  {
    path: 'logs',
    component: LogsDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

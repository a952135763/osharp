import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestshoworderComponent } from './test/testshoworder.component'

const routes: Routes = [
  { path: 'test/:orderid', component: TestshoworderComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShoworderRoutingModule { }

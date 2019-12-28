import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { ShoworderRoutingModule } from './showorder.routing';
import { TestshoworderComponent } from './test/testshoworder.component';
import { CountDownModule }from '@delon/abc';
import { CountdownModule } from 'ngx-countdown';

@NgModule({
  imports: [CommonModule, SharedModule,ShoworderRoutingModule,CountDownModule,CountdownModule],
  declarations: [TestshoworderComponent],
})
export class ShoworderModule {}
